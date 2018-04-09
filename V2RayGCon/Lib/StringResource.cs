using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace V2RayGCon.Lib
{
    class StringResource
    {
        static ResourceManager ResMgr(string resFileName)
        {
            return new ResourceManager(resFileName, Assembly.GetExecutingAssembly());
        }

        static Func<string, string> StringLoader(string resFileName)
        {
            // Debug.WriteLine("Filename: " + resFileName);
            ResourceManager resources = ResMgr(resFileName);
            return (key) =>
            {
                // Debug.WriteLine("key: " + key);
                return resources.GetString(key);
            };
        }

        public static Func<string, string> I18N = StringLoader(Properties.Resources.Text);
        public static Func<string, string> resData = StringLoader(Properties.Resources.Data);
    }
}
