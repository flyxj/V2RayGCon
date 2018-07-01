using Newtonsoft.Json.Linq;

namespace V2RayGCon.Model.BaseClass
{
    interface IConfigerComponent
    {
        JToken GetSettings();
        void UpdateData(JObject config);
    }
}
