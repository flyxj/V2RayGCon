using System.Collections.Generic;
using System.Linq;
using System.Net;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service.Caches
{

    public class HTML : Model.BaseClass.ICacheComponent<string, string>
    {
        // main lock
        object writeLock;

        // url=(rwLock, content)
        Dictionary<string, Model.Data.LockValuePair<string>> data;

        public HTML()
        {
            writeLock = new object();
            Clear();
        }

        #region public method
        public int Count
        {
            get => data.Count;
        }

        public string[] Keys
        {
            get => data.Keys.ToArray();
        }

        public void Remove(List<string> urls)
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

        public void Clear()
        {
            lock (writeLock)
            {
                data = new Dictionary<string, Model.Data.LockValuePair<string>>();
            }
        }

        public string this[string url]
        {
            get => GetCache(url);
        }


        #endregion

        #region private method
        string GetCache(string url)
        {
            lock (writeLock)
            {
                if (!data.ContainsKey(url))
                {
                    data[url] = new Model.Data.LockValuePair<string>();
                }
            }

            var c = data[url];
            lock (c.rwLock)
            {
                var timeout = Lib.Utils.Str2Int(
                    StrConst("ParseImportTimeOut"));

                var retry = Lib.Utils.Str2Int(
                    StrConst("ParseImportRetry"));

                for (var i = 0;
                    i < retry && string.IsNullOrEmpty(c.content);
                    i++)
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
    }
}
