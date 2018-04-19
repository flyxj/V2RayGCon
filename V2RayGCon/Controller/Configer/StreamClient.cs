using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static V2RayGCon.Lib.StringResource;



namespace V2RayGCon.Controller.Configer
{
    class StreamClient : INotifyPropertyChanged
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

        public void SetSecurity(string security)
        {
            var streamSecurity = Model.Table.streamSecurity;
            int index = 0;

            foreach (var s in streamSecurity)
            {
                if (s.Value.Equals(security, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    index = s.Key;
                    break;
                }
            }

            tls = index;
        }

        string GetSecuritySetting()
        {
            var streamSecurity = Model.Table.streamSecurity;
            return tls == 0 ? string.Empty : streamSecurity[tls];
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

    }

}
