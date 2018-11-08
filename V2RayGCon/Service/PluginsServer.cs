using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    class PluginsServer : Model.BaseClass.SingletonService<PluginsServer>
    {
        Service.Setting setting;
        Notifier notifier;

        Dictionary<string, Model.Plugin.IPlugin> plugins =
            new Dictionary<string, Model.Plugin.IPlugin>();

        PluginsServer() { }

        public void Run(
            Setting setting,
            Notifier notifier)
        {
            this.setting = setting;
            this.notifier = notifier;

            ReloadPlugins();
            UpdateNotifierMenu();
            var enabledList = GetCurEnabledPluginFileNames();
            StartPlugins(enabledList);
        }

        #region properties
        #endregion

        #region public methods
        public void Cleanup()
        {
            ClearPlugins();
        }

        public void UpdateNotifierMenu()
        {
            var enabledList = GetCurEnabledPluginFileNames();

            var children = new List<ToolStripMenuItem>();
            foreach (var fileName in enabledList)
            {
                if (plugins.ContainsKey(fileName))
                {
                    var plugin = plugins[fileName];
                    children.Add(
                        new ToolStripMenuItem(
                            fileName,
                            null,
                            (s, a) => plugin.Show()));
                }
            }

            notifier.UpdatePluginMenu(children.Count > 0 ?
                new ToolStripMenuItem(
                    I18N.Plugins,
                    Properties.Resources.Module_16x,
                    children.ToArray()) :
                null);
        }

        public void RefreshPlugins()
        {
            ReloadPlugins();
            SavePlugInfos();
            if (plugins.Count <= 0)
            {
                Task.Factory.StartNew(
                    () => MessageBox.Show(I18N.FindNoPlugin));
            }
        }
        #endregion

        #region private methods
        void ReloadPlugins()
        {
            ClearPlugins();
            var dllFileNames = SearchDllFiles();
            if (dllFileNames == null)
            {
                return;
            }

            foreach (var relativeFilePath in dllFileNames)
            {
                if (!Lib.Utils.IsTrustedPlugin(relativeFilePath))
                {
                    continue;
                }

                var plugin = Model.Plugin.Sandboxer.LoadTrustedPlugin(relativeFilePath);
                if (plugin == null)
                {
                    continue;
                }

                var key = Path.GetFileName(relativeFilePath);
                this.plugins[key] = plugin;
            }
        }

        void StartPlugins(List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                if (plugins.ContainsKey(fileName))
                {
                    plugins[fileName].Run();
                }
            }
        }

        void CleanupPlugins(List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                if (plugins.ContainsKey(fileName))
                {
                    plugins[fileName].Cleanup();
                }
            }
        }

        void ClearPlugins()
        {
            CleanupPlugins(plugins.Keys.ToList());
            plugins = new Dictionary<string, Model.Plugin.IPlugin>();
        }

        List<string> GetCurEnabledPluginFileNames()
        {
            var list = setting.GetPluginInfoItems();
            return list
                .Where(p => p.isUse)
                .Select(p => p.filename)
                .ToList();
        }

        void SavePlugInfos()
        {
            if (plugins.Count <= 0)
            {
                setting.SavePluginInfoItems(null);
                return;
            }

            var enabledList = GetCurEnabledPluginFileNames();
            var newPluginsInfo = new List<Model.Data.PluginInfoItem>();
            foreach (var item in this.plugins)
            {
                var plugin = item.Value;
                var filename = item.Key;
                var pluginInfo = new Model.Data.PluginInfoItem
                {
                    filename = filename,
                    name = plugin.Name,
                    version = plugin.Version,
                    description = plugin.Description,
                    isUse = enabledList.Contains(filename),
                };
                newPluginsInfo.Add(pluginInfo);
            }
            setting.SavePluginInfoItems(newPluginsInfo);
        }

        string[] SearchDllFiles()
        {
            var path = Properties.Resources.PluginsFolderName;
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path, "*.dll");
            }
            return null;
        }

        #endregion
    }
}
