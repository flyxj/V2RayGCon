using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    public class Setting : Model.BaseClass.SingletonService<Setting>
    {
        public event EventHandler<Model.Data.StrEvent> OnLog;
        public event EventHandler OnRequireNotifyTextUpdate;
        Model.Data.UserSettings userSettings;

        // Singleton need this private ctor.
        Setting()
        {
            userSettings = LoadUserSettings();
            runningServerSummary = string.Empty;
            isShutdown = false;
        }
        #region Properties
        public string runningServerSummary { get; set; }

        public bool isShutdown { get; set; }

        public string decodeCache
        {
            get
            {
                return userSettings.DecodeCache;
            }
            set
            {
                userSettings.DecodeCache = value;
                LazySaveUserSettings();
            }
        }

        public bool isUseV4
        {
            get
            {
                return userSettings.isUseV4Format;
            }
            set
            {
                userSettings.isUseV4Format = value;
                LazySaveUserSettings();
            }
        }

        public bool isPortable
        {
            get
            {
                return userSettings.isPortable;
            }
            set
            {
                userSettings.isPortable = value;
                LazySaveUserSettings();
            }
        }

        public bool isServerTrackerOn = false;

        public int serverPanelPageSize
        {
            get
            {
                var size = userSettings.ServerPanelPageSize;
                return Lib.Utils.Clamp(size, 1, 101);
            }
            set
            {
                userSettings.ServerPanelPageSize = Lib.Utils.Clamp(value, 1, 101);
                LazySaveUserSettings();
            }
        }

        public CultureInfo orgCulture = null;

        ConcurrentQueue<string> _logCache = new ConcurrentQueue<string>();
        public string logCache
        {
            get
            {
                return string.Join(Environment.NewLine, _logCache)
                    + System.Environment.NewLine;
            }
            private set
            {
                // keep 200 lines of log
                if (_logCache.Count > 300)
                {
                    var blackHole = "";
                    for (var i = 0; i < 100; i++)
                    {
                        _logCache.TryDequeue(out blackHole);
                    }
                }
                _logCache.Enqueue(value);
            }
        }

        public Model.Data.Enum.Cultures culture
        {
            get
            {
                var cultures = Model.Data.Table.Cultures;
                var c = userSettings.Culture;

                if (!cultures.ContainsValue(c))
                {
                    return Model.Data.Enum.Cultures.auto;
                }

                return cultures.Where(s => s.Value == c).First().Key;
            }

            set
            {
                var cultures = Model.Data.Table.Cultures;
                var c = Model.Data.Enum.Cultures.auto;
                if (cultures.ContainsKey(value))
                {
                    c = value;
                }
                userSettings.Culture = Model.Data.Table.Cultures[c];
                LazySaveUserSettings();
            }
        }

        public bool isShowConfigerToolsPanel
        {
            get
            {
                return userSettings.CfgShowToolPanel == true;
            }
            set
            {
                userSettings.CfgShowToolPanel = value;
                LazySaveUserSettings();
            }
        }

        public int maxLogLines
        {
            get
            {
                int n = userSettings.MaxLogLine;
                return Lib.Utils.Clamp(n, 10, 1000);
            }
            private set { }
        }

        #endregion

        #region public methods
        public void InvokeEventIgnoreErrorOnRequireNotifyTextUpdate()
        {
            try
            {
                OnRequireNotifyTextUpdate?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        public void Cleanup()
        {
            lazyGCTimer?.Release();
            lazySaveUserSettingsTimer?.Release();
            SaveUserSettingsNow();
        }

        readonly object saveUserSettingsLocker = new object();
        public void SaveUserSettingsNow()
        {
            lock (saveUserSettingsLocker)
            {
                if (userSettings.isPortable)
                {
                    DebugSendLog("Try save to setting file");
                    SaveUserSettingsToFile();
                    return;
                }

                DebugSendLog("Try save to properties");
                SetUserSettingFileIsPortableToFalse();
                SaveUserSettingsToProperties();
            }
        }

        /*
         * string something;
         * if(something == null){} // Boom!
         */
        Lib.Sys.CancelableTimeout lazyGCTimer = null;
        public void LazyGC()
        {
            // Create on demand.
            if (lazyGCTimer == null)
            {
                lazyGCTimer = new Lib.Sys.CancelableTimeout(
                    () => GC.Collect(),
                    1000 * Lib.Utils.Str2Int(StrConst.LazyGCDelay));
            }

            lazyGCTimer.Start();
        }

        public void SaveServerTrackerSetting(Model.Data.ServerTracker serverTrackerSetting)
        {
            userSettings.ServerTracker =
                JsonConvert.SerializeObject(serverTrackerSetting);
            LazySaveUserSettings();
        }

        public Model.Data.ServerTracker GetServerTrackerSetting()
        {
            var empty = new Model.Data.ServerTracker();
            Model.Data.ServerTracker r = null;
            try
            {
                r = JsonConvert
                    .DeserializeObject<Model.Data.ServerTracker>(
                        userSettings.ServerTracker);

                if (r.serverList == null)
                {
                    r.serverList = new List<string>();
                }
            }
            catch
            {
                return empty;
            }
            return r ?? empty;
        }

        public void SaveSysProxySetting(Model.Data.ProxyRegKeyValue proxy)
        {
            userSettings.SysProxySetting =
                JsonConvert.SerializeObject(proxy);
            LazySaveUserSettings();
        }

        public Model.Data.ProxyRegKeyValue GetSysProxySetting()
        {
            var empty = new Model.Data.ProxyRegKeyValue();
            Model.Data.ProxyRegKeyValue proxy = null;
            try
            {
                proxy = JsonConvert
                    .DeserializeObject<Model.Data.ProxyRegKeyValue>(
                        userSettings.SysProxySetting);
            }
            catch
            {
                return empty;
            }
            return proxy ?? empty;
        }

        public void SavePacServerSettings(
            Model.Data.PacServerSettings pacSetting)
        {
            userSettings.PacServerSettings =
                JsonConvert.SerializeObject(pacSetting);
            LazySaveUserSettings();
        }

        public Model.Data.PacServerSettings GetPacServerSettings()
        {
            Model.Data.PacServerSettings result = null;

            var empty = new Model.Data.PacServerSettings();
            try
            {
                result = JsonConvert
                    .DeserializeObject<Model.Data.PacServerSettings>(
                        userSettings.PacServerSettings);
            }
            catch
            {
                return empty;
            }

            return result ?? empty;
        }

        public List<Controller.CoreServerCtrl> LoadServerList()
        {
            var empty = new List<Controller.CoreServerCtrl>();

            List<Controller.CoreServerCtrl> list = null;
            try
            {
                list = JsonConvert
                    .DeserializeObject<List<Controller.CoreServerCtrl>>(
                        userSettings.ServerList);
            }
            catch { }

            if (list == null)
            {
                return empty;
            }

            // make sure every config of server can be parsed correctly
            return list.Where(s =>
            {
                try
                {
                    return JObject.Parse(s.config) != null;
                }
                catch { }
                return false;
            }).ToList();
        }

        public void SaveFormRect(Form form)
        {
            var key = form.GetType().Name;
            var list = GetWinFormRectList();

            list[key] = new Rectangle(form.Left, form.Top, form.Width, form.Height);
            userSettings.WinFormPosList = JsonConvert.SerializeObject(list);
            LazySaveUserSettings();
        }

        public void RestoreFormRect(Form form)
        {
            var key = form.GetType().Name;
            var list = GetWinFormRectList();

            if (!list.ContainsKey(key))
            {
                return;
            }

            var rect = list[key];
            var screen = Screen.PrimaryScreen.WorkingArea;

            form.Width = Math.Max(rect.Width, 300);
            form.Height = Math.Max(rect.Height, 200);
            form.Left = Lib.Utils.Clamp(rect.Left, 0, screen.Right - form.Width);
            form.Top = Lib.Utils.Clamp(rect.Top, 0, screen.Bottom - form.Height);
        }

        public void SendLog(string log)
        {
            logCache = log;
            try
            {
                OnLog?.Invoke(this, new Model.Data.StrEvent(log));
            }
            catch { }
        }

        public List<Model.Data.ImportItem> GetGlobalImportItems()
        {
            try
            {
                var items = JsonConvert
                    .DeserializeObject<List<Model.Data.ImportItem>>(
                        userSettings.ImportUrls);

                if (items != null)
                {
                    return items;
                }
            }
            catch { };
            return new List<Model.Data.ImportItem>();
        }

        public void SaveGlobalImportItems(string options)
        {
            userSettings.ImportUrls = options;
            LazySaveUserSettings();
        }

        public List<Model.Data.PluginInfoItem> GetPluginInfoItems()
        {
            try
            {
                var items = JsonConvert
                    .DeserializeObject<List<Model.Data.PluginInfoItem>>(
                        userSettings.PluginInfoItems);

                if (items != null)
                {
                    return items;
                }
            }
            catch { };
            return new List<Model.Data.PluginInfoItem>();
        }

        public void SavePluginInfoItems(
            List<Model.Data.PluginInfoItem> itemList)
        {
            string json = JsonConvert.SerializeObject(
                itemList ?? new List<Model.Data.PluginInfoItem>());

            userSettings.PluginInfoItems = json;
            LazySaveUserSettings();
        }

        public List<Model.Data.SubscriptionItem> GetSubscriptionItems()
        {
            try
            {
                var items = JsonConvert
                    .DeserializeObject<List<Model.Data.SubscriptionItem>>(
                        userSettings.SubscribeUrls);

                if (items != null)
                {
                    return items;
                }
            }
            catch { };
            return new List<Model.Data.SubscriptionItem>();
        }

        public void SaveSubscriptionItems(string options)
        {
            userSettings.SubscribeUrls = options;
            LazySaveUserSettings();
        }

        public void SaveServerList(List<Controller.CoreServerCtrl> serverList)
        {
            string json = JsonConvert.SerializeObject(
                serverList ?? new List<Controller.CoreServerCtrl>());

            userSettings.ServerList = json;
            LazySaveUserSettings();
        }
        #endregion

        #region private method
        void SetUserSettingFileIsPortableToFalse()
        {
            var filename = Properties.Resources.PortableUserSettingsFilename;
            if (!File.Exists(filename))
            {
                DebugSendLog("setting file not exists");
                return;
            }

            try
            {
                var s = JsonConvert
                    .DeserializeObject<Model.Data.UserSettings>(
                    File.ReadAllText(filename));

                DebugSendLog("Read setting file for unset portable");

                if (s.isPortable)
                {
                    s.isPortable = false;
                    DebugSendLog("Write setting file for unset portable");
                    File.WriteAllText(filename, JsonConvert.SerializeObject(s));
                }

                DebugSendLog("unset portable done");
            }
            catch
            {
                if (!isShutdown)
                {
                    // this is important do not use task
                    var msg = string.Format(I18N.UnsetPortableModeFail, filename);
                    MessageBox.Show(msg);
                }
            }
        }

        void SaveUserSettingsToProperties()
        {
            var p = Properties.Settings.Default;
            var s = userSettings;

            // int
            p.MaxLogLine = s.MaxLogLine;
            p.ServerPanelPageSize = s.ServerPanelPageSize;

            // bool
            p.UseV4Format = s.isUseV4Format;
            p.CfgShowToolPanel = s.CfgShowToolPanel;
            p.Portable = s.isPortable;

            // string
            p.ImportUrls = s.ImportUrls;
            p.DecodeCache = s.DecodeCache;
            p.SubscribeUrls = s.SubscribeUrls;
            p.PluginInfoItems = s.PluginInfoItems;
            p.Culture = s.Culture;
            p.ServerList = s.ServerList;
            p.PacServerSettings = s.PacServerSettings;
            p.SysProxySetting = s.SysProxySetting;
            p.ServerTracker = s.ServerTracker;
            p.WinFormPosList = s.WinFormPosList;

            p.Save();
        }

        void SaveUserSettingsToFile()
        {
            var filename = Properties.Resources.PortableUserSettingsFilename;
            try
            {
                var content = JsonConvert.SerializeObject(userSettings);
                File.WriteAllText(filename, content);
            }
            catch
            {
                if (!isShutdown)
                {
                    // this is important do not use task!
                    MessageBox.Show(I18N.SaveUserSettingsToFileFail);
                }
            }
        }

        Model.Data.UserSettings LoadUserSettingsFromPorperties()
        {
            DebugSendLog("Try read setting from properties");

            try
            {
                var p = Properties.Settings.Default;

                var result = new Model.Data.UserSettings
                {
                    // int
                    MaxLogLine = p.MaxLogLine,
                    ServerPanelPageSize = p.ServerPanelPageSize,

                    // bool
                    isUseV4Format = p.UseV4Format,
                    CfgShowToolPanel = p.CfgShowToolPanel,
                    isPortable = p.Portable,

                    // string
                    ImportUrls = p.ImportUrls,
                    DecodeCache = p.DecodeCache,
                    SubscribeUrls = p.SubscribeUrls,
                    PluginInfoItems = p.PluginInfoItems,
                    Culture = p.Culture,
                    ServerList = p.ServerList,
                    PacServerSettings = p.PacServerSettings,
                    SysProxySetting = p.SysProxySetting,
                    ServerTracker = p.ServerTracker,
                    WinFormPosList = p.WinFormPosList
                };
                return result;
            }
            catch { }
            return new Model.Data.UserSettings();
        }

        Model.Data.UserSettings LoadUserSettingsFromFile()
        {
            DebugSendLog("Try read setting from file");
            var filename = Properties.Resources.PortableUserSettingsFilename;
            if (File.Exists(filename))
            {
                try
                {
                    var result = JsonConvert
                        .DeserializeObject<Model.Data.UserSettings>(
                            File.ReadAllText(filename));
                    return result.isPortable ? result : null;
                }
                catch { }
            }
            return null;
        }

        Model.Data.UserSettings LoadUserSettings()
        {
            return LoadUserSettingsFromFile() ?? LoadUserSettingsFromPorperties();
        }

        Lib.Sys.CancelableTimeout lazySaveUserSettingsTimer = null;
        void LazySaveUserSettings()
        {
            if (lazySaveUserSettingsTimer == null)
            {
                lazySaveUserSettingsTimer = new Lib.Sys.CancelableTimeout(
                    SaveUserSettingsNow,
                    1000 * Lib.Utils.Str2Int(
                        StrConst.LazySaveUserSettingsDelay));
            }

            lazySaveUserSettingsTimer.Start();
        }

        Dictionary<string, Rectangle> winFormRectListCache = null;
        Dictionary<string, Rectangle> GetWinFormRectList()
        {
            if (winFormRectListCache != null)
            {
                return winFormRectListCache;
            }

            try
            {
                winFormRectListCache = JsonConvert
                    .DeserializeObject<Dictionary<string, Rectangle>>(
                        userSettings.WinFormPosList);
            }
            catch { }

            if (winFormRectListCache == null)
            {
                winFormRectListCache = new Dictionary<string, Rectangle>();
            }

            return winFormRectListCache;
        }
        #endregion

        #region debug
        void DebugSendLog(string content)
        {
#if DEBUG
            SendLog(content);
#endif
        }
        #endregion
    }
}
