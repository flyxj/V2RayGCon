using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V2RayGCon.Lib
{
    public class VLinkCodec
    {
        static JObject OverwriteConfig(JObject config, string overwrite)
        {
            var o = overwrite;
            if (string.IsNullOrEmpty(o))
            {
                return config;
            }
            return Lib.Utils.MergeJson(config, JObject.Parse(overwrite));
        }

        static List<string> FetchAllUrls(string urls, int timeout)
        {
            var tasks = new List<Task<string>>();
            foreach (var url in urls.Split(','))
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

        static JObject MergeOnlineConfig(JObject config, string urls, int timeout)
        {
            if (string.IsNullOrEmpty(urls))
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

        public static string TrimUrls(string raw_urls)
        {
            if (string.IsNullOrEmpty(raw_urls))
            {
                return string.Empty;
            }

            var uarr = raw_urls
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", ",")
                .Split(',');

            var result = new List<string>();
            foreach (var u in uarr)
            {
                if (!string.IsNullOrEmpty(u))
                {
                    result.Add(u);
                }
            }

            return result.Count > 0 ?
                string.Join(",", result) :
                string.Empty;
        }

        /*
         * exceptions  
         * Newtonsoft.Json.JsonReaderException
         */
        public static string EncodeLink(string urls, string overwrite)
        {
            var v = JObject.Parse(@"{}");
            var u = TrimUrls(urls);

            if (!string.IsNullOrEmpty(u))
            {
                v["u"] = u;
            }

            if (!string.IsNullOrEmpty(overwrite))
            {
                v["o"] = JObject.Parse(overwrite).ToString(Formatting.None);
            }

            var b64Link = Lib.Utils.Base64Encode(v.ToString(Formatting.None));
            return Lib.Utils.AddLinkPrefix(b64Link, Model.Data.Enum.LinkTypes.v);
        }

        /*
         * exceptions  
         * test<FormatException>("v://***");
         * test<System.Net.WebException>("v://eyJ1IjoiaHR0cDovL3N1by5pbS81NjlHYVQifQ==", 1);
         * test<Newtonsoft.Json.JsonReaderException>("v://aa");
         */
        public static JObject DecodeLink(string vlink, int timeout = -1)
        {
            var b64Link = Lib.Utils.GetLinkBody(vlink);
            var l = Lib.Utils.Base64Decode(b64Link);

            var c = JObject.Parse(@"{}");

            var v = JObject.Parse(l);
            var u = Lib.Utils.GetValue<string>(v, "u");
            if (!string.IsNullOrEmpty(u))
            {
                c = MergeOnlineConfig(c, u, timeout);
            }

            var o = Lib.Utils.GetValue<string>(v, "o");
            if (!string.IsNullOrEmpty(o))
            {
                c = OverwriteConfig(c, o);
            }

            return c;
        }
    }
}
