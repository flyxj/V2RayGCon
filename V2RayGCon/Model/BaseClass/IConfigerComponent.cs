using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V2RayGCon.Model.BaseClass
{
    interface IConfigerComponent
    {
        JToken GetSettings();
        void UpdateData(JObject config);
    }
}
