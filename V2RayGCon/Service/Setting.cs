using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    public class Setting : Model.BaseClass.SingletonService<Setting>
    {
        private Model.Data.ServerList serverList;
        public event EventHandler<Model.Data.StrEvent> OnLog;
        public event EventHandler OnRequireMenuUpdate;
        public event EventHandler OnRequireFlyPanelUpdate;
        public event EventHandler OnSysProxyChanged;

        Dictionary<string, Rectangle> _winFormPosList = null; // cache
        Queue<string> logCache;

        Model.BaseClass.CancelableTimeout
            lazyGCTimer = null,
            lazySaveServerListTimer = null;


        Setting()
        {
            logCache = new Queue<string>();

            LoadServerList();
            serverList.BindEvents();

            curSysProxy = string.Empty;

            serverList.OnLog += (s, a) => SendLog(a.Data);

            LazySaveServerList();

            serverList.ListChanged += LazySaveServerList;
            serverList.OnRequireMenuUpdate += InvokeOnRequireMenuUpdate;
            serverList.OnRequireFlyPanelUpdate += InvokeOnRequireFlyPanelUpdate;
        }

        #region Properties

        public string curSysProxy;

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
        public void SaveServerList()
        {
            string json = JsonConvert.SerializeObject(serverList);
            Properties.Settings.Default.ServerList = json;
            Properties.Settings.Default.Save();
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

        public void SaveFormPosition(Form form, string key)
        {
            var rect = new Rectangle(form.Left, form.Top, form.Width, form.Height);
            SetWinFormPosition(key, rect);
        }

        public void RestoreFormPosition(Form form, string key)
        {
            var rect = GetWinFormPosition(key);
            var screen = Screen.PrimaryScreen.WorkingArea;
            if (rect.Left < 0 || rect.Top < 0
                || rect.Width < 300 || rect.Height < 200
                || rect.Right > screen.Right
                || rect.Bottom > screen.Bottom)
            {
                return;
            }

            form.Left = rect.Left;
            form.Top = rect.Top;
            form.Width = rect.Width;
            form.Height = rect.Height;
        }

        public List<int> GetActiveServerList()
        {
            return serverList.GetActiveServerList();
        }

        public void StartServersByList(List<int> servers)
        {
            serverList.StartServersByList(servers);
        }

        public void WakeupAutorunServer()
        {
            serverList.WakeupAutorunServerThen();
        }

        public void SetSysProxy(string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                return;
            }

            Lib.ProxySetter.setProxy(link, true);
            curSysProxy = link;
            InvokeOnSysProxyChanged();
        }

        public void ClearSysProxy()
        {
            Lib.ProxySetter.setProxy("", false);
            curSysProxy = string.Empty;
            InvokeOnSysProxyChanged();
        }

        public void StopAllServersThen(Action lambda = null)
        {
            serverList.StopAllServersThen(lambda);
        }

        public void RestartAllServers()
        {
            serverList.RestartAllServersThen();
        }

        public string GetLogCache()
        {
            return string.Join("\n", logCache);
        }

        public void UpdateAllSummary()
        {
            serverList.UpdateAllSummary();
        }

        public void SendLog(string log)
        {
            // cache 200 lines of log
            if (logCache.Count > 300)
            {
                for (var i = 0; i < 100; i++)
                {
                    logCache.Dequeue();
                }
            }
            logCache.Enqueue(log);

            var arg = new Model.Data.StrEvent(log);

            try
            {
                OnLog?.Invoke(this, arg);
            }
            catch { }
        }

        public int GetServerCount()
        {
            return serverList.Count;
        }

        public void ImportLinks(string links)
        {
            var that = this;

            var tasks = new Task<Tuple<bool, List<string[]>>>[] {
                new Task<Tuple<bool, List<string[]>>>(()=>ImportVmessLinks(links)),
                new Task<Tuple<bool, List<string[]>>>(()=>ImportV2RayLinks(links)),
                new Task<Tuple<bool, List<string[]>>>(()=>ImportSSLinks(links)),
            };

            Task.Factory.StartNew(() =>
            {
                // import all links
                foreach (var task in tasks)
                {
                    task.Start();
                }

                Task.WaitAll(tasks);

                // get results
                var allResults = new List<string[]>();
                var isAddNewServer = false;

                foreach (var task in tasks)
                {
                    isAddNewServer = isAddNewServer || task.Result.Item1;
                    allResults.AddRange(task.Result.Item2);
                    task.Dispose();
                }

                // show results
                if (isAddNewServer)
                {
                    serverList.UpdateAllSummary();
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

        public List<Model.Data.UrlItem> GetImportUrlItems()
        {
            try
            {
                var items = JsonConvert.DeserializeObject<List<Model.Data.UrlItem>>(
                    Properties.Settings.Default.ImportUrls);
                if (items != null)
                {
                    return items;
                }
            }
            catch { };
            return new List<Model.Data.UrlItem>();
        }

        public void SaveImportUrlItems(string options)
        {
            Properties.Settings.Default.ImportUrls = options;
            Properties.Settings.Default.Save();
            RestartAllServers();
        }

        public List<Model.Data.UrlItem> GetSubscriptionUrls()
        {
            try
            {
                var items = JsonConvert.DeserializeObject<List<Model.Data.UrlItem>>(
                    Properties.Settings.Default.SubscribeUrls);
                if (items != null)
                {
                    return items;
                }
            }
            catch { };
            return new List<Model.Data.UrlItem>();
        }

        public void SaveSubscriptionUrls(string options)
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
                list = JsonConvert.DeserializeObject<Model.Data.ServerList>(
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
            return serverList.AsReadOnly();
        }

        public string GetServerByIndex(int index)
        {
            if (GetServerCount() == 0
                || index < 0
                || index >= GetServerCount())
            {
                return string.Empty;
            }

            return serverList[index].config;
        }

        public void DeleteAllServer()
        {
            serverList.DeleteAllItemsThen(
                () => Service.Cache.Instance.core.Clear());
        }

        public bool AddServer(JObject config, bool quiet = false)
        {
            return serverList.AddConfig(Lib.Utils.Config2String(config), quiet);
        }

        public int SearchServer(string configString)
        {
            return serverList.SearchConfig(configString);
        }

        public bool ReplaceServer(int orgIndex, string newConfig)
        {
            return serverList.Replace(orgIndex, newConfig);
        }


        #endregion

        #region private method
        Rectangle GetWinFormPosition(string name)
        {
            // init _winFormPostList
            if (_winFormPosList == null)
            {
                try
                {
                    _winFormPosList = JsonConvert.DeserializeObject<
                        Dictionary<string, Rectangle>>(
                        Properties.Settings.Default.WinFormPosList);
                }
                catch { }
            }

            if (_winFormPosList == null)
            {
                _winFormPosList = new Dictionary<string, Rectangle>();
            }

            if (_winFormPosList.ContainsKey(name))
            {
                return _winFormPosList[name];
            }

            return new Rectangle(-1, -1, -1, -1);
        }

        void SetWinFormPosition(string name, Rectangle rect)
        {
            if (_winFormPosList == null)
            {
                _winFormPosList = new Dictionary<string, Rectangle>();
            }
            _winFormPosList[name] = rect;
            Properties.Settings.Default.WinFormPosList = JsonConvert.SerializeObject(_winFormPosList);
            Properties.Settings.Default.Save();
        }

        void InvokeOnSysProxyChanged()
        {
            try
            {
                OnSysProxyChanged?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        void InvokeOnRequireMenuUpdate(object sender, EventArgs args)
        {
            try
            {
                OnRequireMenuUpdate?.Invoke(this, EventArgs.Empty);
            }
            catch { }
            LazyGC();
        }

        void InvokeOnRequireFlyPanelUpdate(object sender, EventArgs args)
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
            // create in need
            if (lazySaveServerListTimer == null)
            {
                var delay = Lib.Utils.Str2Int(StrConst("LazySaveServerListDelay"));

                lazySaveServerListTimer =
                    new Model.BaseClass.CancelableTimeout(SaveServerList, delay * 1000);
            }

            lazySaveServerListTimer.Start();
        }

        #endregion
    }
}
