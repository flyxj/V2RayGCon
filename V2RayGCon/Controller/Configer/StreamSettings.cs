using Newtonsoft.Json.Linq;
using System;



namespace V2RayGCon.Controller.Configer
{
    class StreamSettings : Model.BaseClass.NotifyComponent
    {
        Service.Cache cache;
        public StreamSettings()
        {
            cache = Service.Cache.Instance;
            _isServer = false;
        }

        #region properties
        private int _kcpType;
        public int kcpType
        {
            get { return _kcpType; }
            set { SetField(ref _kcpType, value); }
        }

        private int _tcpType;
        public int tcpType
        {
            get { return _tcpType; }
            set { SetField(ref _tcpType, value); }
        }

        private string _wsPath;
        public string wsPath
        {
            get { return _wsPath; }
            set { SetField(ref _wsPath, value); }
        }

        private string _dsPath;
        public string domainSocketPath
        {
            get { return _dsPath; }
            set { SetField(ref _dsPath, value); }
        }

        private string _h2Path;
        public string h2Path
        {
            get { return _h2Path; }
            set { SetField(ref _h2Path, value); }
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
        public JToken GetKCPSetting()
        {
            // 0 -> none -> kcp
            // 1 -> srtp -> kcp_srtp
            // ...

            var key = "kcp";
            if (kcpType > 0)
            {
                key = "kcp_" + Model.Data.Table.kcpTypes[kcpType];
            }

            JToken stream = cache.tpl.LoadTemplate(key);
            InsertTLSSettings(stream);
            return stream;
        }

        public JToken GetH2Setting()
        {
            JToken stream = cache.tpl.LoadTemplate("h2");
            stream["httpSettings"]["path"] = h2Path;

            InsertTLSSettings(stream);
            return stream;
        }

        public JToken GetWSSetting()
        {
            JToken stream = cache.tpl.LoadTemplate("ws");
            stream["wsSettings"]["path"] = wsPath;

            InsertTLSSettings(stream);
            return stream;
        }

        public JToken GetDSockSetting()
        {
            JToken stream = cache.tpl.LoadTemplate("dsock");
            stream["dsSettings"]["path"] = domainSocketPath;

            InsertTLSSettings(stream);
            return stream;
        }

        public JToken GetTCPSetting()
        {
            // 0 -> none -> tcp
            // 1 -> http -> tcp_http
            var key = "tcp";
            if (tcpType > 0)
            {
                key = "tcp_" + Model.Data.Table.tcpTypes[tcpType];
            }

            var stream = cache.tpl.LoadTemplate(key);
            InsertTLSSettings(stream);
            return stream;
        }

        public void UpdateData(JObject config)
        {
            var GetStr = Lib.Utils.GetStringByPrefixAndKeyHelper(config);

            string prefix;

            if (_isServer)
            {
                prefix = "inbound.streamSettings";
            }
            else
            {
                prefix = "outbound.streamSettings";
            }

            h2Path = GetStr(prefix, "httpSettings.path");

            wsPath = GetStr(prefix, "wsSettings.path");

            domainSocketPath = GetStr(prefix, "dsSettings.path");

            tls = Math.Max(0, Lib.Utils.GetIndexIgnoreCase(
                Model.Data.Table.streamSecurity,
                GetStr(prefix, "security")));

            kcpType = Lib.Utils.GetIndexIgnoreCase(
                Model.Data.Table.kcpTypes,
                GetStr(prefix, "kcpSettings.header.type"));

            tcpType = Lib.Utils.GetIndexIgnoreCase(
                Model.Data.Table.tcpTypes,
                GetStr(prefix, "tcpSettings.header.type"));
        }
        #endregion

        #region private method
        void InsertTLSSettings(JToken streamSettings)
        {
            var tlsTpl = cache.tpl.LoadTemplate("tls");
            if (tls <= 0)
            {
                streamSettings["security"] = string.Empty;
            }
            else
            {
                var streamSecurity = Model.Data.Table.streamSecurity;
                streamSettings["security"] = streamSecurity[tls];
                streamSettings["tlsSettings"] = tlsTpl;
            }
        }
        #endregion

    }

}
