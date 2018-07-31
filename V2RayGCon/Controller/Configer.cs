using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller
{
    class Configer
    {
        public int preSection;

        Service.Setting setting;
        Service.Cache cache;

        public JObject config;
        int separator;
        string originalConfig;
        Dictionary<int, string> sections;

        ConfigerComponet.Editor editor;
        Dictionary<string, Model.BaseClass.IConfigerComponent> components;

        public Configer(Scintilla mainEditor, int serverIndex = -1)
        {
            cache = Service.Cache.Instance;
            setting = Service.Setting.Instance;

            separator = Model.Data.Table.sectionSeparator;
            sections = Model.Data.Table.configSections;
            preSection = 0;
            ClearOriginalConfig();
            LoadConfig(serverIndex);

            components = new Dictionary<string, Model.BaseClass.IConfigerComponent>();
            editor = new ConfigerComponet.Editor();
            BindEditor(mainEditor);
            editor.content = config.ToString();

            Update();
        }

        #region public method

        public Model.BaseClass.IConfigerComponent GetComponent(string componentName)
        {
            if (!components.ContainsKey(componentName))
            {
                throw new KeyNotFoundException();
            }
            return components[componentName];
        }

        public void AddComponent(
            string componentName,
            Model.BaseClass.IConfigerComponent component)
        {
            component.Bind(this);
            components[componentName] = component;
        }

        public void ClearOriginalConfig()
        {
            originalConfig = string.Empty;
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
                // prevent loop infinitely
                return true;
            }

            if (CheckValid())
            {
                SaveChanges();
                preSection = curSection;
                ShowSection();
                Update();
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

        public void Update()
        {
            foreach (var component in components)
            {
                component.Value.Update(config);
            }
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
                    var inTpl = cache.tpl.LoadExample("inTpl");
                    inTpl["protocol"] = examples[preSection][index][2];
                    inTpl["settings"] = cache.tpl.LoadExample(key);
                    content = inTpl.ToString();
                }
                else if (preSection == Model.Data.Table.outboundIndex)
                {
                    var outTpl = cache.tpl.LoadExample("outTpl");
                    outTpl["protocol"] = examples[preSection][index][2];
                    outTpl["settings"] = cache.tpl.LoadExample(key);
                    content = outTpl.ToString();
                }
                else
                {
                    content = cache.tpl.LoadExample(key).ToString();
                }

                editor.content = content;
            }
            catch
            {
                MessageBox.Show(I18N("EditorNoExample"));
            }
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
                Update();
                ShowSection();
                return true;
            }
            catch { }
            return false;
        }

        public void LoadServer(int index)
        {
            LoadConfig(index);
            Update();
            ShowSection();
        }

        public void InjectConfigHelper(Action lamda)
        {
            if (!CheckValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }

            SaveChanges();

            lamda?.Invoke();

            Update();
            ShowSection();
        }

        #endregion

        #region private method
        void BindEditor(Control scintilla)
        {
            // bind scintilla
            var bs = new BindingSource();
            bs.DataSource = editor;
            scintilla.DataBindings.Add(
                "Text",
                bs,
                nameof(editor.content),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
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
            Update();
            return true;
        }

        void SaveChanges()
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

        bool CheckValid()
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
                o = cache.tpl.LoadMinConfig();
                MessageBox.Show(I18N("EditorCannotLoadServerConfig"));
            }

            config = o;
        }
        #endregion
    }
}
