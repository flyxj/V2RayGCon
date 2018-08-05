using Newtonsoft.Json.Linq;

namespace V2RayGCon.Model.BaseClass
{
    interface IConfigerComponent
    {
        // bind UI controls with component
        void Bind(Controller.Configer container);

        // update component settings from config
        void Update(JObject config);
    }
}
