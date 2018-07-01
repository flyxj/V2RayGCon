using Newtonsoft.Json.Linq;
using static V2RayGCon.Lib.StringResource;



namespace V2RayGCon.Controller.Configer
{
    class VmessCtrl :
        Model.BaseClass.NotifyComponent,
        Model.BaseClass.IConfigerComponent
    {
        public VmessCtrl()
        {
            serverMode = false;
        }

        #region properties
        public bool serverMode;

        private string _ID;
        public string ID
        {
            get { return _ID; }
            set { SetField(ref _ID, value); }
        }

        private string _level;
        public string level
        {
            get { return _level; }
            set { SetField(ref _level, value); }
        }

        private string _altID;
        public string altID
        {
            get { return _altID; }
            set { SetField(ref _altID, value); }
        }

        private string _blackhole;
        private string _ip;
        private int _port;
        public string addr
        {
            get { return string.Join(":", _ip, _port); }
            set
            {
                Lib.Utils.TryParseIPAddr(value, out _ip, out _port);
                SetField(ref _blackhole, value);
            }
        }
        #endregion

        #region public method
        public JToken GetSettings()
        {
            var configTemplate = JObject.Parse(resData("config_tpl"));

            JToken vmess = serverMode ?
                configTemplate["vmessServer"] :
                configTemplate["vmessClient"];

            if (serverMode)
            {
                vmess["port"] = _port;
                vmess["listen"] = _ip;
                vmess["settings"]["clients"][0]["id"] = ID;
                vmess["settings"]["clients"][0]["level"] = Lib.Utils.Str2Int(level);
                vmess["settings"]["clients"][0]["alterId"] = Lib.Utils.Str2Int(altID);
            }
            else
            {
                vmess["vnext"][0]["address"] = _ip;
                vmess["vnext"][0]["port"] = _port;
                vmess["vnext"][0]["users"][0]["id"] = ID;
                vmess["vnext"][0]["users"][0]["alterId"] = Lib.Utils.Str2Int(altID);
                vmess["vnext"][0]["users"][0]["level"] = Lib.Utils.Str2Int(level);
            }

            return vmess;
        }

        public void UpdateData(JObject config)
        {
            var prefix = serverMode ?
                "inbound.settings.clients.0" :
                "outbound.settings.vnext.0.users.0";

            ID = Lib.Utils.GetValue<string>(config, prefix, "id");
            level = Lib.Utils.GetValue<int>(config, prefix, "level").ToString();
            altID = Lib.Utils.GetValue<int>(config, prefix, "alterId").ToString();

            addr = serverMode ?
                Lib.Utils.GetAddr(config, "inbound", "listen", "port") :
                Lib.Utils.GetAddr(config, "outbound.settings.vnext.0", "address", "port");
        }
        #endregion
    }
}
