using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service.Caches
{
    public class Template
    {
        JObject template, example, package;

        public Template()
        {
            template = JObject.Parse(StrConst("config_tpl"));
            example = JObject.Parse(StrConst("config_def"));
            package = JObject.Parse(StrConst("config_pkg"));
        }

        #region public method
        public JObject LoadPackage(string key)
        {
            var node = LoadJObjectPart(package, key);
            return JObject.Parse(node.ToString());
        }

        public JToken LoadTemplate(string key)
        {
            var node = LoadJObjectPart(template, key);
            return JToken.Parse(node.ToString());
        }

        public JObject LoadMinConfig()
        {
            return JObject.Parse(StrConst("config_min"));
        }

        public JToken LoadExample(string key)
        {
            var node = LoadJObjectPart(example, key);
            return JToken.Parse(node.ToString());
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
