using Newtonsoft.Json.Linq;

namespace V2RayGCon.Controller.Configer
{
    class StreamSettings :
        Model.BaseClass.NotifyComponent,
        Model.BaseClass.IConfigerComponent
    {
        Service.Cache cache;
        public StreamSettings()
        {
            cache = Service.Cache.Instance;
            _isServer = false;
        }

        #region properties
        private int _streamType;
        public int streamType
        {
            get { return _streamType; }
            set { SetField(ref _streamType, value); }
        }

        private string _streamParamText;
        public string streamParamText
        {
            get { return _streamParamText; }
            set { SetField(ref _streamParamText, value); }
        }

        private int _tls;
        public int tls
        {
            get { return _tls; }
            set { SetField(ref _tls, value); }
        }

        private bool _isServer;
        public bool isServer
        {
            get { return _isServer; }
            set { _isServer = value; }
        }
        #endregion

        #region public method
        public JToken GetSettings()
        {
            streamType = Lib.Utils.Clamp(
                streamType,
                0,
                Model.Data.Table.streamSettings.Count);

            var key = "none";
            var s = Model.Data.Table.streamSettings[streamType];
            if (s.dropDownStyle && s.options.ContainsKey(streamParamText))
            {
                key = streamParamText;
            }

            var tpl = cache.tpl.LoadTemplate(s.options[key]) as JObject;
            if (!s.dropDownStyle)
            {
                Lib.Utils.SetValue<string>(tpl, s.optionPath, streamParamText);
            }

            InsertTLSSettings(tpl);
            return tpl;
        }

        public void UpdateData(JObject config)
        {
            var GetStr = Lib.Utils.GetStringByPrefixAndKeyHelper(config);

            string prefix = isServer ?
                "inbound.streamSettings" :
                "outbound.streamSettings";

            var index = GetIndexByNetwork(GetStr(prefix, "network"));
            streamType = index;
            streamParamText = index < 0 ? string.Empty :
                GetStr(
                    prefix,
                    Model.Data.Table.streamSettings[index].optionPath);

            tls = GetStr(prefix, "security") == "tls" ? 1 : 0;
        }
        #endregion

        #region private method
        int GetIndexByNetwork(string network)
        {
            if (string.IsNullOrEmpty(network))
            {
                return -1;
            }

            foreach (var item in Model.Data.Table.streamSettings)
            {
                if (item.Value.network == network)
                {
                    return item.Key;
                }
            }

            return -1;
        }

        void InsertTLSSettings(JToken streamSettings)
        {
            var tlsTpl = cache.tpl.LoadTemplate("tls");
            if (tls <= 0)
            {
                streamSettings["security"] = string.Empty;
            }
            else
            {
                var streamSecurity = Model.Data.Table.streamTLS;
                streamSettings["security"] = streamSecurity[tls];
                streamSettings["tlsSettings"] = tlsTpl;
            }
        }
        #endregion

    }

}
