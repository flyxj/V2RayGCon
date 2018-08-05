using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller
{
    class Configer
    {
        Service.Setting setting;
        Service.Cache cache;

        public JObject config;
        string originalConfig;
        ConfigerComponet.Editor editor;

        Dictionary<Type, Model.BaseClass.ConfigerComponent> components;

        public Configer(int serverIndex = -1)
        {
            cache = Service.Cache.Instance;
            setting = Service.Setting.Instance;

            components = new Dictionary<Type, Model.BaseClass.ConfigerComponent>();
            ClearOriginalConfig();
            LoadConfig(serverIndex);
        }

        #region public method
        public T GetComponent<T>() where T : Model.BaseClass.ConfigerComponent
        {
            var type = typeof(T);

            if (!components.ContainsKey(type))
            {
                throw new KeyNotFoundException();
            }

            return components[type] as T;
        }

        public void Prepare()
        {
            editor = GetComponent<ConfigerComponet.Editor>();
            editor.ShowSection();
            Update();
        }

        public void Plug(Model.BaseClass.ConfigerComponent component)
        {
            var type = component.GetType();
            if (components.ContainsKey(type))
            {
                throw new ArgumentException("Key already existed!");
            }

            component.Bind(this);
            components[type] = component;
        }

        public void Plug(List<Model.BaseClass.ConfigerComponent> components)
        {
            foreach (var component in components)
            {
                Plug(component);
            }
        }

        public void ClearOriginalConfig()
        {
            originalConfig = string.Empty;
        }

        public string GetAlias()
        {
            return Lib.Utils.GetValue<string>(config, "v2raygcon.alias");
        }

        public void Update()
        {
            foreach (var component in components)
            {
                component.Value.Update(config);
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
                originalConfig = Lib.Utils.Config2Base64String(config);
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
                editor.ShowSection();
                return true;
            }
            catch { }
            return false;
        }

        public void LoadServer(int index)
        {
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
