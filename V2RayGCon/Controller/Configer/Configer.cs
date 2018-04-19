using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.Configer
{
    class Configer
    {
        Service.Setting setting;
        public SSRClient ssrClient;
        public StreamClient streamClient;
        public VmessClient vmessClient;
        public Editor editor;

        JObject config;
        int perSection, separator;
        Dictionary<int, string> sections;

        public Configer()
        {
            setting = Service.Setting.Instance;
            ssrClient = new SSRClient();
            streamClient = new StreamClient();
            vmessClient = new VmessClient();
            editor = new Editor(SectionChanged);

            separator = Model.Table.sectionSeparator;
            sections = Model.Table.configSections;
            perSection = 0;

            LoadConfig();
            editor.content = config.ToString();
            UpdateData();

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
            Func<string, string, string> GetStr = (_perfix, _key) =>
            {
                return Lib.Utils.GetStrFromJToken(config, _perfix + _key);
            };

            Func<string, string, string, string> GetAddrStr = (_perfix, _keyIP, _keyPort) =>
            {
                var ip = Lib.Utils.GetStrFromJToken(config, _perfix + _keyIP);
                var port = Lib.Utils.GetStrFromJToken(config, _perfix + _keyPort);
                return string.Join(":", ip, port);
            };

            string perfix;

            // vmess
            perfix = "outbound.settings.vnext.0.users.0.";
            vmessClient.ID = GetStr(perfix, "id");
            vmessClient.level = GetStr(perfix, "level");
            vmessClient.altID = GetStr(perfix, "alterId");
            perfix = "outbound.settings.vnext.0.";
            vmessClient.addr = GetAddrStr(perfix, "address", "port");


            // SS outbound.settings.servers.0.
            perfix = "outbound.settings.servers.0.";
            ssrClient.email = GetStr(perfix, "email");
            ssrClient.pass = GetStr(perfix, "password");
            ssrClient.addr = GetAddrStr(perfix, "address", "port");
            ssrClient.OTA = Lib.Utils.GetBoolFromJToken(config, perfix + "ota");
            ssrClient.SetMethod(GetStr(perfix, "method"));

            // kcp ws tls
            perfix = "outbound.streamSettings.";
            streamClient.kcpType = GetStr(perfix, "kcpSettings.header.type");
            streamClient.wsPath = GetStr(perfix, "wsSettings.path");
            streamClient.SetSecurity(GetStr(perfix, "security"));
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
                defConfig.ToString() :
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
                config["outbound"]["streamSettings"] = streamClient.GetKCPSetting();
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public void InsertWS()
        {
            try
            {
                config["outbound"]["streamSettings"] = streamClient.GetWSSetting();
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
                config["outbound"]["streamSettings"] = streamClient.GetTCPSetting();
            }
            catch { }
            UpdateData();
            ShowSection();
        }

        public void InsertSSRClient()
        {
            InsertOutBoundSetting(ssrClient.GetSettings(), "shadowsocks");
            UpdateData();
            ShowSection();
        }

        public void InsertVmessClient()
        {
            InsertOutBoundSetting(vmessClient.GetSettings(), "vmess");
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
