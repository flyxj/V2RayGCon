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
            ComboBox tls,
            RadioButton inbound,
            Button insert)
        {
            cache = Service.Cache.Instance;
            _isServer = false;

            DataBinding(type, param, tls);
            Connect(type, param);
            AttachEvent(inbound, insert);
            InitComboBox(tls, type);
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

        private int _tls;
        public int tls
        {
            get { return _tls; }
            set { SetField(ref _tls, value); }
        }

        private bool _isServer;
        public bool isServer
        {
            get { return _isServer; }
            set { _isServer = value; }
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

            tls = GetStr(prefix, "security") == "tls" ? 1 : 0;
        }
        #endregion

        #region private method
        void InitComboBox(ComboBox tls, ComboBox cboxType)
        {
            Lib.UI.FillComboBox(tls, Model.Data.Table.streamTLS);

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
                    param.DropDownStyle = ComboBoxStyle.Simple;
                    return;
                }

                param.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var option in s.options)
                {
                    param.Items.Add(option.Key);
                }
            };
        }

        void AttachEvent(RadioButton inbound, Button insert)
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

        void DataBinding(ComboBox type, ComboBox param, ComboBox tls)
        {
            var bs = new BindingSource();
            bs.DataSource = this;

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

            tls.DataBindings.Add(
                nameof(ComboBox.SelectedIndex),
                bs,
                nameof(this.tls),
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

        void InsertTLSSettings(JToken streamSettings)
        {
            var tlsTpl = cache.tpl.LoadTemplate("tls");
            if (tls <= 0)
            {
                streamSettings["security"] = string.Empty;
            }
            else
            {
                var streamSecurity = Model.Data.Table.streamTLS;
                streamSettings["security"] = streamSecurity[tls];
                streamSettings["tlsSettings"] = tlsTpl;
            }
        }
        #endregion

    }

}
