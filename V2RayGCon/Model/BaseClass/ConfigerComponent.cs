using Newtonsoft.Json.Linq;

namespace V2RayGCon.Model.BaseClass
{
    abstract class ConfigerComponent : NotifyComponent, IConfigerComponent
    {
        protected Controller.Configer container;

        // bind UI controls with component
        public void Bind(Controller.Configer container)
        {
            this.container = container;
        }

        // update component settings from config
        public abstract void Update(JObject config);
    }
}
