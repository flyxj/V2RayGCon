using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Controller.ConfigerComponet
{
    class StreamSettings : ConfigerComponentController
    {
        Service.Cache cache;
        public StreamSettings(
            ComboBox type,
            ComboBox param,
            RadioButton inbound,
            Button insert,
            CheckBox tls,
            CheckBox sockopt)
        {
            cache = Service.Cache.Instance;
            isServer = false;
            DataBinding(type, param, tls, sockopt);
            Connect(type, param);
            AttachEvent(inbound, insert);
            InitComboBox(type);
        }

        #region properties
        private int _streamType;
        public int streamType
        {
            get { return _streamType; }
            set { SetField(ref _streamType, value); }
        }

        private string _streamParamText;
        public string streamParamText
        {
            get { return _streamParamText; }
            set { SetField(ref _streamParamText, value); }
        }

        bool isServer { get; set; }

        bool _isUseTls;
        public bool isUseTls
        {
            get { return _isUseTls; }
            set { SetField(ref _isUseTls, value); }
        }

        bool _isUseSockopt;
        public bool isUseSockopt
        {
            get { return _isUseSockopt; }
            set { SetField(ref _isUseSockopt, value); }
        }

        #endregion

        #region public method
        public override void Update(JObject config)
        {
            var GetStr = Lib.Utils.GetStringByPrefixAndKeyHelper(config);

            string prefix = isServer ?
                "inbound.streamSettings" :
                "outbound.streamSettings";

            var index = GetIndexByNetwork(GetStr(prefix, "network"));
            streamType = index;
            streamParamText = index < 0 ? string.Empty :
                GetStr(
                    prefix,
                    Model.Data.Table.streamSettings[index].optionPath);

            isUseSockopt = Lib.Utils.GetKey(config, prefix + ".sockopt") != null;
            isUseTls = GetStr(prefix, "security") == "tls";
        }
        #endregion

        #region private method
        void InitComboBox(ComboBox cboxType)
        {
            var streamType = new Dictionary<int, string>();
            foreach (var type in Model.Data.Table.streamSettings)
            {
                streamType.Add(type.Key, type.Value.name);
            }
            Lib.UI.FillComboBox(cboxType, streamType);
        }

        void Connect(ComboBox type, ComboBox param)
        {
            type.SelectedIndexChanged += (sender, arg) =>
            {
                var index = type.SelectedIndex;

                if (index < 0)
                {
                    param.SelectedIndex = -1;
                    param.Items.Clear();
                    return;
                }

                var s = Model.Data.Table.streamSettings[index];

                param.Items.Clear();

                if (!s.dropDownStyle)
                {
                    param.DropDownStyle = ComboBoxStyle.DropDown;
                    return;
                }

                param.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var option in s.options)
                {
                    param.Items.Add(option.Key);
                }
            };
        }

        void AttachEvent(
            RadioButton inbound,
            Button insert)
        {
            inbound.CheckedChanged += (s, a) =>
            {
                this.isServer = inbound.Checked;
                this.Update(container.config);
            };

            insert.Click += (s, a) =>
            {
                container.InjectConfigHelper(() =>
                {
                    Inject(ref container.config);
                });
            };
        }

        void DataBinding(
            ComboBox type,
            ComboBox param,
            CheckBox tls,
            CheckBox sockopt)
        {
            var bs = new BindingSource();
            bs.DataSource = this;

            tls.DataBindings.Add(
                nameof(CheckBox.Checked),
                bs,
                nameof(this.isUseTls),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            sockopt.DataBindings.Add(
               nameof(CheckBox.Checked),
               bs,
               nameof(this.isUseSockopt),
               true,
               DataSourceUpdateMode.OnPropertyChanged);

            type.DataBindings.Add(
                nameof(ComboBox.SelectedIndex),
                bs,
                nameof(this.streamType),
                true,
                DataSourceUpdateMode.OnValidation);

            param.DataBindings.Add(
                nameof(ComboBox.Text),
                bs,
                nameof(this.streamParamText),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        void Inject(ref JObject config)
        {
            var settings = GetSettings();
            var key = isServer ? "inbound" : "outbound";
            JObject stream = Lib.Utils.CreateJObject(key);
            stream[key]["streamSettings"] = settings;

            try
            {
                Lib.Utils.RemoveKeyFromJObject(config, key + ".streamSettings");
            }
            catch (KeyNotFoundException) { }

            Lib.Utils.CombineConfig(ref config, stream);
        }

        JToken GetSettings()
        {
            streamType = Lib.Utils.Clamp(
                streamType,
                0,
                Model.Data.Table.streamSettings.Count);

            var key = "none";
            var s = Model.Data.Table.streamSettings[streamType];
            if (s.dropDownStyle && s.options.ContainsKey(streamParamText))
            {
                key = streamParamText;
            }

            var tpl = cache.tpl.LoadTemplate(s.options[key]) as JObject;
            if (!s.dropDownStyle)
            {
                Lib.Utils.SetValue<string>(tpl, s.optionPath, streamParamText);
            }

            InsertTLSSettings(tpl);
            InsertSocksMark(tpl);
            return tpl;
        }

        int GetIndexByNetwork(string network)
        {
            if (string.IsNullOrEmpty(network))
            {
                return -1;
            }

            foreach (var item in Model.Data.Table.streamSettings)
            {
                if (item.Value.network == network)
                {
                    return item.Key;
                }
            }

            return -1;
        }

        void InsertSocksMark(JToken streamSettings)
        {
            if (isUseSockopt)
            {
                streamSettings["sockopt"] =
                    JToken.Parse(@"{mark:0,tcpFastOpen:true,tproxy:'off'}");
                return;
            }
        }

        void InsertTLSSettings(JToken streamSettings)
        {
            var tlsTpl = cache.tpl.LoadTemplate("tls");
            if (isUseTls)
            {
                streamSettings["security"] = "tls";
                streamSettings["tlsSettings"] = tlsTpl;
            }
            else
            {
                streamSettings["security"] = "none";
            }
        }
        #endregion

    }

}
