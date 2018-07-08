using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.Configer
{
    class Configer
    {
        Service.Setting setting;
        public SSClient ssClient;
        public StreamSettings streamSettings;
        public VmessCtrl vmessCtrl;
        public Editor editor;
        public SSServer ssServer;
        public VGC vgc;
        public Import import;

        JObject config;
        int separator;
        public int preSection;
        Dictionary<int, string> sections;
        string originalConfig;

        public Configer(Scintilla element, int serverIndex = -1)
        {
            setting = Service.Setting.Instance;
            ssServer = new SSServer();
            ssClient = new SSClient();
            streamSettings = new StreamSettings();
            vmessCtrl = new VmessCtrl();
            editor = new Editor();
            vgc = new VGC();
            import = new Import(element);

            separator = Model.Data.Table.sectionSeparator;
            sections = Model.Data.Table.configSections;
            preSection = 0;
            originalConfig = string.Empty;

            LoadConfig(serverIndex);
            editor.content = config.ToString();
            UpdateData();
        }

        #region public method

        public void InsertDtrMTProto()
        {
            InsertConfigHelper(()=> {
                var eg = Lib.Utils.LoadExamples();
                var mtproto = eg["dtrMTProto"] as JObject;
                mtproto["inboundDetour"][0]["settings"]["users"][0]["secret"] =
                    Lib.Utils.RandomHex(32);
                config = Lib.Utils.MergeJson(config, mtproto);
            });
        }

        public void SetVmessServerMode(bool isServer)
        {
            vmessCtrl.serverMode = isServer;
            vmessCtrl.UpdateData(config);
        }

        public void SetStreamSettingsServerMode(bool isServer)
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

        public List<string> GetExamplesDescription()
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

        public void FormatCurrentContent()
        {
            try
            {
                var json = JToken.Parse(editor.content);
                editor.content = json.ToString();
            }
            catch
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
            }
        }

        public void SaveChanges()
        {
            var content = JToken.Parse(editor.content);

            if (preSection == 0)
            {
                config = content as JObject;
                return;
            }

            if (preSection >= separator)
            {
                config[sections[preSection]] = content as JArray;
            }
            else
            {
                config[sections[preSection]] = content as JObject;
            }
        }

        public bool CheckValid()
        {
            try
            {
                JToken.Parse(editor.content);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateData()
        {
            vmessCtrl.UpdateData(config);
            ssClient.UpdateData(config);
            ssServer.UpdateData(config);
            streamSettings.UpdateData(config);
            vgc.UpdateData(config);
            import.UpdateData(config);
        }

        public void DiscardChanges()
        {
            editor.content =
                preSection == 0 ?
                config.ToString() :
                config[sections[preSection]].ToString();
        }

        public void ReplaceOriginalServer()
        {
            if (string.IsNullOrEmpty(originalConfig))
            {
                // no origin, add a new server.
                AddNewServer();
                return;
            }

            // find out index
            var index = setting.GetServerIndex(originalConfig);
            if (index >= 0)
            {
                ReplaceServer(index);
            }
            else
            {
                MessageBox.Show(I18N("OrgServNotFound"));
            }
        }

        public void ReplaceServer(int serverIndex)
        {
            if (!FlushEditor())
            {
                return;
            }

            if (setting.ReplaceServer(config, serverIndex)) {
                originalConfig = Lib.Utils.Config2Base64String(config);
            }else{
                MessageBox.Show(I18N("DuplicateServer"));
            }
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
            if (!FlushEditor())
            {
                return;
            }

            if (setting.AddServer(config))
            {
                originalConfig = Lib.Utils.Config2Base64String(config);
            }
            else{
                MessageBox.Show(I18N("DuplicateServer"));
            }
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

        public void InsertVmess()
        {
            InsertConfigHelper(() =>
            {
                var vmess = vmessCtrl.GetSettings();
                if (vmessCtrl.serverMode)
                {
                    var keys = new List<string> {
                        "port",
                        "listen",
                        "settings",
                        "protocol" };

                    foreach (var key in keys)
                    {
                        config["inbound"][key] = vmess[key];
                    }
                    // config["inbound"] = vmess;
                }
                else
                {
                    InsertOutBoundSetting(vmess, "vmess");
                }
            });
        }

        public void InsertSkipCN()
        {
            var eg = JObject.Parse(resData("config_def"));
            var c = JObject.Parse(@"{}");
            c["dns"] = eg["dnsCFnGoogle"];
            c["routing"] = eg["routeCNIP"];
            c["outboundDetour"] = eg["outDtrDefault"];

            InsertConfigHelper(() =>
            {
                config=Lib.Utils.MergeJson(config, c);
            });
        }

        public void InsertVGC()
        {
            InsertConfigHelper(() =>
            {
                config["v2raygcon"] = vgc.GetSettings();
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



        bool FlushEditor()
        {
            if (!CheckValid())
            {
                if (Lib.UI.Confirm(I18N("EditorDiscardChange")))
                {
                    DiscardChanges();
                }
                else
                {
                    return false;
                }
            }

            SaveChanges();
            UpdateData();
            return true;
        }

        public void InsertConfigHelper(Action lamda)
        {
            if (!CheckValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }

            SaveChanges();

            try
            {
                lamda?.Invoke();
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
                originalConfig = b64Config;
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
