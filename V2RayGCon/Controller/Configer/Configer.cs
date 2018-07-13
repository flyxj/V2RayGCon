using Newtonsoft.Json.Linq;
using ScintillaNET;
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
        Service.Cache cache;

        public Configer(Scintilla element, int serverIndex = -1)
        {
            cache = Service.Cache.Instance;
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
            ClearOriginalConfig();

            LoadConfig(serverIndex);
            editor.content = config.ToString();
            UpdateData();
        }

        #region public method
        public void ClearHTMLCache()
        {
            InsertConfigHelper(() =>
            {
                Service.Cache.Instance.RemoveFromCache<string>(
                    StrConst("CacheHTML"),
                    Lib.ImportParser.GetImportUrls(config));
            });
        }

        public void ClearOriginalConfig()
        {
            originalConfig = string.Empty;
        }

        public void InsertDtrMTProto()
        {
            InsertConfigHelper(() =>
            {
                var mtproto = cache.LoadTemplate("dtrMTProto") as JObject;
                var user0 = Lib.Utils.GetKey(mtproto, "inboundDetour.0.settings.users.0");
                user0["secret"] = Lib.Utils.RandomHex(32);
                config = Lib.Utils.CombineConfig(config, mtproto);
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

        public bool ReplaceOriginalServer()
        {
            var index = setting.GetServerIndex(originalConfig);
            if (string.IsNullOrEmpty(originalConfig) || index < 0)
            {
                MessageBox.Show(I18N("OrgServNotFound"));
                return false;
            }
            else
            {
                return ReplaceServer(index);
            }
        }

        public bool ReplaceServer(int serverIndex)
        {
            if (!FlushEditor())
            {
                return false;
            }

            if (setting.ReplaceServer(config, serverIndex))
            {
                originalConfig = Lib.Utils.Config2Base64String(config);
                return true;
            }
            else
            {
                MessageBox.Show(I18N("DuplicateServer"));
                return false;
            }
        }

        public void LoadExample(int index)
        {
            if (index < 0)
            {
                return;
            }

            var examples = Model.Data.Table.examples;
            try
            {
                string key = examples[preSection][index][1];
                string content;

                if (preSection == Model.Data.Table.inboundIndex)
                {
                    var inTpl = cache.LoadExample("inTpl");
                    inTpl["protocol"] = examples[preSection][index][2];
                    inTpl["settings"] = cache.LoadExample(key);
                    content = inTpl.ToString();
                }
                else if (preSection == Model.Data.Table.outboundIndex)
                {
                    var outTpl = cache.LoadExample("outTpl");
                    outTpl["protocol"] = examples[preSection][index][2];
                    outTpl["settings"] = cache.LoadExample(key);
                    content = outTpl.ToString();
                }
                else
                {
                    content = cache.LoadExample(key).ToString();
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
                InsertStreamSetting(
                    streamSettings.GetKCPSetting());
            });
        }

        public void InsertH2()
        {
            InsertConfigHelper(() =>
            {
                InsertStreamSetting(
                    streamSettings.GetH2Setting());
            });
        }

        public void InsertWS()
        {
            InsertConfigHelper(() =>
            {
                InsertStreamSetting(
                    streamSettings.GetWSSetting());
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
            else
            {
                MessageBox.Show(I18N("DuplicateServer"));
            }
        }

        public void InsertTCP()
        {
            InsertConfigHelper(() =>
            {
                InsertStreamSetting(
                    streamSettings.GetTCPSetting());
            });
        }

        public void InsertSSClient()
        {
            InsertConfigHelper(() =>
            {
                var outbound = Lib.Utils.CreateJObject("outbound");
                outbound["outbound"]["settings"] = ssClient.GetSettings();
                outbound["outbound"]["protocol"] = "shadowsocks";

                try
                {
                    Lib.Utils.RemoveKeyFromJObject(config, "outbound.settings");
                }
                catch (KeyNotFoundException) { }

                config = Lib.Utils.CombineConfig(config, outbound);
            });
        }

        public void InsertVmess()
        {
            InsertConfigHelper(() =>
            {
                var key = vmessCtrl.serverMode ? "inbound" : "outbound";
                var vmess = Lib.Utils.CreateJObject(key);
                vmess[key] = vmessCtrl.GetSettings();

                try
                {
                    Lib.Utils.RemoveKeyFromJObject(config, key + ".settings");
                }
                catch (KeyNotFoundException) { }

                config = Lib.Utils.CombineConfig(config, vmess);
            });
        }

        public void InsertSkipCN()
        {
            InsertConfigHelper(() =>
            {
                var c = JObject.Parse(@"{}");
                c["dns"] = cache.LoadExample("dnsCFnGoogle");
                c["routing"] = cache.LoadExample("routeCNIP");
                c["outboundDetour"] = cache.LoadExample("outDtrDefault");
                config = Lib.Utils.CombineConfig(config, c);
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
                var inbound = Lib.Utils.CreateJObject("inbound");
                inbound["inbound"] = ssServer.GetSettings();

                try
                {
                    Lib.Utils.RemoveKeyFromJObject(config, "inbound.settings");
                }
                catch (KeyNotFoundException) { }

                config = Lib.Utils.CombineConfig(config, inbound);
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

        public void InsertConfigHelper(Action lamda)
        {
            if (!CheckValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }

            SaveChanges();

            lamda?.Invoke();

            UpdateData();
            ShowSection();
        }

        #endregion

        #region private method

        void InsertStreamSetting(JToken streamSetting)
        {
            var key = streamSettings.isServer ? "inbound" : "outbound";

            var empty = Lib.Utils.CreateJObject(key);
            var stream = empty.DeepClone() as JObject;

            empty[key]["streamSettings"] = null;
            stream[key]["streamSettings"] = streamSetting;

            var temp = Lib.Utils.CombineConfig(config, empty);
            config = Lib.Utils.CombineConfig(temp, stream);
        }

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
                o = cache.LoadMinConfig();
                MessageBox.Show(I18N("EditorCannotLoadServerConfig"));
            }

            config = o;
        }
        #endregion
    }
}
