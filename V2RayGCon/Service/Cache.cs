using System;
using System.Collections.Generic;

namespace V2RayGCon.Service
{
    class Cache : Model.BaseClass.SingletonService<Cache>
    {
        Dictionary<string, object> wLock;
        Dictionary<string, object> data;

        Cache()
        {
            wLock = new Dictionary<string, object>();
            data = new Dictionary<string, object>();
        }

        #region public method
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
