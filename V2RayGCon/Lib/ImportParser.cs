using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Lib
{
    public class ImportParser
    {
        #region public method
        public static List<string> GetImportUrls(JObject config)
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

        /*
         * exceptions  
         * test<FormatException> base64 decode fail
         * test<System.Net.WebException> url not exist
         * test<Newtonsoft.Json.JsonReaderException> json decode fail
         */
        public static JObject ParseImport(JObject config, int timeout = -1)
        {
            var maxDepth = Lib.Utils.Str2Int(resData("ParseImportDepth"));

            var result = ParseImportRecursively(
                (urls) => FetchAllUrls(urls, timeout),
                config,
                maxDepth);

            Lib.Utils.RemoveKeyFromJObject(result, "v2raygcon.import");
            return result;
        }

        public static JObject ParseImportRecursively(
            Func<List<string>, List<string>> fetcher,
            JObject config,
            int depth)
        {
            var result = JObject.Parse(@"{}");

            if (depth <= 0)
            {
                return result;
            }

            var urls = GetImportUrls(config);
            var contents = fetcher(urls);

            if (contents.Count <= 0)
            {
                return config.DeepClone() as JObject;
            }

            var configList =
                Lib.Utils.ExecuteInParallel<string, JObject>(
                    contents,
                    (content) =>
                    {
                        return ParseImportRecursively(
                            fetcher,
                            JObject.Parse(content),
                            depth - 1);
                    });

            foreach (var c in configList)
            {
                result = Lib.Utils.MergeConfig(result, c);
            }

            return Lib.Utils.MergeConfig(result, config);
        }
        #endregion

        #region private method
        static JObject OverwriteConfig(JObject config, string overwrite)
        {
            var o = overwrite;
            if (string.IsNullOrEmpty(o))
            {
                return config;
            }
            return Lib.Utils.MergeConfig(config, JObject.Parse(overwrite));
        }

        static List<string> FetchAllUrls(List<string> urls, int timeout)
        {
            if (urls.Count <= 0)
            {
                return new List<string>();
            }

            var retry = Lib.Utils.Str2Int(resData("ParseImportRetry"));

            return Lib.Utils.ExecuteInParallel<string, string>(
                urls,
                (url) =>
                {
                    var html = string.Empty;

                    for (var i = 0;
                    i < retry && string.IsNullOrEmpty(html);
                    i++)
                    {
                        html = Lib.Utils.Fetch(url, timeout, true);
                    }

                    return html;
                });
        }

        #endregion

    }
}
