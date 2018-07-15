using System.Collections.Generic;
using System.Net;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service.Caches
{

    public class HTML
    {
        // main lock
        object writeLock;

        // url=(rwLock, content)
        Dictionary<string, Model.Data.LockStrPair> data;

        public HTML()
        {
            writeLock = new object();
            data = new Dictionary<string, Model.Data.LockStrPair>();
        }

        #region public method
        public void RemoveCache(List<string> urls)
        {
            lock (writeLock)
            {
                foreach (var url in urls)
                {
                    if (data.ContainsKey(url))
                    {
                        lock (data[url].rwLock)
                        {
                            data.Remove(url);
                        }
                    }
                }

            }
        }

        public void RemoveAllCache()
        {
            var urls = new List<string>(data.Keys);
            RemoveCache(urls);
        }

        public string GetCache(string url)
        {
            lock (writeLock)
            {
                if (!data.ContainsKey(url))
                {
                    data[url] = new Model.Data.LockStrPair();
                }
            }

            var c = data[url];
            lock (c.rwLock)
            {
                var timeout = Lib.Utils.Str2Int(
                        StrConst("ParseImportTimeOut"));

                for (var i = 0; i < 2 && string.IsNullOrEmpty(c.content); i++)
                {
                    c.content = Lib.Utils.Fetch(url, timeout * 1000);
                }
            }

            if (string.IsNullOrEmpty(c.content))
            {
                throw new WebException("Download fail!");
            }
            return c.content;
        }
        #endregion

        #region private method


        #endregion
    }
}
