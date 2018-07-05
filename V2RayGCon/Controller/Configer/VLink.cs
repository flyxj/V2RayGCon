using Newtonsoft.Json;
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
            set { SetField(ref _urls, value); }
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

        #region public method
        public JToken GetSettings()
        {
            var c = JObject.Parse(@"{}");
            var link = Lib.VLinkCodec.EncodeLink(urls, overwrite);
            return Lib.VLinkCodec.DecodeLink(link);
        }

        public void UpdateData(JObject config)
        {
            urls = Lib.VLinkCodec.TrimUrls(urls).Replace(",", "\n");
            linkEncode = Lib.VLinkCodec.EncodeLink(urls, overwrite);
        }
        #endregion
    }
}
