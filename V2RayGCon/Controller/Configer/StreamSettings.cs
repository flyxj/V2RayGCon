using Newtonsoft.Json.Linq;
using static V2RayGCon.Lib.StringResource;



namespace V2RayGCon.Controller.Configer
{
    class StreamSettings : Model.BaseClass.NotifyComponent
    {
        private string _kcpType;

        public string kcpType
        {
            get { return _kcpType; }
            set { SetField(ref _kcpType, value); }
        }

        private string _wsPath;

        public string wsPath
        {
            get { return _wsPath; }
            set { SetField(ref _wsPath, value); }
        }

        private int _tls;

        public int tls
        {
            get { return _tls; }
            set { SetField(ref _tls, value); }
        }

        public StreamSettings()
        {
            _isServer = false;
        }

        private bool _isServer;

        public bool isServer
        {
            get { return _isServer; }
            set { _isServer = value; }
        }


        public void SetSecurity(string security)
        {
            tls = Lib.Utils.LookupDict(Model.Data.Table.streamSecurity, security);
        }

        string GetSecuritySetting()
        {
            var streamSecurity = Model.Data.Table.streamSecurity;
            return tls <= 0 ? string.Empty : streamSecurity[tls];
        }

        public JToken GetKCPSetting()
        {
            var configTemplate = JObject.Parse(resData("config_tpl"));
            JToken stream = configTemplate["kcp"];
            stream["kcpSettings"]["header"]["type"] = kcpType;
            stream["security"] = GetSecuritySetting();
            return stream;
        }

        public JToken GetWSSetting()
        {
            var configTemplate = JObject.Parse(resData("config_tpl"));
            JToken stream = configTemplate["ws"];
            stream["wsSettings"]["path"] = wsPath;
            stream["security"] = GetSecuritySetting();
            return stream;
        }

        public JToken GetTCPSetting()
        {
            var configTemplate = JObject.Parse(resData("config_tpl"));
            JToken stream = configTemplate["tcp"];
            stream["security"] = GetSecuritySetting();
            return stream;
        }

        public void UpdateData(JObject config)
        {
            var GetStr = Lib.Utils.ClosureGetStringFromJToken(config);

            string perfix;

            if (_isServer)
            {
                perfix = "inbound.streamSettings.";
            }
            else
            {
                perfix = "outbound.streamSettings.";
            }

            kcpType = GetStr(perfix, "kcpSettings.header.type");
            wsPath = GetStr(perfix, "wsSettings.path");
            SetSecurity(GetStr(perfix, "security"));
        }

    }

}
