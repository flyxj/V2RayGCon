using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using static V2RayGCon.Lib.StringResource;


namespace V2RayGCon.Controller.Configer
{
    class SSServer : INotifyPropertyChanged
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

        private string _port;

        public string port
        {
            get { return _port; }
            set
            {
                SetField(ref _port, value);
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

        private int _network;

        public int network
        {
            get { return _network; }
            set { SetField(ref _network, value); }
        }

        private bool _OTA;

        public bool OTA
        {
            get { return _OTA; }
            set { SetField(ref _OTA, value); }
        }

        public void SetMethod(string selectedMethod)
        {
            method = Lib.Utils.LookupDict(
                Model.Table.ssMethods,
                selectedMethod);
        }

        public void SetNetwork(string selectedNetwork)
        {
            network = Lib.Utils.LookupDict(
                Model.Table.ssNetworks,
                selectedNetwork);
        }

        public JToken GetSettings()
        {
            var configTemplate = JObject.Parse(resData("config_tpl"));
            JToken server = configTemplate["ssServer"];

            var methods = Model.Table.ssMethods;
            var networks = Model.Table.ssNetworks;

            server["port"] = Lib.Utils.Str2Int(port);
            server["settings"]["method"] = method < 0 ? methods[0] : methods[method];
            server["settings"]["password"] = pass;
            server["settings"]["email"] = email;
            server["settings"]["ota"] = OTA;
            server["settings"]["network"] = network < 0 ? networks[0] : networks[network];

            return server;
        }

        public void UpdateData(JObject config)
        {
            var GetStr = Lib.Utils.ClosureGetStringFromJToken(config);
            var perfix = "inbound.settings.";
            port = GetStr("inbound.", "port");
            SetMethod(GetStr(perfix, "method"));
            SetNetwork(GetStr(perfix, "network"));
            pass = GetStr(perfix, "password");
            email = GetStr(perfix, "email");
            OTA = Lib.Utils.GetBoolFromJToken(config, perfix + "ota");
        }

    }
}
