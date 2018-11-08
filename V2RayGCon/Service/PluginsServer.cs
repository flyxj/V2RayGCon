using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace V2RayGCon.Service
{
    public class PluginsServer : Model.BaseClass.SingletonService<PluginsServer>
    {
        Service.Setting setting;

        Dictionary<string, Model.Plugin.PluginContracts.IPlugin> plugins =
            new Dictionary<string, Model.Plugin.PluginContracts.IPlugin>();

        PluginsServer() { }

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

            Model.Plugin.PluginContracts.IPlugin plugin;
            foreach (var fileName in dllFileNames)
            {
                Type pluginType = Model.Plugin.Sandboxer.LoadPluginTypeFromFile(fileName);

                if (pluginType == null || !Lib.Utils.IsTrustedPlugin(fileName))
                {
                    continue;
                }

                plugin = Model.Plugin.Sandboxer.CreatePluginInstance(
                    pluginType);


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

        /// <summary>
        /// Return null if fail to load dll
        /// </summary>
        /// <param name="pluginFileName">for individual namespaces</param>
        /// <returns></returns>
        Model.Plugin.PluginContracts.IPlugin CreatePluginInstanceInSandbox(
            string pluginFileName,
            Type pluginType)
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/misc/how-to-run-partially-trusted-code-in-a-sandbox

            //Setting the AppDomainSetup. It is very important to set the ApplicationBase to a folder   
            //other than the one in which the sandboxer resides.  
            AppDomainSetup adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = Path.GetFullPath(Properties.Resources.PluginsFolderName);

            //Setting the permissions for the AppDomain. We give the permission to execute and to   
            //read/discover the location where the untrusted code is loaded.  
            PermissionSet permSet = new PermissionSet(PermissionState.None);
            permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

            //We want the sandboxer assembly's strong name, so that we can add it to the full trust list.  
            StrongName fullTrustAssembly = typeof(Model.Plugin.Sandboxer).Assembly.Evidence.GetHostEvidence<StrongName>();

            //Now we have everything we need to create the AppDomain, so let's create it.  
            AppDomain newDomain = AppDomain.CreateDomain("Sandbox" + pluginFileName, null, adSetup, permSet, fullTrustAssembly);

            //Use CreateInstanceFrom to load an instance of the Sandboxer class into the  
            //new AppDomain.   
            ObjectHandle handle = Activator.CreateInstanceFrom(
                newDomain,
                typeof(Model.Plugin.Sandboxer).Assembly.ManifestModule.FullyQualifiedName,
                typeof(Model.Plugin.Sandboxer).FullName);


            //Unwrap the new domain instance into a reference in this domain and use it to execute the   
            //untrusted code.  
            var box = handle.Unwrap() as Model.Plugin.Sandboxer;
            return box.CreatePartiallyTrustedPluginInstance(pluginType) ? box : null;
        }

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
