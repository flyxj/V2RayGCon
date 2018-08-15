using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service.Caches
{
    public class Template
    {
        #region public method
        public JToken LoadTemplate(string key)
        {
            var tpl = JObject.Parse(StrConst("config_tpl"));
            var part = LoadJObjectPart(tpl, key);

            // memory neg optimize
            return JToken.Parse(part.ToString());
        }

        public JObject LoadMinConfig()
        {
            return JObject.Parse(StrConst("config_min"));
        }

        public JToken LoadExample(string key)
        {
            var tpl = JObject.Parse(StrConst("config_def"));
            var part = LoadJObjectPart(tpl, key);

            // memory neg optimize
            return JToken.Parse(part.ToString());
        }
        #endregion

        #region private method
        JToken LoadJObjectPart(JObject source, string path)
        {
            var result = Lib.Utils.GetKey(source, path);
            if (result == null)
            {
                throw new JsonReaderException();
            }
            return result;
        }
        #endregion
    }
}
