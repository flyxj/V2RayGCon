using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Lib
{
    public class ImportParser
    {
        static List<string> GetImportUrls(JObject config)
        {
            var result = new List<string>();
            var import = Lib.Utils.GetKey(config, "v2raygcon.import");
            if (import != null && import is JObject)
            {
                var urls = ((JObject)import).Properties().Select(p => p.Name).ToList();

                if (urls == null)
                {
                    return result;
                }

                return urls;
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
            if (urls.Count <= 0)
            {
                return new List<string>();
            }

            var retry = Lib.Utils.Str2Int(resData("ParseImportRetry"));

            var tasks = new List<Task<string>>();
            foreach (var url in urls)
            {
                var task = new Task<string>(
                    () =>
                    {
                        var html = string.Empty;

                        for (var i = 0;
                        i < retry && string.IsNullOrEmpty(html);
                        i++)
                        {
                            html = Lib.Utils.Fetch(url, timeout);
                        }

                        return html;
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

        static void ClearImport(JObject config)
        {
            var import = Lib.Utils.GetKey(config, "v2raygcon.import");
            if (import != null)
            {
                ((JObject)config["v2raygcon"]).Property("import")?.Remove();
            }
        }

        /*
         * exceptions  
         * test<FormatException> base64 decode fail
         * test<System.Net.WebException> url not exist
         * test<Newtonsoft.Json.JsonReaderException> json decode fail
         */
        public static JObject ParseImport(JObject config, int timeout = -1)
        {
            var maxDepth = Lib.Utils.Str2Int(resData("ParseImportDepth"));

            List<string> GetContent(List<string> urls)
            {
                return FetchAllUrls(urls, timeout);
            }


            var cfg = ParseImportRecursively(config, maxDepth, GetContent);


            // ClearImport(cfg);

            return cfg;
        }

        static JObject ParseImportRecursively(JObject config, int depth, Func<List<string>, List<string>> fetcher)
        {
            var cfg = JObject.Parse(@"{}");
            if (depth <= 0)
            {
                return cfg;
            }

            var urls = GetImportUrls(config);
            foreach (var content in fetcher(urls))
            {
                var c = ParseImportRecursively(
                    JObject.Parse(content),
                    depth - 1,
                    fetcher);

                cfg = Lib.Utils.MergeJson(cfg, c);
            }

            return Lib.Utils.MergeJson(config, cfg);
        }

    }
}
