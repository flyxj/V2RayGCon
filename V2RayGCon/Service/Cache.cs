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

        Cache()
        {
            writeLock = new Dictionary<string, object>();
            data = new Dictionary<string, object>();
            json = new Dictionary<string, JObject> {
                { "template",JObject.Parse(StrConst("config_tpl"))},
                { "example",JObject.Parse(StrConst("config_def"))},
                { "minConfig",JObject.Parse(StrConst("config_min"))},
            };
        }

        #region public method
        public void UpdateHTMLCache(string url, string html)
        {
            if (html == null || string.IsNullOrEmpty(html))
            {
                return;
            }

            var cache = GetCache<string>(StrConst("CacheHTML"));

            lock (cache.Item1)
            {
                cache.Item2[url] = html;
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
                    d.Remove(key);
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
