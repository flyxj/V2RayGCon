using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static V2RayGCon.Lib.StringResource;


namespace V2RayGCon.Controller.Configer
{
    class VLink:
        Model.BaseClass.NotifyComponent,
        Model.BaseClass.IConfigerComponent
    {
        #region properties
        private string _name,_urls,_overwrite,_link;

        public string name
        {
            get { return _name; }
            set { SetField(ref _name, value); }
        }

        public string urls
        {
            get { return _urls; }
            set { SetField(ref _urls, value); }
        }

        public string overwrite
        {
            get { return _overwrite; }
            set { SetField(ref _overwrite, value); }
        }

        public string link
        {
            get { return _link; }
            set { SetField(ref _link, value); }
        }

        #endregion

        #region private method
        void NormalizeUrls()
        {
            var uarr = urls.Replace("\r", "").Replace(",", "\n").Split('\n');
            var result = new List<string>();
            foreach(var u in uarr)
            {
                if (!string.IsNullOrEmpty(u))
                {
                    result.Add(u);
                }
            }
            urls = result.Count > 0 ? string.Join("\n",result):string.Empty;
        }
        #endregion

        #region public method

        public JToken GetSettings()
        {
            NormalizeUrls();
            var v = new JObject();
            v["o"] = overwrite;
            v["u"] = urls;
            v["n"] = name;
            return v as JToken;
        }

        public void UpdateData(JObject config)
        {
            // do not need update
        }
        #endregion
    }
}
