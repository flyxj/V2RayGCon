using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace V2RayGCon.Service
{
    public class PluginsServer : Model.BaseClass.SingletonService<PluginsServer>
    {
        Service.Setting setting;
        Model.Plugin.ApiServer auxApi = null;
        Dictionary<string, Model.Plugin.PluginContracts.IPlugin> plugins =
            new Dictionary<string, Model.Plugin.PluginContracts.IPlugin>();

        PluginsServer()
        {
            auxApi = new Model.Plugin.ApiServer();
        }

        public void Run(Setting setting)
        {
            this.setting = setting;
        }

        #region properties
        #endregion

        #region public methods
        public void ReloadPluginInfo()
        {
            this.plugins = new Dictionary<string, Model.Plugin.PluginContracts.IPlugin>();
            var pluginInfos = new List<Model.Data.PluginInfoItem>();

            var dllFileNames = GetDllFileNames();
            if (dllFileNames == null)
            {
                setting.SavePluginInfoItems(pluginInfos);
                return;
            }

            foreach (var fileName in dllFileNames)
            {
                var plugin = CreatePluginInstance(
                    LoadPluginTypeFromAssembly(
                         LoadAssemblyFromFile(fileName)));

                if (plugin == null)
                {
                    Lib.Sys.Log.Error($"Load {fileName} fail!");
                    continue;
                }

                var key = Path.GetFileName(fileName);
                this.plugins[key] = plugin;
            }

            UpdatePluginInfos();
        }
        #endregion

        #region private methods
        bool GetIsUse(string filename, List<Model.Data.PluginInfoItem> pluginInfos)
        {
            var item = pluginInfos.FirstOrDefault(p => p.filename == filename);
            return item == null ? false : item.isUse;
        }

        void UpdatePluginInfos()
        {
            var oldPluginsInfo = setting.GetPluginInfoItems();
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
                    isUse = GetIsUse(filename, oldPluginsInfo),
                };
                newPluginsInfo.Add(pluginInfo);
            }
            setting.SavePluginInfoItems(newPluginsInfo);
        }

        /// <summary>
        /// return null if fail or param is null
        /// </summary>
        /// <param name="pluginType"></param>
        /// <returns></returns>
        Model.Plugin.PluginContracts.IPlugin CreatePluginInstance(Type pluginType)
        {
            if (pluginType == null)
            {
                return null;
            }

            try
            {
                return (Model.Plugin.PluginContracts.IPlugin)
                    Activator.CreateInstance(pluginType, auxApi);
            }
            catch
            {
                Lib.Sys.Log.Error("Load plugin fail.");
            }

            return null;
        }

        /// <summary>
        /// return null if fail or param is null
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        Type LoadPluginTypeFromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                return null;
            }

            var pluginType = typeof(Model.Plugin.PluginContracts.IPlugin);
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsInterface
                    && !type.IsAbstract
                    && type.GetInterface(pluginType.Name) != null)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// return null if fail, return null if param is null
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Assembly LoadAssemblyFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            try
            {
                AssemblyName an = AssemblyName.GetAssemblyName(fileName);
                return Assembly.Load(an);
            }
            catch { }
            return null;
        }

        string[] GetDllFileNames()
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
