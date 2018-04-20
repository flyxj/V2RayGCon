using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.Configer
{
    class Configer
    {
        Service.Setting setting;
        public SSClient ssClient;
        public StreamSettings streamClient;
        public VmessClient vmessClient;
        public VmessServer vmessServer;
        public Editor editor;
        public SSServer ssServer;

        JObject config;
        int perSection, separator;
        Dictionary<int, string> sections;

        public Configer()
        {
            setting = Service.Setting.Instance;
            ssServer = new SSServer();
            ssClient = new SSClient();
            streamClient = new StreamSettings(StreamIsInboundChanged);
            vmessClient = new VmessClient();
            vmessServer = new VmessServer();
            editor = new Editor(SectionChanged);

            separator = Model.Table.sectionSeparator;
            sections = Model.Table.configSections;
            perSection = 0;

            LoadConfig();
            editor.content = config.ToString();
            UpdateData();

        }

        public void StreamIsInboundChanged()
        {
            streamClient.UpdateData(config);
        }

        public void ShowSection(int section = -1)
        {
            var index = section < 0 ? perSection : section;

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

        public void SectionChanged()
        {
            int curSection = editor.curSection;

            if (curSection != perSection)
            {
                if (IsValid())
                {
                    if (IsSectionChange() && Lib.UI.Confirm(I18N("EditorSaveChange")))
                    {
                        SaveChanges();
                    }

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
                        editor.curSection = perSection;
                        return;
                    }
                }
            }
        }

        public bool IsSectionChange()
        {
            var content = editor.content;

            if (perSection >= separator)
            {
                return !(JToken.DeepEquals(JArray.Parse(content),
                    config[sections[perSection]]));
            }

            var section = JObject.Parse(content);

            if (perSection == 0)
            {
                return !(JToken.DeepEquals(section, config));
            }

            return !(JToken.DeepEquals(section,
                config[sections[perSection]]));
        }

        public bool SaveChanges()
        {
            if (!IsValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return false;
            }

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
            }
            catch
            {
                return false;
            }

            return true;
        }

        void UpdateData()
        {
            vmessClient.UpdateData(config);
            ssClient.UpdateData(config);
            ssServer.UpdateData(config);
            streamClient.UpdateData(config);
            vmessServer.UpdateData(config);
        }

        public void DiscardChanges()
        {
            editor.content =
                perSection == 0 ?
                config.ToString() :
                config[sections[perSection]].ToString();
        }

        public void OverwriteServerConfig(int serverIndex)
        {
            if (IsSectionChange() && !Lib.UI.Confirm(I18N("EditorDiscardChange")))
            {
                return;
            }

            setting.ReplaceServer(
                Lib.Utils.Base64Encode(config.ToString()),
                serverIndex);

            MessageBox.Show(I18N("Done"));
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
                if (streamClient.isInbound)
                {
                    config["inbound"]["streamSettings"] = streamClient.GetKCPSetting();
                }
                else
                {
                    config["outbound"]["streamSettings"] = streamClient.GetKCPSetting();
                }
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public void InsertWS()
        {
            try
            {
                if (streamClient.isInbound)
                {
                    config["inbound"]["streamSettings"] = streamClient.GetWSSetting();
                }
                else
                {
                    config["outbound"]["streamSettings"] = streamClient.GetWSSetting();
                }
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public void InsertNewServer()
        {
            if (IsSectionChange() && !Lib.UI.Confirm(I18N("EditorDiscardChange")))
            {
                return;
            }
            setting.AddServer(Lib.Utils.Base64Encode(config.ToString()));
            MessageBox.Show(I18N("Done"));
        }

        public void InsertTCP()
        {
            try
            {
                if (streamClient.isInbound)
                {
                    config["inbound"]["streamSettings"] = streamClient.GetTCPSetting();
                }
                else
                {
                    config["outbound"]["streamSettings"] = streamClient.GetTCPSetting();
                }
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public void InsertSSClient()
        {
            InsertOutBoundSetting(ssClient.GetSettings(), "shadowsocks");
            UpdateData();
            ShowSection();
        }

        public void InsertVmessClient()
        {
            InsertOutBoundSetting(vmessClient.GetSettings(), "vmess");
            UpdateData();
            ShowSection();
        }

        public void InsertVmessServer()
        {
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
            try
            {
                config["inbound"] = ssServer.GetSettings();
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        void InsertOutBoundSetting(JToken settings, string protocol)
        {
            try
            {
                config["outbound"]["settings"] = settings;
            }
            catch
            {
                Debug.WriteLine("FormConfiger: can not insert outbound.setting");
            }
            try
            {
                config["outbound"]["protocol"] = protocol;
            }
            catch
            {
                Debug.WriteLine("FormConfiger: can not insert outbound.protocol");
            }
            try
            {
                config["outbound"]["tag"] = "agentout";
            }
            catch
            {
                Debug.WriteLine("FormConfiger: can not insert outbound.tag");
            }

        }

        void LoadConfig()
        {
            JObject o = null;
            string b64Config = setting.GetServer(setting.curEditingIndex);

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
