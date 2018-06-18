using Newtonsoft.Json.Linq;

using static V2RayGCon.Lib.StringResource;


namespace V2RayGCon.Controller.Configer
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

        #region public method


        public JToken GetSettings()
        {
            var configTemplate = JObject.Parse(resData("config_tpl"));
            JToken vgc = configTemplate["vgc"];

            vgc["alias"] = alias;
            vgc["description"] = description;

            return vgc;
        }

        public void UpdateData(JObject config)
        {
            var prefix = "v2raygcon";

            alias = Lib.Utils.GetValue<string>(config, prefix, "alias");
            description = Lib.Utils.GetValue<string>(config, prefix, "description");
        }
        #endregion
    }
}
