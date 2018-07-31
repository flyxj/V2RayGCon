using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Controller.ConfigerComponet
{
    class SSClient :
        Model.BaseClass.NotifyComponent,
        Model.BaseClass.IConfigerComponent
    {
        #region properties
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
        #endregion

        #region public method
        public JObject Inject(JObject config)
        {
            var outbound = Lib.Utils.CreateJObject("outbound");
            outbound["outbound"]["settings"] = GetSettings();
            outbound["outbound"]["protocol"] = "shadowsocks";

            try
            {
                Lib.Utils.RemoveKeyFromJObject(config, "outbound.settings");
            }
            catch (KeyNotFoundException) { }

            return Lib.Utils.CombineConfig(config, outbound);
        }

        // text box [address, pass] combo box [method] check box[OTA]
        public void Bind(List<Control> controls)
        {
            if (controls.Count != 4)
            {
                throw new ArgumentException();
            }

            var bs = new BindingSource();
            bs.DataSource = this;

            controls[0].DataBindings.Add("Text", bs, nameof(this.addr));
            controls[1].DataBindings.Add("Text", bs, nameof(this.pass));

            controls[2].DataBindings.Add(
                nameof(ComboBox.SelectedIndex),
                bs,
                nameof(this.method),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            controls[3].DataBindings.Add(
                nameof(CheckBox.Checked),
                bs,
                nameof(this.OTA),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

        }

        public void Update(JObject config)
        {
            var prefix = "outbound.settings.servers.0";

            addr = Lib.Utils.GetAddr(config, prefix, "address", "port");
            pass = Lib.Utils.GetValue<string>(config, prefix, "password");

            OTA = Lib.Utils.GetValue<bool>(config, prefix, "ota");
            SetMethod(Lib.Utils.GetValue<string>(config, prefix, "method"));
        }
        #endregion

        #region private method
        void SetMethod(string selectedMethod)
        {
            method = Lib.Utils.GetIndexIgnoreCase(Model.Data.Table.ssMethods, selectedMethod);
        }

        JToken GetSettings()
        {
            JToken client = Service.Cache.Instance.
                tpl.LoadTemplate("ssClient");

            var ssMethods = Model.Data.Table.ssMethods;

            client["servers"][0]["address"] = _ip;
            client["servers"][0]["port"] = _port;
            client["servers"][0]["method"] = method < 0 ? ssMethods[0] : ssMethods[method];
            client["servers"][0]["password"] = pass;
            client["servers"][0]["ota"] = OTA;

            return client;
        }
        #endregion
    }
}
