using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller
{
    class Configer : Model.BaseClass.FormController
    {
        Service.Setting setting;
        Service.Cache cache;

        public JObject config;
        string originalConfig, originalFile;
        ConfigerComponet.Editor editor;

        public Configer(int serverIndex = -1)
        {
            cache = Service.Cache.Instance;
            setting = Service.Setting.Instance;

            originalFile = string.Empty;
            originalConfig = string.Empty;

            LoadConfig(serverIndex);
        }

        #region public method

        public void Prepare()
        {
            editor = GetComponent<ConfigerComponet.Editor>();
            editor.ShowSection();
            Update();
        }

        public bool IsConfigSaved()
        {
            if (editor.IsChanged())
            {
                return false;
            }

            if (string.IsNullOrEmpty(originalConfig)
                && string.IsNullOrEmpty(originalFile))
            {
                return false;
            }

            if (string.IsNullOrEmpty(originalFile))
            {
                JObject orgConfig = JObject.Parse(
                Lib.Utils.Base64Decode(originalConfig));
                return JObject.DeepEquals(orgConfig, config);
            }

            JObject orgFile = JObject.Parse(originalFile);
            return JObject.DeepEquals(orgFile, config);
        }

        public string GetAlias()
        {
            return Lib.Utils.GetValue<string>(config, "v2raygcon.alias");
        }

        public void Update()
        {
            foreach (var component in this.GetAllComponents())
            {
                (component.Value as Controller.ConfigerComponet.ConfigerComponentController)
                    .Update(config);
            }
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
            if (!editor.Flush())
            {
                return false;
            }

            Update();

            if (setting.ReplaceServer(config, serverIndex))
            {
                MarkOriginalConfig();
                return true;
            }
            else
            {
                MessageBox.Show(I18N("DuplicateServer"));
                return false;
            }
        }

        public void AddNewServer()
        {
            if (!editor.Flush())
            {
                return;
            }

            Update();

            if (setting.AddServer(config))
            {
                MarkOriginalConfig();
            }
            else
            {
                MessageBox.Show(I18N("DuplicateServer"));
            }
        }

        public void MarkOriginalConfig()
        {
            originalFile = string.Empty;
            originalConfig = Lib.Utils.Config2Base64String(config);
        }

        public void MarkOriginalFile()
        {
            originalConfig = string.Empty;
            originalFile = GetConfigFormated();
        }

        public string GetConfigFormated()
        {
            return config.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public bool LoadJsonFromFile(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            try
            {
                var o = JObject.Parse(content);
                if (o == null)
                {
                    return false;
                }
                config = o;
                Update();
                editor.ShowSection();
                MarkOriginalFile();
                return true;
            }
            catch { }
            return false;
        }

        public void LoadServer(int index)
        {
            editor.DiscardChanges();
            editor.SelectSection(0);
            LoadConfig(index);
            Update();
            editor.ShowSection();
        }

        public void InjectConfigHelper(Action lambda)
        {
            if (!editor.Flush())
            {
                return;
            }

            lambda?.Invoke();

            Update();
            editor.ShowSection();
        }

        #endregion

        #region private method
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
                o = cache.tpl.LoadMinConfig();
                MessageBox.Show(I18N("EditorCannotLoadServerConfig"));
            }

            config = o;
            MarkOriginalConfig();
        }
        #endregion
    }
}
