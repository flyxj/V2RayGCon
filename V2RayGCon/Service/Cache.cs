using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    public class Cache : Model.BaseClass.SingletonService<Cache>
    {
        Dictionary<string, object> writeLock;
        Dictionary<string, object> data;
        Dictionary<string, JObject> json;
        Dictionary<string, string> decode;
        object writeDecodeCacheLock;
        public Caches.HTML html;

        Cache()
        {
            writeDecodeCacheLock = new object();
            writeLock = new Dictionary<string, object>();
            data = new Dictionary<string, object>();
            decode = LoadDecodeCache();
            json = new Dictionary<string, JObject> {
                { "template",JObject.Parse(StrConst("config_tpl"))},
                { "example",JObject.Parse(StrConst("config_def"))},
                { "minConfig",JObject.Parse(StrConst("config_min"))},
            };
            html = new Caches.HTML();
        }

        #region public method
        public void ClearDecodeCache()
        {
            lock (writeDecodeCacheLock)
            {
                decode = new Dictionary<string, string>();
                SaveDecodeCache();
            }
        }

        public string GetDecodeCache(string configString)
        {
            if (decode.ContainsKey(configString))
            {
                return decode[configString];
            }

            return string.Empty;
        }

        public void UpdateDecodeCache(string configString, string decodedString)
        {
            try
            {
                JObject.Parse(decodedString);
            }
            catch
            {
                return;
            }

            lock (writeDecodeCacheLock)
            {
                var keys = new List<string>(decode.Keys);
                var cacheSize = Lib.Utils.Str2Int(StrConst("DecodeCacheSize"));

                for (var i = 0; i < keys.Count - cacheSize; i++)
                {
                    if (decode.ContainsKey(keys[i]))
                    {
                        decode.Remove(keys[i]);
                    }
                }

                decode[configString] = decodedString;
                SaveDecodeCache();
            }
        }

        public void ClearSummariesCache()
        {
            var summary = GetCache<string[]>(StrConst("CacheSummary"));
            lock (summary.Item1)
            {
                var d = summary.Item2 as Dictionary<string, string[]>;
                var keys = new List<string>(d.Keys);
                foreach (var key in keys)
                {
                    if (d.ContainsKey(key))
                    {
                        d.Remove(key);
                    }
                }
            }
        }

        public JToken LoadTemplate(string key)
        {
            return LoadJObjectPart(json["template"], key);
        }

        public JObject LoadMinConfig()
        {
            return json["minConfig"].DeepClone() as JObject;
        }

        public JToken LoadExample(string key)
        {
            return LoadJObjectPart(json["example"], key);
        }

        public void RemoveFromCache<T>(string cacheName, List<string> keys)
        {
            if (!data.ContainsKey(cacheName))
            {
                return;
            }

            lock (writeLock[cacheName])
            {
                var d = data[cacheName] as Dictionary<string, T>;

                foreach (var key in keys)
                {
                    if (d.ContainsKey(key))
                    {
                        d.Remove(key);
                    }
                }
            }
        }

        public Tuple<object, Dictionary<string, T>> GetCache<T>(string cacheName)
        {
            if (!writeLock.ContainsKey(cacheName))
            {
                CreateCache<T>(cacheName);
            }

            return new Tuple<object, Dictionary<string, T>>(
                writeLock[cacheName],
                data[cacheName] as Dictionary<string, T>);
        }
        #endregion

        #region private method
        Dictionary<string, string> LoadDecodeCache()
        {
            var result = new Dictionary<string, string>();
            var decodeRawStr = Properties.Settings.Default.DecodeCache;
            try
            {
                var temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodeRawStr);
                foreach (var item in temp)
                {
                    try
                    {
                        JObject.Parse(item.Value);
                        result[item.Key] = item.Value;
                    }
                    catch
                    {
                        // continue
                    }
                }
            }
            catch { }
            return result;
        }

        void SaveDecodeCache()
        {
            string json = JsonConvert.SerializeObject(decode);
            Properties.Settings.Default.DecodeCache = json;
            Properties.Settings.Default.Save();
        }

        JToken LoadJObjectPart(JObject source, string path)
        {
            var result = Lib.Utils.GetKey(source, path);
            if (result == null)
            {
                throw new JsonReaderException();
            }
            return result.DeepClone();
        }

        void CreateCache<T>(string cacheName)
        {
            writeLock[cacheName] = new object();
            data[cacheName] = new Dictionary<string, T>();
        }
        #endregion
    }
}
