using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using static V2RayGCon.Lib.StringResource;


namespace V2RayGCon.Controller.Configer
{
    class SSRClient : INotifyPropertyChanged
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

        private string _email;

        public string email
        {
            get { return _email; }
            set { SetField(ref _email, value); }
        }

        private string _addr;
        private string _ip;
        private int _port;

        public string addr
        {
            get { return string.Join(":", _ip, _port); }
            set
            {
                Lib.Utils.TryParseIPAddr(value, out _ip, out _port);
                SetField(ref _addr, value);
            }
        }

        private string _pass;

        public string pass
        {
            get { return _pass; }
            set { SetField(ref _pass, value); }
        }

        private int _method;

        public int method
        {
            get { return _method; }
            set { SetField(ref _method, value); }
        }

        private bool _OTA;

        public bool OTA
        {
            get { return _OTA; }
            set { SetField(ref _OTA, value); }
        }

        public void SetMethod(string selectedMethod)
        {
            var methods = Model.Table.ssrMethods;
            int index = 0;

            foreach (var m in methods)
            {
                if (m.Value.Equals(selectedMethod))
                {
                    index = m.Key;
                    break;
                }
            }
            method = index;
        }

        public JToken GetSettings()
        {
            var configTemplate = JObject.Parse(resData("config_tpl"));
            JToken client = configTemplate["ssrClient"];
            var ssrMethods = Model.Table.ssrMethods;

            client["servers"][0]["email"] = email;
            client["servers"][0]["address"] = _ip;
            client["servers"][0]["port"] = _port;
            client["servers"][0]["method"] = ssrMethods[method];
            client["servers"][0]["password"] = pass;
            client["servers"][0]["ota"] = OTA;

            return client;
        }

    }
}
