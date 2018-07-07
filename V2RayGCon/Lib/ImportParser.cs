using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2RayGCon.Lib
{
    public class ImportParser
    {

        static List<string> GetImportUrls(JObject config)
        {
            var result = new List<string>();
            var import = Lib.Utils.GetKey(config, "v2raygcon.import");
            if(import != null && import is JArray)
            {
                var urls = import.ToObject<string[]>();
                for(var i = 0; i < urls.Length; i++)
                {
                    var url = urls[i];
                    if (url.Length > 0 && !result.Contains(url))
                    {
                        result.Add(url);
                    }
                }
            }
            return result;
        }

        static JObject OverwriteConfig(JObject config, string overwrite)
        {
            var o = overwrite;
            if (string.IsNullOrEmpty(o))
            {
                return config;
            }
            return Lib.Utils.MergeJson(config, JObject.Parse(overwrite));
        }

        static List<string> FetchAllUrls(List<string> urls, int timeout)
        {
            var tasks = new List<Task<string>>();
            foreach (var url in urls)
            {
                var task = new Task<string>(() =>
                {
                    var content = Lib.Utils.Fetch(url, timeout);
                    return content;
                });
                tasks.Add(task);
                task.Start();
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                // throw ae.Flatten();
                foreach (var e in ae.InnerExceptions)
                {
                    throw e;
                }
            }
            
            var result = new List<string>();
            foreach (var task in tasks)
            {
                result.Add(task.Result);
            }
            return result;
        }

        static JObject MergeOnlineConfig(JObject config, List<string> urls, int timeout)
        {
            if (urls.Count<=0)
            {
                return config;
            }

            var contents = FetchAllUrls(urls, timeout);

            foreach (var content in contents)
            {
                var cfg = JObject.Parse(content);
                config = Lib.Utils.MergeJson(config, cfg);
            }

            return config;
        }

        /*
         * exceptions  
         * test<FormatException> base64 decode fail
         * test<System.Net.WebException> url not exist
         * test<Newtonsoft.Json.JsonReaderException> json decode fail
         */
        public static JObject ParseImport(JObject config,int timeout=-1)
        {
            var urls = GetImportUrls(config);
            var cfgTpl = MergeOnlineConfig(
                JObject.Parse(@"{}"),
                urls, 
                timeout);

            var cfg = Lib.Utils.MergeJson(cfgTpl, config);

            var import = Lib.Utils.GetKey(cfg, "v2raygcon.import");
            if (import != null)
            {
                ((JObject)cfg["v2raygcon"]).Property("import")?.Remove();
            }
            return cfg;
        }

    }
}
