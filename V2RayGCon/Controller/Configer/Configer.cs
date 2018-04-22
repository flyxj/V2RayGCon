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
        public int perSection;
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
            perSection = 0;

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
            var index = section < 0 ? perSection : section;

            index = Lib.Utils.Clamp(index, 0, sections.Count - 1);

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
            return Lib.Utils.GetStrFromJToken(config, "v2raygcon.alias");
        }

        public bool SectionChanged(int curSection)
        {
            // int curSection = editor.curSection;

            if (curSection == perSection)
            {
                return false;
            }

            if (IsValid())
            {
                SaveChanges();
                perSection = curSection;
                ShowSection(curSection);
            }
            else
            {
                if (Lib.UI.Confirm(I18N("CannotParseJson")))
                {
                    perSection = curSection;
                    ShowSection(curSection);
                }
                else
                {
                    //editor.curSection = perSection;
                    return true;
                }
            }
            return false;
        }

        public bool SaveChanges()
        {
            var content = editor.content;
            if (perSection >= separator)
            {
                config[sections[perSection]] =
                    JArray.Parse(content);
            }
            else if (perSection == 0)
            {
                config = JObject.Parse(content);

            }
            else
            {
                config[sections[perSection]] =
                    JObject.Parse(content);
            }
            UpdateData();
            return true;
        }

        public bool IsValid()
        {
            try
            {
                if (perSection >= separator)
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

        void UpdateData()
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
                perSection == 0 ?
                config.ToString() :
                config[sections[perSection]].ToString();
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
            }

            setting.ReplaceServer(
                Lib.Utils.Base64Encode(config.ToString()),
                serverIndex);
            //MessageBox.Show(I18N("Done"));
        }

        public void LoadExample()
        {
            var defConfig = JObject.Parse(resData("config_def"));

            string example =
                perSection == 0 ?
                resData("config_min") :
                defConfig[sections[perSection]]?.ToString();

            if (string.IsNullOrEmpty(example))
            {
                MessageBox.Show(I18N("EditorNoExample"));
            }
            else
            {
                editor.content = example;
            }
        }

        public void InsertKCP()
        {
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
            streamSettings.UpdateData(config);
            ShowSection();
        }

        public void InsertWS()
        {
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
            streamSettings.UpdateData(config);
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
            }
            setting.AddServer(Lib.Utils.Base64Encode(config.ToString()));
            MessageBox.Show(I18N("Done"));
        }

        public void InsertTCP()
        {
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
            streamSettings.UpdateData(config);
            ShowSection();
        }

        public void InsertSSClient()
        {
            InsertOutBoundSetting(ssClient.GetSettings(), "shadowsocks");
            ssClient.UpdateData(config);
            ShowSection();
        }

        public void InsertVmessClient()
        {
            InsertOutBoundSetting(vmessClient.GetSettings(), "vmess");
            vmessClient.UpdateData(config);
            ShowSection();
        }

        public void InsertVmessServer()
        {
            try
            {
                config["inbound"] = vmessServer.GetSettings();
            }
            catch { }
            vmessServer.UpdateData(config);
            ShowSection();
        }

        public void InsertSSServer()
        {
            try
            {
                config["inbound"] = ssServer.GetSettings();
            }
            catch { }
            ssServer.UpdateData(config);
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
