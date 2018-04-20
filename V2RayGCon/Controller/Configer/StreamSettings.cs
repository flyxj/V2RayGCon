using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static V2RayGCon.Lib.StringResource;



namespace V2RayGCon.Controller.Configer
{
    class StreamSettings : INotifyPropertyChanged
    {
        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

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

        Action IsInboundChanged;

        public StreamSettings(Action OnIsInboundChanged)
        {
            IsInboundChanged = OnIsInboundChanged;
        }

        private bool _isInbound;

        public bool isInbound
        {
            get { return _isInbound; }
            set
            {
                SetField(ref _isInbound, value);
                IsInboundChanged();
            }
        }

        public void SetSecurity(string security)
        {
            tls = Lib.Utils.LookupDict(Model.Table.streamSecurity, security);
        }

        string GetSecuritySetting()
        {
            var streamSecurity = Model.Table.streamSecurity;
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

            var perfix = "outbound.streamSettings.";

            if (isInbound)
            {
                perfix = "inbound.streamSettings.";
            }

            kcpType = GetStr(perfix, "kcpSettings.header.type");
            wsPath = GetStr(perfix, "wsSettings.path");
            SetSecurity(GetStr(perfix, "security"));
        }

    }

}
