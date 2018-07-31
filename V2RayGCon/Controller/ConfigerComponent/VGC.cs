using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Controller.ConfigerComponet
{
    class VGC :
        Model.BaseClass.NotifyComponent,
        Model.BaseClass.IConfigerComponent
    {
        #region properties
        private string _alias;
        private string _description;

        public string alias
        {
            get { return _alias; }
            set { SetField(ref _alias, value); }
        }

        public string description
        {
            get { return _description; }
            set { SetField(ref _description, value); }
        }

        #endregion

        #region private method
        JToken GetSettings()
        {
            JToken vgc = Service.Cache.Instance.
                tpl.LoadTemplate("vgc");

            vgc["alias"] = alias;
            vgc["description"] = description;

            return vgc;
        }
        #endregion

        #region public method
        // text box [alias, description]
        public void Bind(List<Control> controls)
        {
            if (controls.Count != 2)
            {
                throw new ArgumentException();
            }

            var bs = new BindingSource();
            bs.DataSource = this;
            controls[0].DataBindings.Add("Text", bs, nameof(this.alias));
            controls[1].DataBindings.Add("Text", bs, nameof(this.description));
        }

        public JObject Inject(JObject config)
        {
            config["v2raygcon"] = GetSettings();
            return config;
        }

        public void Update(JObject config)
        {
            var prefix = "v2raygcon";

            alias = Lib.Utils.GetValue<string>(config, prefix, "alias");
            description = Lib.Utils.GetValue<string>(config, prefix, "description");
        }
        #endregion
    }
}
