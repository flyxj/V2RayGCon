using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Model.BaseClass
{
    interface IConfigerComponent
    {
        // inject component settings into config
        JObject Inject(JObject config);

        // bind UI controls with component
        void Bind(List<Control> controls);

        // update component settings from config
        void Update(JObject config);
    }
}
