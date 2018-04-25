using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.Configer
{
    class Configer
    {
        Service.Setting setting;
        public SSClient ssClient;
        public StreamSettings streamSettings;
        public VmessClient vmessClient;
        public VmessServer vmessServer;
        public Editor editor;
        public SSServer ssServer;

        JObject config;
        int separator;
        public int preSection;
        Dictionary<int, string> sections;

        public Configer()
        {
            setting = Service.Setting.Instance;
            ssServer = new SSServer();
            ssClient = new SSClient();
            streamSettings = new StreamSettings();
            vmessClient = new VmessClient();
            vmessServer = new VmessServer();
            editor = new Editor();

            separator = Model.Data.Table.sectionSeparator;
            sections = Model.Data.Table.configSections;
            preSection = 0;

            LoadConfig();
            editor.content = config.ToString();
            UpdateData();

        }

        public void StreamSettingsIsServerChange(bool isServer)
        {
            streamSettings.isServer = isServer;
            streamSettings.UpdateData(config);
        }

        public void ShowSection(int section = -1)
        {
            var index = section < 0 ? preSection : section;

            index = Lib.Utils.Clamp(index, 0, sections.Count);

            if (index == 0)
            {
                editor.content = config.ToString();
                return;
            }

            var part = config[sections[index]];
            if (part == null)
            {
                if (index >= separator)
                {
                    part = new JArray();
                }
                else
                {
                    part = new JObject();
                }
                config[sections[index]] = part;
            }
            editor.content = part.ToString();
        }

        public string GetAlias()
        {
            return Lib.Utils.GetString(config, "v2raygcon.alias");
        }

        public List<string> GetExampleDescriptions()
        {
            var list = new List<string>();

            var examples = Model.Data.Table.examples;

            if (!examples.ContainsKey(preSection))
            {
                return list;
            }

            foreach (var example in examples[preSection])
            {
                // 0.description 1.keyString
                list.Add(example[0]);
            }

            return list;
        }

        public bool SectionChanged(int curSection)
        {
            if (curSection == preSection)
            {
                // prevent inf loop
                return true;
            }

            if (IsValid())
            {
                SaveChanges();
                preSection = curSection;
                ShowSection();
                UpdateData();
            }
            else
            {
                if (Lib.UI.Confirm(I18N("CannotParseJson")))
                {
                    preSection = curSection;
                    ShowSection();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool SaveChanges()
        {
            var content = editor.content;
            if (preSection >= separator)
            {
                config[sections[preSection]] =
                    JArray.Parse(content);
            }
            else if (preSection == 0)
            {
                config = JObject.Parse(content);

            }
            else
            {
                config[sections[preSection]] =
                    JObject.Parse(content);
            }
            return true;
        }

        public bool IsValid()
        {
            try
            {
                if (preSection >= separator)
                {
                    JArray.Parse(editor.content);
                }
                else
                {
                    JObject.Parse(editor.content);
                }
                return true;
            }
            catch { }
            return false;
        }

        public void UpdateData()
        {
            vmessClient.UpdateData(config);
            ssClient.UpdateData(config);
            ssServer.UpdateData(config);
            streamSettings.UpdateData(config);
            vmessServer.UpdateData(config);
        }

        public void DiscardChanges()
        {
            editor.content =
                preSection == 0 ?
                config.ToString() :
                config[sections[preSection]].ToString();
        }

        public void ReplaceServer(int serverIndex)
        {
            if (!IsValid())
            {
                if (!Lib.UI.Confirm(I18N("EditorDiscardChange")))
                {
                    return;
                }
            }
            else
            {
                SaveChanges();
                UpdateData();
            }

            setting.ReplaceServer(
                Lib.Utils.Base64Encode(config.ToString()),
                serverIndex);
            //MessageBox.Show(I18N("Done"));
        }

        public void LoadExample(int index)
        {
            if (index < 0)
            {
                return;
            }

            var defConfig = JObject.Parse(resData("config_def"));
            var examples = Model.Data.Table.examples;
            try
            {
                string key = examples[preSection][index][1];
                string content;

                if (preSection == Model.Data.Table.inboundIndex)
                {
                    var inTpl = defConfig["inTpl"];
                    inTpl["protocol"] = examples[preSection][index][2];
                    inTpl["settings"] = defConfig[key];
                    content = inTpl.ToString();
                }
                else if (preSection == Model.Data.Table.outboundIndex)
                {
                    var outTpl = defConfig["outTpl"];
                    outTpl["protocol"] = examples[preSection][index][2];
                    outTpl["settings"] = defConfig[key];
                    content = outTpl.ToString();
                }
                else
                {
                    content = defConfig[key].ToString();
                }

                editor.content = content;
            }
            catch
            {
                MessageBox.Show(I18N("EditorNoExample"));
            }
        }

        public void InsertKCP()
        {
            if (!IsValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }

            SaveChanges();
            try
            {
                if (streamSettings.isServer)
                {
                    config["inbound"]["streamSettings"] = streamSettings.GetKCPSetting();
                }
                else
                {
                    config["outbound"]["streamSettings"] = streamSettings.GetKCPSetting();
                }
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public void InsertWS()
        {
            if (!IsValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }
            SaveChanges();
            try
            {
                if (streamSettings.isServer)
                {
                    config["inbound"]["streamSettings"] = streamSettings.GetWSSetting();
                }
                else
                {
                    config["outbound"]["streamSettings"] = streamSettings.GetWSSetting();
                }
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public void AddNewServer()
        {
            if (!IsValid())
            {
                if (!Lib.UI.Confirm(I18N("EditorDiscardChange")))
                {
                    return;
                }
            }
            else
            {
                SaveChanges();
                UpdateData();
            }
            setting.AddServer(Lib.Utils.Base64Encode(config.ToString()));
            MessageBox.Show(I18N("Done"));
        }

        public void InsertTCP()
        {
            if (!IsValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }
            SaveChanges();

            try
            {
                if (streamSettings.isServer)
                {
                    config["inbound"]["streamSettings"] = streamSettings.GetTCPSetting();
                }
                else
                {
                    config["outbound"]["streamSettings"] = streamSettings.GetTCPSetting();
                }
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public void InsertSSClient()
        {
            if (!IsValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }

            SaveChanges();
            InsertOutBoundSetting(ssClient.GetSettings(), "shadowsocks");
            UpdateData();
            ShowSection();
        }

        public void InsertVmessClient()
        {
            if (!IsValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }

            SaveChanges();
            InsertOutBoundSetting(vmessClient.GetSettings(), "vmess");
            UpdateData();
            ShowSection();
        }

        public void InsertVmessServer()
        {
            if (!IsValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }

            SaveChanges();
            try
            {
                config["inbound"] = vmessServer.GetSettings();
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public void InsertSSServer()
        {
            if (!IsValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }

            SaveChanges();
            try
            {
                config["inbound"] = ssServer.GetSettings();
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public string GetConfigFormated()
        {
            return config.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        void InsertOutBoundSetting(JToken settings, string protocol)
        {
            try
            {
                config["outbound"]["settings"] = settings;
            }
            catch { }
            try
            {
                config["outbound"]["protocol"] = protocol;
            }
            catch { }
            try
            {
                config["outbound"]["tag"] = "agentout";
            }
            catch { }

        }

        public bool SetConfig(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            try
            {
                var o = JObject.Parse(json);
                if (o == null)
                {
                    return false;
                }
                config = o;
                UpdateData();
                ShowSection();
                return true;
            }
            catch { }
            return false;
        }

        public void LoadServer(int index)
        {
            LoadConfig(index);
            UpdateData();
            ShowSection();
        }

        void LoadConfig(int index = -1)
        {
            var serverIndex = index < 0 ? setting.curEditingIndex : index;

            JObject o = null;
            string b64Config = setting.GetServer(serverIndex);

            if (!string.IsNullOrEmpty(b64Config))
            {
                o = JObject.Parse(Lib.Utils.Base64Decode(b64Config));
            }

            if (o == null)
            {
                o = JObject.Parse(resData("config_min"));
                MessageBox.Show(I18N("EditorCannotLoadServerConfig"));
            }

            config = o;
        }
    }
}
