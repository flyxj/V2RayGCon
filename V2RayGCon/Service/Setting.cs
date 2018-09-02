using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        Queue<string> logCache;

        Setting()
        {
            logCache = new Queue<string>();

            LoadServerList();
            serverList.BindEvents();

            curSysProxy = string.Empty;

            serverList.OnLog += (s, a) => SendLog(a.Data);

            SaveServerList();

            serverList.ListChanged += SaveServerList;
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
            set
            {
                int n = Lib.Utils.Clamp(value, 10, 1000);
                Properties.Settings.Default.MaxLogLine = n;
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        #region public methods
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
            serverList.DeleteAllItems();
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
        }

        void InvokeOnRequireFlyPanelUpdate(object sender, EventArgs args)
        {
            try
            {
                OnRequireFlyPanelUpdate?.Invoke(this, EventArgs.Empty);
            }
            catch { }
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


        void SaveServerList()
        {
            string json = JsonConvert.SerializeObject(serverList);
            Properties.Settings.Default.ServerList = json;
            Properties.Settings.Default.Save();
        }

        #endregion
    }
}
