using Newtonsoft.Json.Linq;
using System;
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
        public VGC vgc;

        JObject config;
        int separator;
        public int preSection;
        Dictionary<int, string> sections;

        public Configer(int serverIndex = -1)
        {
            setting = Service.Setting.Instance;
            ssServer = new SSServer();
            ssClient = new SSClient();
            streamSettings = new StreamSettings();
            vmessClient = new VmessClient();
            vmessServer = new VmessServer();
            editor = new Editor();
            vgc = new VGC();

            separator = Model.Data.Table.sectionSeparator;
            sections = Model.Data.Table.configSections;
            preSection = 0;

            LoadConfig(serverIndex);
            editor.content = config.ToString();
            UpdateData();

        }

        #region public method

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
            return Lib.Utils.GetValue<string>(config, "v2raygcon.alias");
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

        public bool OnSectionChanged(int curSection)
        {
            if (curSection == preSection)
            {
                // prevent inf loop
                return true;
            }

            if (CheckValid())
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

        public bool CheckValid()
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
            vgc.UpdateData(config);
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
            if (!CheckValid())
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
            InsertConfigHelper(() =>
            {
                if (streamSettings.isServer)
                {
                    config["inbound"]["streamSettings"] = streamSettings.GetKCPSetting();
                }
                else
                {
                    config["outbound"]["streamSettings"] = streamSettings.GetKCPSetting();
                }
            });
        }

        public void InsertWS()
        {
            InsertConfigHelper(() =>
            {
                if (streamSettings.isServer)
                {
                    config["inbound"]["streamSettings"] = streamSettings.GetWSSetting();
                }
                else
                {
                    config["outbound"]["streamSettings"] = streamSettings.GetWSSetting();
                }
            });
        }

        public void AddNewServer()
        {
            if (!CheckValid())
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
            InsertConfigHelper(() =>
            {
                if (streamSettings.isServer)
                {
                    config["inbound"]["streamSettings"] = streamSettings.GetTCPSetting();
                }
                else
                {
                    config["outbound"]["streamSettings"] = streamSettings.GetTCPSetting();
                }
            });
        }

        public void InsertSSClient()
        {
            InsertConfigHelper(() =>
            {
                InsertOutBoundSetting(ssClient.GetSettings(), "shadowsocks");
            });
        }

        public void InsertVmessClient()
        {
            InsertConfigHelper(() =>
            {
                InsertOutBoundSetting(vmessClient.GetSettings(), "vmess");
            });
        }

        public void InsertVGC()
        {
            InsertConfigHelper(() =>
            {
                config["v2raygcon"] = vgc.GetSettings();
            });
        }

        public void InsertVmessServer()
        {
            InsertConfigHelper(() =>
            {
                config["inbound"] = vmessServer.GetSettings();
            });
        }

        public void InsertSSServer()
        {
            InsertConfigHelper(() =>
            {
                config["inbound"] = ssServer.GetSettings();
            });
        }

        public string GetConfigFormated()
        {
            return config.ToString(Newtonsoft.Json.Formatting.Indented);
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
        #endregion

        #region private method
        void InsertConfigHelper(Action f)
        {
            if (!CheckValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }

            SaveChanges();

            try
            {
                f();
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

        void LoadConfig(int index = -1)
        {
            var serverIndex = index < 0 ? 0 : index;

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
        #endregion
    }
}
