using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Controller.ConfigerComponet
{
    class Shadowsocks : ConfigerComponentController
    {
        Service.Cache cache;
        public Shadowsocks(
            RadioButton rbtnInboundMode,
            TextBox tboxAddress,
            TextBox tboxPassword,
            CheckBox chkIsShowPassword,
            ComboBox cboxMethods,
            CheckBox chkIsUseOTA,
            Button btnInsert)
        {
            cache = Service.Cache.Instance;
            isServerMode = false;

            Lib.UI.FillComboBox(cboxMethods, Model.Data.Table.ssMethods);
            Lib.UI.ResetComboBoxDropdownMenuWidth(cboxMethods);

            AttachEvents(tboxPassword, chkIsShowPassword, btnInsert, rbtnInboundMode);
            DataBinding(tboxAddress, tboxPassword, cboxMethods, chkIsUseOTA);
        }


        #region properties
        private bool isServerMode { get; set; }

        private int _methodTypeIndex;
        public int methodTypeIndex
        {
            get { return _methodTypeIndex; }
            set { SetField(ref _methodTypeIndex, value); }
        }

        private string _address;
        public string address
        {
            get { return _address; }
            set { SetField(ref _address, value); }
        }

        private string _password;
        public string password
        {
            get { return _password; }
            set { SetField(ref _password, value); }
        }

        bool _isUseOTA;
        public bool isUseOTA
        {
            get { return _isUseOTA; }
            set { SetField(ref _isUseOTA, value); }
        }
        #endregion

        #region public method
        public override void Update(JObject config)
        {
            var GetStr = Lib.Utils.GetStringByPrefixAndKeyHelper(config);
            var prefix = this.isServerMode ? "inbound.settings" : "outbound.settings.servers.0";
            this.isUseOTA = Lib.Utils.GetValue<bool>(config, prefix, "ota");
            this.password = GetStr(prefix, "password");
            this.address = isServerMode ?
                Lib.Utils.GetAddr(config, "inbound", "listen", "port") :
                Lib.Utils.GetAddr(config, prefix, "address", "port");
            SetMethodTypeIndex(GetStr(prefix, "method"));
        }
        #endregion

        #region private method
        void SetMethodTypeIndex(string selectedMethod)
        {
            this.methodTypeIndex = Lib.Utils.GetIndexIgnoreCase(
                Model.Data.Table.ssMethods,
                selectedMethod);
        }

        private void DataBinding(TextBox tboxAddress, TextBox tboxPassword, ComboBox cboxMethods, CheckBox chkIsUseOTA)
        {
            var bs = new BindingSource();
            bs.DataSource = this;

            tboxAddress.DataBindings.Add("Text", bs, nameof(this.address));
            tboxPassword.DataBindings.Add("Text", bs, nameof(this.password));

            chkIsUseOTA.DataBindings.Add(
                nameof(CheckBox.Checked),
                bs,
                nameof(this.isUseOTA),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            cboxMethods.DataBindings.Add(
                nameof(ComboBox.SelectedIndex),
                bs,
                nameof(this.methodTypeIndex),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        private void AttachEvents(TextBox tboxPassword, CheckBox chkIsShowPassword, Button btnInsert,
            RadioButton rbtnIsServerMode)
        {
            rbtnIsServerMode.CheckedChanged += (s, a) =>
            {
                this.isServerMode = rbtnIsServerMode.Checked;
                this.Update(container.config);
            };

            chkIsShowPassword.CheckedChanged += (s, a) =>
            {
                tboxPassword.PasswordChar = chkIsShowPassword.Checked ? '\0' : '*';
            };

            btnInsert.Click += (s, a) =>
            {
                container.InjectConfigHelper(() =>
                {
                    Inject(ref container.config);
                });
            };
        }

        void Inject(ref JObject config)
        {

            var keyName = isServerMode ? "inbound.settings" : "outbound.settings";
            var tpl = Lib.Utils.CreateJObject(keyName);

            if (isServerMode)
            {
                tpl["inbound"] = GetSettings();
            }
            else
            {
                tpl["outbound"]["settings"] = GetSettings();
                tpl["outbound"]["protocol"] = "shadowsocks";
            }

            try
            {
                Lib.Utils.RemoveKeyFromJObject(config, keyName);
            }
            catch (KeyNotFoundException) { }

            Lib.Utils.CombineConfig(ref config, tpl);
        }

        JToken GetSettings()
        {

            var ssMethods = Model.Data.Table.ssMethods;
            var tpl = cache.tpl.LoadTemplate(isServerMode ? "ssServer" : "ssClient");
            Lib.Utils.TryParseIPAddr(this.address, out string ip, out int port);
            var index = this.methodTypeIndex;
            var methodName = index < 0 ? ssMethods[0] : ssMethods[index];

            if (isServerMode)
            {
                tpl["port"] = port;
                tpl["listen"] = ip;
                tpl["settings"]["method"] = methodName;
                tpl["settings"]["password"] = this.password;
                tpl["settings"]["ota"] = this.isUseOTA;
                tpl["settings"]["network"] = "tcp,udp";
            }
            else
            {
                tpl["servers"][0]["address"] = ip;
                tpl["servers"][0]["port"] = port;
                tpl["servers"][0]["method"] = methodName;
                tpl["servers"][0]["password"] = this.password;
                tpl["servers"][0]["ota"] = this.isUseOTA;
            }

            return tpl;
        }

        #endregion

    }

}
