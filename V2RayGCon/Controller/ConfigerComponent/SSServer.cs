using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Controller.ConfigerComponet
{
    class SSServer :
        Model.BaseClass.NotifyComponent,
        Model.BaseClass.IConfigerComponent
    {
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
        // inject component settings into config
        public JObject Inject(JObject config)
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
        public void Bind(List<Control> controls)
        {

            if (controls.Count != 5)
            {
                throw new ArgumentException();
            }

            var bs = new BindingSource();
            bs.DataSource = this;

            controls[0].DataBindings.Add("Text", bs, nameof(this.pass));
            controls[1].DataBindings.Add("Text", bs, nameof(this.port));

            controls[2].DataBindings.Add(
                nameof(ComboBox.SelectedIndex),
                bs,
                nameof(this.network),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            controls[3].DataBindings.Add(
                nameof(ComboBox.SelectedIndex),
                bs,
                nameof(this.method),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            controls[4].DataBindings.Add(
                nameof(CheckBox.Checked),
                bs,
                nameof(this.OTA),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        public void Update(JObject config)
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
