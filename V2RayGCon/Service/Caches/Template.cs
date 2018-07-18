using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service.Caches
{
    public class Template
    {
        Dictionary<string, JObject> json;

        public Template()
        {
            json = new Dictionary<string, JObject> {
                { "template",JObject.Parse(StrConst("config_tpl"))},
                { "example",JObject.Parse(StrConst("config_def"))},
                { "minConfig",JObject.Parse(StrConst("config_min"))},
            };
        }

        #region public method
        public JToken LoadTemplate(string key)
        {
            return LoadJObjectPart(json["template"], key);
        }

        public JObject LoadMinConfig()
        {
            return json["minConfig"].DeepClone() as JObject;
        }

        public JToken LoadExample(string key)
        {
            return LoadJObjectPart(json["example"], key);
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
            return result.DeepClone();
        }
        #endregion
    }
}
