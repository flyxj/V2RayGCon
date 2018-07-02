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
        private string _urls,_overwrite,_linkEncode,_linkDecode;

        public string urls
        {
            get { return _urls; }
            set { SetField(ref _urls, FormatUrls(value)); }
        }

        public string overwrite
        {
            get { return _overwrite; }
            set { SetField(ref _overwrite, value); }
        }

        public string linkDecode
        {
            get { return _linkDecode; }
            set { SetField(ref _linkDecode, value); }
        }

        public string linkEncode
        {
            get { return _linkEncode; }
            set { SetField(ref _linkEncode, value); }
        }

        #endregion

        #region private method
        JObject MergeOnlineConfig(JObject config,string urls)
        {
            if (!string.IsNullOrEmpty(urls))
            {
                foreach (var url in urls.Split('\n'))
                {
                    var content = Lib.Utils.Fetch(url);
                    var j = JObject.Parse(content);
                    config = Lib.Utils.MergeJson(config, j);
                }
            }
            return config;
        }

        JObject OverwriteConfig(JObject config,string overwrite)
        {
            var o = overwrite;
            if (string.IsNullOrEmpty(o))
            {
                return config;
            }

            return Lib.Utils.MergeJson(config, JObject.Parse(overwrite));
        }

        string FormatUrls(string raw_urls)
        {
            var uarr = raw_urls.Replace("\r", "").Replace(",", "\n").Split('\n');
            var result = new List<string>();
            foreach(var u in uarr)
            {
                if (!string.IsNullOrEmpty(u))
                {
                    result.Add(u);
                }
            }
            return result.Count > 0 ? string.Join("\n",result):string.Empty;
        }
        #endregion

        #region public method
        public JObject DecodeLink()
        {
            var b64Link = Lib.Utils.GetLinkBody(linkDecode);
            var l = Lib.Utils.Base64Decode(b64Link);
            
            var c = JObject.Parse(@"{}");

            var v = JObject.Parse(l);
            var u = Lib.Utils.GetValue<string>(v, "u");
            if (!string.IsNullOrEmpty(u))
            {
                c = MergeOnlineConfig(c, u);
            }

            var o = Lib.Utils.GetValue<string>(v, "o");
            if (!string.IsNullOrEmpty(o))
            {
                c = OverwriteConfig(c, o);
            }

            return c;
        }

        public JToken GetSettings()
        {
            var c = JObject.Parse(@"{}");
            c = MergeOnlineConfig(c,urls);
            c = OverwriteConfig(c,overwrite);
            return c;
        }

        public void UpdateData(JObject config)
        {
            // do not need update
            var v = JObject.Parse(@"{}");

            if (!string.IsNullOrEmpty(urls)) {
                v["u"] = urls;
            }

            if (!string.IsNullOrEmpty(overwrite)) {
                v["o"] = overwrite;
            }

            try
            {
                var b64Link = Lib.Utils.Base64Encode(v.ToString());
                linkEncode = Lib.Utils.AddLinkPrefix(b64Link, Model.Data.Enum.LinkTypes.v);
            }
            catch {
                linkEncode = string.Empty;
            }
        }
        #endregion
    }
}
