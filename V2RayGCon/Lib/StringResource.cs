using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace V2RayGCon.Lib
{
    public class StringResource
    {
        static ResourceManager ResMgr(string resFileName)
        {
            return new ResourceManager(resFileName, Assembly.GetExecutingAssembly());
        }

        static Func<string, string> StringLoader(string resFileName)
        {
            ResourceManager resources = ResMgr(resFileName);
            return key =>
            {
                var value = resources.GetString(key);
                if (value == null)
                {
                    throw new KeyNotFoundException($"key: {key}");
                }
                return value;
            };
        }

        public static Func<string, string> I18N = StringLoader(Properties.Resources.Text);
        public static Func<string, string> resData = StringLoader(Properties.Resources.Data);
    }
}
