using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace V2RayGCon.Lib
{
    public class StringResource
    {
        static Dictionary<string, ResourceManager> res = new Dictionary<string, ResourceManager>{
            { "i18n",new ResourceManager(Properties.Resources.Text,Assembly.GetExecutingAssembly())},
            { "data",new ResourceManager(Properties.Resources.Data,Assembly.GetExecutingAssembly())},
        };

        static string LoadString(ResourceManager resMgr, string key)
        {
            var value = resMgr.GetString(key);
            if (value == null)
            {
                throw new KeyNotFoundException($"key: {key}");
            }
            return value;
        }

        public static string I18N(string key)
        {
            return LoadString(res["i18n"], key);
        }

        public static string resData(string key)
        {
            return LoadString(res["data"], key);
        }
    }
}
