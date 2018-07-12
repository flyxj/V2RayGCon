using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    public class Cache : Model.BaseClass.SingletonService<Cache>
    {
        Dictionary<string, object> wLock;
        Dictionary<string, object> data;
        Dictionary<string, JObject> json;

        Cache()
        {
            wLock = new Dictionary<string, object>();
            data = new Dictionary<string, object>();
            json = new Dictionary<string, JObject> {
                { "template",JObject.Parse(resData("config_tpl"))},
                { "example",JObject.Parse(resData("config_def"))},
                { "minConfig",JObject.Parse(resData("config_min"))},
            };
        }

        #region public method

        JToken LoadJObjectPart(JObject source, string path)
        {
            var result = Lib.Utils.GetKey(source, path);
            if (result == null)
            {
                throw new JsonReaderException();
            }
            return result.DeepClone();
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

            lock (wLock[cacheName])
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
            if (!wLock.ContainsKey(cacheName))
            {
                CreateCache<T>(cacheName);
            }

            return new Tuple<object, Dictionary<string, T>>(
                wLock[cacheName],
                data[cacheName] as Dictionary<string, T>);
        }
        #endregion

        #region private method
        void CreateCache<T>(string cacheName)
        {
            wLock[cacheName] = new object();
            data[cacheName] = new Dictionary<string, T>();
        }
        #endregion
    }
}
