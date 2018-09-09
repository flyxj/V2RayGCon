using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    public class Setting : Model.BaseClass.SingletonService<Setting>
    {
        private Model.Data.ServerList serverList;

        public event EventHandler<Model.Data.StrEvent> OnLog, OnUpdateNotifierText;
        public event EventHandler OnRequireMenuUpdate, OnRequireFlyPanelUpdate, OnSysProxyChanged;

        Model.BaseClass.CancelableTimeout
            lazyGCTimer = null,
            lazySaveServerListTimer = null;

        int preRunningServerCount = 0;

        Setting()
        {
            LoadServerList();
            serverList.BindEventsToAllServers();
            serverList.OnLog += (s, a) => SendLog(a.Data);
            serverList.ListChanged += LazySaveServerList;
            serverList.OnRequireMenuUpdate += InvokeEventOnRequireMenuUpdate;
            serverList.OnRequireFlyPanelUpdate += InvokeEventOnRequireFlyPanelUpdate;
            LazySaveServerList();
        }

        #region Properties

        ConcurrentQueue<string> _logCache = new ConcurrentQueue<string>();
        public string logCache
        {
            get
            {
                return string.Join(Environment.NewLine, _logCache);
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

        public string curSysProxy = null;

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
        public void PackSelectedServers()
        {
            serverList.PackSelectedServers();
        }

        public int GetSelectedServersCount()
        {
            return serverList.Where(s => s.isSelected).ToList().Count;
        }

        public void WakeupAutorunServers()
        {
            serverList.WakeupAutorunServersThen();
        }

        public void DeleteSelectedServers()
        {
            serverList.DeleteSelectedServersThen();
        }

        public bool DoSpeedTestOnSelectedServers()
        {
            return serverList.DoSpeedTestOnSelectedServers();
        }

        public void SaveServerListImmediately()
        {
            lazySaveServerListTimer.Timeout();
        }

        public void DisposeLazyTimers()
        {
            lazyGCTimer?.Release();
            lazySaveServerListTimer?.Release();
        }

        public void LazyGC()
        {
            // create in need
            if (lazyGCTimer == null)
            {
                var delay = Lib.Utils.Str2Int(StrConst("LazyGCDelay"));

                lazyGCTimer = new Model.BaseClass.CancelableTimeout(
                    () =>
                    {
                        System.GC.Collect();
                    }, delay * 1000);
            }

            lazyGCTimer.Start();
        }

        public void SaveFormRect(Form form, string key)
        {
            var list = GetWinFormRectList();
            list[key] = new Rectangle(
                form.Left, form.Top, form.Width, form.Height);
            Properties.Settings.Default.WinFormPosList =
                JsonConvert.SerializeObject(list);
            Properties.Settings.Default.Save();
        }

        public void RestoreFormRect(Form form, string key)
        {
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

        public List<Model.Data.ServerItem> GetActiveServerList()
        {
            return serverList.GetActiveServersList();
        }

        public void RestartServersByList(List<Model.Data.ServerItem> servers)
        {
            serverList.RetartServersByList(servers);
        }

        public void SetSystemProxy(string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                return;
            }

            Lib.ProxySetter.setProxy(link, true);
            curSysProxy = link;
            InvokeEventOnSysProxyChanged();
        }

        public void ClearSystemProxy()
        {
            Lib.ProxySetter.setProxy("", false);
            curSysProxy = string.Empty;
            InvokeEventOnSysProxyChanged();
        }

        public void StopAllServersThen(Action lambda = null)
        {
            serverList.StopAllServersThen(lambda);
        }

        public void RestartAllServers()
        {
            serverList.RestartAllServersThen();
        }

        public void StopAllSelectedThen(Action lambda = null)
        {
            serverList.StopAllSelectedThen(lambda);
        }

        public void RestartAllSelected()
        {
            serverList.RestartAllSelectedThen();
        }

        public void UpdateAllServersSummary()
        {
            serverList.UpdateAllServersSummary();
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

        public int GetServerListCount()
        {
            return serverList.Count;
        }

        public void ImportLinks(string links)
        {
            var tasks = new Task<Tuple<bool, List<string[]>>>[] {
                new Task<Tuple<bool, List<string[]>>>(
                    ()=>ImportVmessLinks(links)),

                new Task<Tuple<bool, List<string[]>>>(
                    ()=>ImportV2RayLinks(links)),

                new Task<Tuple<bool, List<string[]>>>(
                    ()=>ImportSSLinks(links)),
            };

            Task.Factory.StartNew(() =>
            {
                foreach (var task in tasks)
                {
                    task.Start();
                }
                Task.WaitAll(tasks);

                var allResults = new List<string[]>();  // all server including duplicate
                var isAddNewServer = false;  // add new server
                foreach (var task in tasks)
                {
                    isAddNewServer = isAddNewServer || task.Result.Item1;
                    allResults.AddRange(task.Result.Item2);
                    task.Dispose();
                }

                // show results
                if (isAddNewServer)
                {
                    serverList.UpdateAllServersSummary();
                }

                if (allResults.Count > 0)
                {
                    new Views.FormImportLinksResult(allResults);
                    Application.Run();
                }
                else
                {
                    MessageBox.Show(I18N("NoLinkFound"));
                }
            });
        }

        public List<Model.Data.UrlItem> GetGlobalImportItems()
        {
            try
            {
                var items = JsonConvert.DeserializeObject
                    <List<Model.Data.UrlItem>>(
                    Properties.Settings.Default.ImportUrls);

                if (items != null)
                {
                    return items;
                }
            }
            catch { };
            return new List<Model.Data.UrlItem>();
        }

        public void SaveGlobalImportItems(string options)
        {
            Properties.Settings.Default.ImportUrls = options;
            Properties.Settings.Default.Save();

            var list = serverList
                .Where(s => s.isInjectImport && s.isServerOn)
                .OrderBy(s => s.index)
                .ToList();

            RestartServersByList(list);
        }

        public List<Model.Data.UrlItem> GetSubscriptionItems()
        {
            try
            {
                var items = JsonConvert.DeserializeObject
                    <List<Model.Data.UrlItem>>(
                    Properties.Settings.Default.SubscribeUrls);

                if (items != null)
                {
                    return items;
                }
            }
            catch { };
            return new List<Model.Data.UrlItem>();
        }

        public void SaveSubscriptionItems(string options)
        {
            Properties.Settings.Default.SubscribeUrls = options;
            Properties.Settings.Default.Save();
        }

        public void LoadServerList()
        {
            serverList = new Model.Data.ServerList();

            Model.Data.ServerList list = null;
            try
            {
                list = JsonConvert.DeserializeObject
                    <Model.Data.ServerList>(
                    Properties.Settings.Default.ServerList);

                if (list == null)
                {
                    return;
                }
            }
            catch
            {
                return;
            }

            // make sure every config of server can be parsed correctly
            for (var i = list.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (JObject.Parse(list[i].config) == null)
                    {
                        list.RemoveAtQuiet(i);
                    }
                }
                catch
                {
                    list.RemoveAtQuiet(i);
                }
            }

            serverList = list;
        }

        public ReadOnlyCollection<Model.Data.ServerItem> GetServerList()
        {
            return serverList.OrderBy(s => s.index).ToList().AsReadOnly();
        }

        public string GetServerConfigByIndex(int index)
        {
            if (GetServerListCount() == 0
                || index < 0
                || index >= GetServerListCount())
            {
                return string.Empty;
            }

            return serverList[index].config;
        }

        public void DeleteAllServer()
        {
            serverList.DeleteAllServersThen(
                () => Service.Cache.Instance.core.Clear());
        }

        public bool AddServer(JObject config, bool quiet = false)
        {
            return serverList.AddServer(
                Lib.Utils.Config2String(config),
                quiet);
        }

        public int GetServerIndexByConfig(string configString)
        {
            return serverList.GetServerIndexByConfig(configString);
        }

        public bool ReplaceServerConfigByIndex(int orgIndex, string newConfig)
        {
            return serverList.ReplaceServerConfigByIndex(orgIndex, newConfig);
        }

        #endregion

        #region private method
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

        void InvokeEventOnSysProxyChanged()
        {
            try
            {
                OnSysProxyChanged?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        void InvokeEventOnRequireMenuUpdate(object sender, EventArgs args)
        {
            try
            {
                OnRequireMenuUpdate?.Invoke(this, EventArgs.Empty);
            }
            catch { }
            LazyGC();
        }

        void InvokeEventOnRequireFlyPanelUpdate(object sender, EventArgs args)
        {
            try
            {
                OnRequireFlyPanelUpdate?.Invoke(this, EventArgs.Empty);
            }
            catch { }
            LazyGC();
        }

        Tuple<bool, List<string[]>> ImportSSLinks(string text)
        {
            var isAddNewServer = false;
            var links = Lib.Utils.ExtractLinks(text, Model.Data.Enum.LinkTypes.ss);
            List<string[]> result = new List<string[]>();

            foreach (var link in links)
            {
                var config = Lib.Utils.SSLink2Config(link);

                if (config == null)
                {
                    result.Add(GenImportResult(link, false, I18N("DecodeFail")));
                    continue;
                }

                if (AddServer(config, true))
                {
                    isAddNewServer = true;
                    result.Add(GenImportResult(link, true, I18N("Success")));
                }
                else
                {
                    result.Add(GenImportResult(link, false, I18N("DuplicateServer")));
                }
            }

            return new Tuple<bool, List<string[]>>(isAddNewServer, result);
        }

        Tuple<bool, List<string[]>> ImportV2RayLinks(string text)
        {
            bool isAddNewServer = false;
            var links = Lib.Utils.ExtractLinks(text, Model.Data.Enum.LinkTypes.v2ray);
            List<string[]> result = new List<string[]>();

            foreach (var link in links)
            {
                try
                {
                    var config = JObject.Parse(
                        Lib.Utils.Base64Decode(
                            Lib.Utils.GetLinkBody(link)));

                    if (config != null)
                    {
                        if (AddServer(config, true))
                        {
                            isAddNewServer = true;
                            result.Add(GenImportResult(link, true, I18N("Success")));
                        }
                        else
                        {
                            result.Add(GenImportResult(link, false, I18N("DuplicateServer")));
                        }
                    }
                }
                catch
                {
                    // skip if error occured
                    result.Add(GenImportResult(link, false, I18N("DecodeFail")));
                }
            }

            return new Tuple<bool, List<string[]>>(isAddNewServer, result);
        }

        string[] GenImportResult(string link, bool success, string reason)
        {
            return new string[]
            {
                string.Empty,  // reserve for index
                link,
                success?"√":"×",
                reason,
            };
        }

        Tuple<bool, List<string[]>> ImportVmessLinks(string text)
        {
            var links = Lib.Utils.ExtractLinks(text, Model.Data.Enum.LinkTypes.vmess);
            var result = new List<string[]>();
            var isAddNewServer = false;

            foreach (var link in links)
            {
                var vmess = Lib.Utils.VmessLink2Vmess(link);
                var config = Lib.Utils.Vmess2Config(vmess);

                if (config == null)
                {
                    result.Add(GenImportResult(link, false, I18N("DecodeFail")));
                    continue;
                }

                if (AddServer(config, true))
                {
                    result.Add(GenImportResult(link, true, I18N("Success")));
                    isAddNewServer = true;
                }
                else
                {
                    result.Add(GenImportResult(link, false, I18N("DuplicateServer")));
                }
            }

            return new Tuple<bool, List<string[]>>(isAddNewServer, result);
        }

        void LazySaveServerList()
        {
            // create on demand
            if (lazySaveServerListTimer == null)
            {
                var delay = Lib.Utils.Str2Int(StrConst("LazySaveServerListDelay"));

                lazySaveServerListTimer =
                    new Model.BaseClass.CancelableTimeout(
                        () =>
                        {
                            string json = JsonConvert.SerializeObject(serverList);
                            Properties.Settings.Default.ServerList = json;
                            Properties.Settings.Default.Save();
                        },
                        delay * 1000);
            }

            lazySaveServerListTimer.Start();

            Task.Factory.StartNew(() => CheckRunningServersCount());
        }

        void CheckRunningServersCount()
        {
            var count = serverList.Where(s => s.isServerOn).ToList().Count;
            if (count == preRunningServerCount)
            {
                return;
            }
            preRunningServerCount = count;

            if (count <= 0)
            {
                UpdateNotifierText();
                return;
            }

            if (count == 1)
            {
                var first = serverList.FirstOrDefault(s => s.isServerOn);
                if (first == null)
                {
                    UpdateNotifierText();
                    return;
                }
                first.GetProxyAddrThen((str) => UpdateNotifierText(str));
                return;
            }

            UpdateNotifierText(count.ToString() + I18N("ServersAreRunning"));

        }

        void UpdateNotifierText(string title = null)
        {
            var text = string.IsNullOrEmpty(title) ? I18N("Description") : title;
            try
            {
                OnUpdateNotifierText?.Invoke(this, new Model.Data.StrEvent(text));
            }
            catch { }
        }

        #endregion
    }
}
