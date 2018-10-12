using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service.Caches
{
    public class CoreCache
    {
        object writeLock;
        Dictionary<string, string> data;

        public CoreCache()
        {
            data = LoadDecodeCache();
            writeLock = new object();
        }

        #region public method
        public void Clear()
        {
            lock (writeLock)
            {
                data = new Dictionary<string, string>();
                SaveDecodeCache();
            }
        }

        public string this[string configString]
        {
            get
            {
                if (!data.ContainsKey(configString))
                {
                    throw new KeyNotFoundException(
                        "Core decode cache do not contain this config.");
                }

                return data[configString];
            }
            set
            {
                UpdateValue(configString, value);
            }
        }


        #endregion

        #region private method
        void UpdateValue(string configString, string decodedString)
        {
            try
            {
                JObject.Parse(decodedString);
            }
            catch
            {
                return;
            }

            lock (writeLock)
            {
                TrimDownCache();
                data[configString] = decodedString;
                SaveDecodeCache();
            }
        }

        void TrimDownCache()
        {
            var keys = new List<string>(data.Keys);
            var cacheSize = Lib.Utils.Str2Int(StrConst.DecodeCacheSize);

            for (var i = 0; i < keys.Count - cacheSize; i++)
            {
                if (data.ContainsKey(keys[i]))
                {
                    data.Remove(keys[i]);
                }
            }
        }

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
            string json = JsonConvert.SerializeObject(data);
            Properties.Settings.Default.DecodeCache = json;
            Properties.Settings.Default.Save();
        }
        #endregion
    }
}
