using System;
using System.Collections.Generic;

namespace V2RayGCon.Service
{
    public class Cache : Model.BaseClass.SingletonService<Cache>
    {
        object gLock;

        // general
        Dictionary<string, object> wLocks;
        Dictionary<string, object> data;

        // special
        public Caches.HTML html;
        public Caches.Template tpl;
        public Caches.CoreCache core;

        Cache()
        {
            gLock = new object();
            wLocks = new Dictionary<string, object>();
            data = new Dictionary<string, object>();
            html = new Caches.HTML();
            tpl = new Caches.Template();
            core = new Caches.CoreCache();
        }

        #region public method
        public void Clear<T>(string cacheName)
        {
            var c = GetCache<T>(cacheName);

            lock (gLock)
            {
                var keys = c.Item2.Keys;
                var d = c.Item2 as Dictionary<string, T>;
                foreach (var key in keys)
                {
                    d.Remove(key);
                }
            }
        }

        public void Remove<T>(string cacheName, List<string> keys)
        {
            lock (gLock)
            {
                if (!wLocks.ContainsKey(cacheName))
                {
                    return;
                }

                lock (wLocks[cacheName])
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
        }

        public Tuple<object, Dictionary<string, T>> GetCache<T>(string cacheName)
        {
            lock (gLock)
            {
                if (!wLocks.ContainsKey(cacheName))
                {
                    CreateCache<T>(cacheName);
                }
            }

            return new Tuple<object, Dictionary<string, T>>(
                wLocks[cacheName],
                data[cacheName] as Dictionary<string, T>);
        }
        #endregion

        #region private method
        void CreateCache<T>(string cacheName)
        {
            wLocks[cacheName] = new object();
            data[cacheName] = new Dictionary<string, T>();
        }
        #endregion
    }
}
