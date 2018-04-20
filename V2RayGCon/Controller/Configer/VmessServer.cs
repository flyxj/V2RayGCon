using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static V2RayGCon.Lib.StringResource;



namespace V2RayGCon.Controller.Configer
{
    class VmessServer : INotifyPropertyChanged
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

        private string _port;

        public string port
        {
            get { return _port; }
            set
            {

                SetField(ref _port, value);
            }
        }

        public JToken GetSettings()
        {
            var configTemplate = JObject.Parse(resData("config_tpl"));
            JToken server = configTemplate["vmessServer"];

            server["port"] = Lib.Utils.Str2Int(port);
            server["settings"]["clients"][0]["id"] = ID;
            server["settings"]["clients"][0]["level"] = Lib.Utils.Str2Int(level);
            server["settings"]["clients"][0]["alterId"] = Lib.Utils.Str2Int(altID);

            return server;
        }

        public void UpdateData(JObject config)
        {
            var GetStr = Lib.Utils.ClosureGetStringFromJToken(config);

            var perfix = "inbound.settings.clients.0.";
            ID = GetStr(perfix, "id");
            level = GetStr(perfix, "level");
            altID = GetStr(perfix, "alterId");
            port = GetStr("inbound.", "port");
        }
    }
}
