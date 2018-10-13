using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    public class Setting : Model.BaseClass.SingletonService<Setting>
    {
        Setting()
        {
            switch (culture)
            {
                case Model.Data.Enum.Cultures.enUS:
                    SetCulture("");
                    break;
                case Model.Data.Enum.Cultures.zhCN:
                    SetCulture("zh-CN");
                    break;
            }
        }

        public event EventHandler<Model.Data.StrEvent> OnLog, OnUpdateNotifierText;

        #region Properties
        public bool isServerTrackerOn = false;

        public int serverPanelPageSize
        {
            get
            {
                var size = Properties.Settings.Default.ServerPanelPageSize;
                return Lib.Utils.Clamp(size, 1, 101);
            }
            set
            {
                Properties.Settings.Default.ServerPanelPageSize =
                    Lib.Utils.Clamp(value, 1, 101);
                Properties.Settings.Default.Save();
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
                var c = Properties.Settings.Default.Culture;

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
                Properties.Settings.Default.Culture =
                    Model.Data.Table.Cultures[c];
                Properties.Settings.Default.Save();
            }

        }

        public bool isShowConfigerToolsPanel
        {
            get
            {
                return Properties.Settings.Default.CfgShowToolPanel == true;
            }
            set
            {
                Properties.Settings.Default.CfgShowToolPanel = value;
                Properties.Settings.Default.Save();
            }
        }

        public int maxLogLines
        {
            get
            {
                int n = Properties.Settings.Default.MaxLogLine;
                return Lib.Utils.Clamp(n, 10, 1000);
            }
            private set { }
        }

        #endregion

        #region public methods
        public void SaveServerTrackerSetting(Model.Data.ServerTracker serverTrackerSetting)
        {
            Properties.Settings.Default.ServerTracker =
                JsonConvert.SerializeObject(serverTrackerSetting);
            Properties.Settings.Default.Save();
        }

        public Model.Data.ServerTracker GetServerTrackerSetting()
        {
            var empty = new Model.Data.ServerTracker();
            Model.Data.ServerTracker r = null;
            try
            {
                r = JsonConvert.DeserializeObject<Model.Data.ServerTracker>(Properties.Settings.Default.ServerTracker);
            }
            catch
            {
                return empty;
            }
            return r ?? empty;
        }

        public void SaveSysProxySetting(Model.Data.ProxyRegKeyValue proxy)
        {
            Properties.Settings.Default.SysProxySetting =
                JsonConvert.SerializeObject(proxy);
            Properties.Settings.Default.Save();
        }

        public Model.Data.ProxyRegKeyValue GetSysProxySetting()
        {
            var empty = new Model.Data.ProxyRegKeyValue();
            Model.Data.ProxyRegKeyValue proxy = null;
            try
            {
                proxy = JsonConvert.DeserializeObject<Model.Data.ProxyRegKeyValue>(
                    Properties.Settings.Default.SysProxySetting);
            }
            catch
            {
                return empty;
            }
            return proxy ?? empty;
        }

        public void SavePacServerSettings(Model.Data.PacServerSettings pacSetting)
        {
            Properties.Settings.Default.PacServerSettings =
                JsonConvert.SerializeObject(pacSetting);
            Properties.Settings.Default.Save();
        }

        public Model.Data.PacServerSettings GetPacServerSettings()
        {
            Model.Data.PacServerSettings result = null;

            var empty = new Model.Data.PacServerSettings();
            try
            {
                result = JsonConvert.DeserializeObject<Model.Data.PacServerSettings>(
                    Properties.Settings.Default.PacServerSettings);
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
                list = JsonConvert.DeserializeObject
                    <List<Controller.CoreServerCtrl>>(
                    Properties.Settings.Default.ServerList);

                if (list == null)
                {
                    return empty;
                }
            }
            catch
            {
                return empty;
            }

            // make sure every config of server can be parsed correctly
            for (var i = list.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (JObject.Parse(list[i].config) == null)
                    {
                        list.RemoveAt(i);
                    }
                }
                catch
                {
                    list.RemoveAt(i);
                }
            }

            return list;
        }

        public void SaveFormRect(Form form)
        {
            var key = form.GetType().Name;

            var list = GetWinFormRectList();
            list[key] = new Rectangle(
                form.Left, form.Top, form.Width, form.Height);
            Properties.Settings.Default.WinFormPosList =
                JsonConvert.SerializeObject(list);
            Properties.Settings.Default.Save();
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
                var items = JsonConvert.DeserializeObject
                    <List<Model.Data.ImportItem>>(
                    Properties.Settings.Default.ImportUrls);

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
            Properties.Settings.Default.ImportUrls = options;
            Properties.Settings.Default.Save();
        }

        public List<Model.Data.SubscriptionItem> GetSubscriptionItems()
        {
            try
            {
                var items = JsonConvert.DeserializeObject
                    <List<Model.Data.SubscriptionItem>>(
                    Properties.Settings.Default.SubscribeUrls);

                if (items != null)
                {
                    return items;
                }
            }
            catch { };
            return new List<Model.Data.SubscriptionItem>();
        }

        public void UpdateNotifierText(string title = null)
        {
            var text = string.IsNullOrEmpty(title) ? I18N.Description : title;
            try
            {
                OnUpdateNotifierText?.Invoke(this, new Model.Data.StrEvent(text));
            }
            catch { }
        }

        public void SaveSubscriptionItems(string options)
        {
            Properties.Settings.Default.SubscribeUrls = options;
            Properties.Settings.Default.Save();
        }

        public void SaveServerList(List<Controller.CoreServerCtrl> serverList)
        {
            string json = JsonConvert.SerializeObject(serverList);
            Properties.Settings.Default.ServerList = json;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region private method
        private static void SetCulture(string cultureString)
        {
            var ci = new CultureInfo(cultureString);

            Thread.CurrentThread.CurrentCulture.GetType()
                .GetProperty("DefaultThreadCurrentCulture")
                .SetValue(Thread.CurrentThread.CurrentCulture, ci, null);

            Thread.CurrentThread.CurrentCulture.GetType()
                .GetProperty("DefaultThreadCurrentUICulture")
                .SetValue(Thread.CurrentThread.CurrentCulture, ci, null);
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
                winFormRectListCache = JsonConvert.DeserializeObject<
                    Dictionary<string, Rectangle>>(
                    Properties.Settings.Default.WinFormPosList);
            }
            catch { }

            if (winFormRectListCache == null)
            {
                winFormRectListCache = new Dictionary<string, Rectangle>();
            }

            return winFormRectListCache;
        }
        #endregion
    }
}
