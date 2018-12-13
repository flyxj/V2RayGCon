﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    class PluginsServer : Model.BaseClass.SingletonService<PluginsServer>
    {
        Setting setting;
        Notifier notifier;
        Plugin.ApiServ apis = new Plugin.ApiServ();

        Dictionary<string, VgcApis.IPlugin> plugins =
            new Dictionary<string, VgcApis.IPlugin>();

        PluginsServer() { }

        public void Run(
            Setting setting,
            Servers servers,
            Notifier notifier)
        {
            this.setting = setting;
            this.notifier = notifier;

            apis.Run(setting, servers);
            plugins = SearchForPlugins();
            RestartPlugins();
        }

        #region properties
        #endregion

        #region public methods
        public void RestartPlugins()
        {
            var enabledList = GetCurEnabledPluginFileNames();
            foreach (var p in plugins)
            {
                if (enabledList.Contains(p.Key))
                {
                    p.Value.Run(apis);
                }
                else
                {
                    p.Value.Cleanup();
                }
            }

            UpdateNotifierMenu(enabledList);
        }

        public void Cleanup()
        {
            ClearPlugins();
        }

        public List<Model.Data.PluginInfoItem> GetterAllPluginsInfo()
        {
            return GetPluginInfoFrom(plugins);
        }

        #endregion

        #region private methods
        void UpdateNotifierMenu(List<string> enabledList)
        {
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

        public Dictionary<string, VgcApis.IPlugin> SearchForPlugins()
        {
            // Original design of plugins would load dll files from file system.
            // That is why loading logic looks so complex.
            var pluginList = new Dictionary<string, VgcApis.IPlugin>();
            var plugins = new VgcApis.IPlugin[] {
                new Lunar.Lunar(),
                new Pacman.Pacman(),

#if !V2RAYGCON_LITE
                // Many thanks to windows defender
                new ProxySetter.ProxySetter(),
#endif

                new Statistics.Statistics(),
            };

            foreach (var plugin in plugins)
            {
                pluginList.Add(plugin.Name, plugin);
            }
            return pluginList;
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
            plugins = new Dictionary<string, VgcApis.IPlugin>();
        }

        List<string> GetCurEnabledPluginFileNames()
        {
            var list = setting.GetPluginInfoItems();
            return list
                .Where(p => p.isUse)
                .Select(p => p.filename)
                .ToList();
        }

        List<Model.Data.PluginInfoItem> GetPluginInfoFrom(
            Dictionary<string, VgcApis.IPlugin> pluginList)
        {
            if (pluginList.Count <= 0)
            {
                return new List<Model.Data.PluginInfoItem>();
            }

            var enabledList = GetCurEnabledPluginFileNames();
            var infos = new List<Model.Data.PluginInfoItem>();
            foreach (var item in pluginList)
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
                infos.Add(pluginInfo);
            }
            return infos;
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
