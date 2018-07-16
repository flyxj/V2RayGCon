using System.Collections.Generic;
using System.Linq;

namespace V2RayGCon.Service.Caches
{
    public class GeneralCache<TKey, TValue> : Model.BaseClass.ICacheComponent<TKey, TValue>
    {
        object writeLock;
        Dictionary<TKey, Model.Data.LockValuePair<TValue>> data;

        public GeneralCache()
        {
            writeLock = new object();
            Clear();
        }

        #region public method
        public int Count
        {
            get => data.Count;
        }

        public TKey[] Keys
        {
            get => data.Keys.ToArray();
        }

        public void Remove(List<TKey> keys)
        {
            lock (writeLock)
            {
                foreach (var key in keys)
                {
                    if (data.ContainsKey(key))
                    {
                        lock (data[key].rwLock)
                        {
                            data.Remove(key);
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            lock (writeLock)
            {
                data = new Dictionary<
                    TKey,
                    Model.Data.LockValuePair<TValue>>();
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (data[key].rwLock)
                {
                    return data[key].content;
                }
            }
        }
        #endregion
    }
}
