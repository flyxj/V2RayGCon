using System.Collections.Generic;
using System.IO;

namespace V2RayGCon.Service
{
    public class PluginsServer : Model.BaseClass.SingletonService<PluginsServer>
    {
        Service.Setting setting;
        Model.Plugin.ApiServer auxApi = null;

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
            var filenames = GetPluginFilenames();
            if (filenames == null)
            {
                setting.SavePluginInfoItems(new List<Model.Data.PluginInfoItem>());
            }

            // to do 
            // https://code.msdn.microsoft.com/windowsdesktop/Creating-a-simple-plugin-b6174b62
        }

        string[] GetPluginFilenames()
        {
            var path = Properties.Resources.PluginsFolderName;

            string[] dllFileNames = null;
            if (Directory.Exists(path))
            {
                dllFileNames = Directory.GetFiles(path, "*.dll");
            }
            return dllFileNames;
        }
        #endregion

        #region private methods
        #endregion
    }
}
