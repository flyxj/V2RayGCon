using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Controller.ConfigerComponet
{
    class SSServer : Model.BaseClass.ConfigerComponent
    {
        public SSServer(
            TextBox pass,
            TextBox port,
            ComboBox network,
            ComboBox method,
            CheckBox ota,
            CheckBox showPass,
            Button insert)
        {

            DataBinding(pass, port, network, method, ota);
            AttachEvent(showPass, insert, pass);
        }

        #region properties

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

        // inject component settings into config
        JObject Inject(JObject config)
        {
            var inbound = Lib.Utils.CreateJObject("inbound");
            inbound["inbound"] = GetSettings();

            try
            {
                Lib.Utils.RemoveKeyFromJObject(config, "inbound.settings");
            }
            catch (KeyNotFoundException) { }

            return Lib.Utils.CombineConfig(config, inbound);
        }

        // bind UI controls with component
        void DataBinding(
            TextBox pass,
            TextBox port,
            ComboBox network,
            ComboBox method,
            CheckBox ota)
        {
            var bs = new BindingSource();
            bs.DataSource = this;

            pass.DataBindings.Add("Text", bs, nameof(this.pass));
            port.DataBindings.Add("Text", bs, nameof(this.port));

            network.DataBindings.Add(
                nameof(ComboBox.SelectedIndex),
                bs,
                nameof(this.network),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

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

        JToken GetSettings()
        {
            JToken server = Service.Cache.Instance.
                tpl.LoadTemplate("ssServer");

            var methods = Model.Data.Table.ssMethods;
            var networks = Model.Data.Table.ssNetworks;

            server["port"] = Lib.Utils.Str2Int(port);
            server["settings"]["method"] = method < 0 ? methods[0] : methods[method];
            server["settings"]["password"] = pass;
            server["settings"]["ota"] = OTA;
            server["settings"]["network"] = network < 0 ? networks[0] : networks[network];

            return server;
        }

        void SetMethod(string selectedMethod)
        {
            method = Lib.Utils.GetIndexIgnoreCase(
                Model.Data.Table.ssMethods,
                selectedMethod);
        }

        void SetNetwork(string selectedNetwork)
        {
            network = Lib.Utils.GetIndexIgnoreCase(
                Model.Data.Table.ssNetworks,
                selectedNetwork);
        }
        #endregion

        #region public method
        public override void Update(JObject config)
        {
            port = Lib.Utils.GetValue<int>(config, "inbound", "port").ToString();

            var GetStr = Lib.Utils.GetStringByPrefixAndKeyHelper(config);
            var prefix = "inbound.settings";

            SetMethod(GetStr(prefix, "method"));
            SetNetwork(GetStr(prefix, "network"));
            pass = GetStr(prefix, "password");
            OTA = Lib.Utils.GetValue<bool>(config, prefix, "ota");
        }
        #endregion
    }
}
