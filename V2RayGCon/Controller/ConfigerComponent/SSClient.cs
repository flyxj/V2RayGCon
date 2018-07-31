using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Controller.ConfigerComponet
{
    class SSClient : Model.BaseClass.ConfigerComponent
    {
        public SSClient(
            TextBox address,
            TextBox pass,
            ComboBox method,
            CheckBox ota,
            CheckBox showPass,
            Button insert)
        {
            DataBind(address, pass, method, ota);
            AttachEvent(showPass, insert, pass);

        }

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
        public override void Update(JObject config)
        {
            var prefix = "outbound.settings.servers.0";

            addr = Lib.Utils.GetAddr(config, prefix, "address", "port");
            pass = Lib.Utils.GetValue<string>(config, prefix, "password");

            OTA = Lib.Utils.GetValue<bool>(config, prefix, "ota");
            SetMethod(Lib.Utils.GetValue<string>(config, prefix, "method"));
        }
        #endregion

        #region private method
        void AttachEvent(CheckBox showPass, Button insert, TextBox pass)
        {
            showPass.CheckedChanged += (s, a) =>
            {
                pass.PasswordChar = showPass.Checked ? '\0' : '*';
            };

            insert.Click += (s, a) =>
            {
                container.InjectConfigHelper(() =>
                {
                    container.config = Inject(container.config);
                });
            };
        }

        // text box [address, pass] combo box [method] check box[OTA]
        void DataBind(
            TextBox address,
            TextBox pass,
            ComboBox method,
            CheckBox ota)
        {

            var bs = new BindingSource();
            bs.DataSource = this;

            address.DataBindings.Add("Text", bs, nameof(this.addr));
            pass.DataBindings.Add("Text", bs, nameof(this.pass));

            method.DataBindings.Add(
                nameof(ComboBox.SelectedIndex),
                bs,
                nameof(this.method),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            ota.DataBindings.Add(
                nameof(CheckBox.Checked),
                bs,
                nameof(this.OTA),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        JObject Inject(JObject config)
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
