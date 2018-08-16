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
            List<string> urls = null;
            var empty = new List<string>();
            var import = Lib.Utils.GetKey(config, "v2raygcon.import");
            if (import != null && import is JObject)
            {
                urls = (import as JObject).Properties().Select(p => p.Name).ToList();
            }
            return urls ?? new List<string>();
        }

        /*
         * exceptions  
         * test<FormatException> base64 decode fail
         * test<System.Net.WebException> url not exist
         * test<Newtonsoft.Json.JsonReaderException> json decode fail
         */
        public static string ParseImport(string configString, bool cleanup = false)
        {
            var maxDepth = Lib.Utils.Str2Int(StrConst("ParseImportDepth"));

            var result = ParseImportRecursively(
                GetContentFromCache,
                configString,
                maxDepth);

            var config = JObject.Parse(result);

            try
            {
                Lib.Utils.RemoveKeyFromJObject(config, "v2raygcon.import");
            }
            catch (KeyNotFoundException)
            {
                // do nothing;
            }

            var s = config.ToString();

            if (cleanup)
            {
                config = null;
                result = null;
                GC.Collect();
            }

            return s;
        }

        public static string ParseImportRecursively(
            Func<List<string>, List<string>> fetcher,
            string configString,
            int depth)
        {
            var empty = @"{}";

            if (depth <= 0)
            {
                return empty;
            }

            var config = JObject.Parse(configString);

            var urls = GetImportUrls(config);
            var contents = fetcher(urls);

            if (contents.Count <= 0)
            {
                return configString;
            }

            var configList =
                Lib.Utils.ExecuteInParallel<string, string>(
                    contents,
                    (content) =>
                    {
                        return ParseImportRecursively(
                            fetcher,
                            content,
                            depth - 1);
                    });

            var result = JObject.Parse(empty);
            foreach (var c in configList)
            {
                result = Lib.Utils.CombineConfig(result, JObject.Parse(c));
            }

            return Lib.Utils.CombineConfig(result, config).ToString();
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
            return Lib.Utils.CombineConfig(config, JObject.Parse(overwrite));
        }

        static List<string> GetContentFromCache(List<string> urls)
        {
            return urls.Count <= 0 ?
                urls :
                Lib.Utils.ExecuteInParallel<string, string>(
                    urls,
                    (url) => Service.Cache.Instance.html[url]);
        }

        #endregion

    }
}
