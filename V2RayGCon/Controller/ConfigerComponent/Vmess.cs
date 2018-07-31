using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Controller.ConfigerComponet
{
    class Vmess :
        Model.BaseClass.NotifyComponent,
        Model.BaseClass.IConfigerComponent
    {
        public Vmess()
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

        #region private method
        JToken GetSettings()
        {
            JToken vmess = Service.Cache.Instance.
                tpl.LoadTemplate(serverMode ?
                "vmessServer" : "vmessClient");

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
                vmess["settings"]["vnext"][0]["address"] = _ip;
                vmess["settings"]["vnext"][0]["port"] = _port;
                vmess["settings"]["vnext"][0]["users"][0]["id"] = ID;
                vmess["settings"]["vnext"][0]["users"][0]["alterId"] = Lib.Utils.Str2Int(altID);
                vmess["settings"]["vnext"][0]["users"][0]["level"] = Lib.Utils.Str2Int(level);
            }

            return vmess;
        }
        #endregion

        #region public method
        // textbox [id, level, aid, ipaddr]
        public void Bind(List<Control> controls)
        {
            if (controls.Count != 4)
            {
                throw new ArgumentException();
            }

            foreach (var c in controls)
            {
                if (!(c is TextBox))
                {
                    throw new ArgumentException();
                }
            }

            var bs = new BindingSource();
            bs.DataSource = this;

            controls[0].DataBindings.Add("Text", bs, nameof(this.ID));
            controls[1].DataBindings.Add("Text", bs, nameof(this.level));
            controls[2].DataBindings.Add("Text", bs, nameof(this.altID));
            controls[3].DataBindings.Add("Text", bs, nameof(this.addr));
        }

        public JObject Inject(JObject config)
        {
            var key = serverMode ? "inbound" : "outbound";
            var vmess = Lib.Utils.CreateJObject(key);
            vmess[key] = GetSettings();

            try
            {
                Lib.Utils.RemoveKeyFromJObject(config, key + ".settings");
            }
            catch (KeyNotFoundException) { }

            return Lib.Utils.CombineConfig(config, vmess);
        }

        public void Update(JObject config)
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
