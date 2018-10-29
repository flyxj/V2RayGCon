using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    public class PacServer : Model.BaseClass.SingletonService<PacServer>
    {
        Setting setting;
        public event EventHandler OnPACServerStatusChanged;

        Lib.Nets.SimpleWebServer webServer = null;
        object webServerLock = new object();

        Dictionary<bool, string[]> defaultPacCache = null;
        string customPacCache = string.Empty;
        FileSystemWatcher customPacFileWatcher = null;
        Func<HttpListenerRequest, string> postRequestHandler = null;

        PacServer() { }

        public void Run(Setting setting)
        {
            this.setting = setting;

            ClearCache();
            var pacSetting = setting.GetPacServerSettings();
            var proxySetting = setting.GetSysProxySetting();

            if (pacSetting.isAutorun || !string.IsNullOrEmpty(proxySetting.autoConfigUrl))
            {
                RestartPacServer();
            }

            // becareful issue #9 #3
            if (!Lib.Utils.IsProxyNotSet(proxySetting))
            {
                Lib.Sys.ProxySetter.SetProxy(proxySetting);
            }
        }

        #region properties method
        bool _isWebServRunning = false;
        public bool isWebServRunning
        {
            get
            {
                return _isWebServRunning;
            }
            private set
            {
                _isWebServRunning = value;
            }
        }
        #endregion

        #region static method
        public static Model.Data.Enum.SystemProxyMode DetectSystemProxyMode(Model.Data.ProxyRegKeyValue proxySetting)
        {
            if (!string.IsNullOrEmpty(proxySetting.autoConfigUrl))
            {
                return Model.Data.Enum.SystemProxyMode.PAC;
            }

            if (proxySetting.proxyEnable)
            {
                return Model.Data.Enum.SystemProxyMode.Global;
            }

            return Model.Data.Enum.SystemProxyMode.None;
        }
        #endregion

        #region public method

        Lib.Sys.CancelableTimeout lazySysProxyUpdaterTimer = null;
        public void LazySysProxyUpdater(bool isSocks, string ip, int port)
        {
            lazySysProxyUpdaterTimer?.Release();
            lazySysProxyUpdaterTimer = null;

            var proxySetting = setting.GetSysProxySetting();

            Action setProxy = null;
            switch (DetectSystemProxyMode(proxySetting))
            {
                case Model.Data.Enum.SystemProxyMode.None:
                    return;
                case Model.Data.Enum.SystemProxyMode.PAC:
                    // get current pac mode (white list or black list)
                    var p = Lib.Utils.GetProxyParamsFromUrl(proxySetting.autoConfigUrl);
                    if (p == null)
                    {
                        p = new Model.Data.PacUrlParams();
                    }
                    p.ip = ip;
                    p.port = port;
                    p.isSocks = isSocks;
                    setProxy = () => SetPACProx(p);
                    break;
                case Model.Data.Enum.SystemProxyMode.Global:
                    // global proxy must be http 
                    if (isSocks)
                    {
                        return;
                    }
                    setProxy = () => SetGlobalProxy(ip, port);
                    break;
            }

            lazySysProxyUpdaterTimer = new Lib.Sys.CancelableTimeout(setProxy, 1000);
            lazySysProxyUpdaterTimer.Start();
        }

        public void StartPacServer()
        {
            lock (webServerLock)
            {
                if (isWebServRunning)
                {
                    return;
                }
                RestartPacServer();
            }
        }

        public void SetPACProx(Model.Data.PacUrlParams param)
        {
            var proxy = new Model.Data.ProxyRegKeyValue();
            proxy.autoConfigUrl = GenPacUrl(param);
            Lib.Sys.ProxySetter.SetProxy(proxy);
            setting.SaveSysProxySetting(proxy);
            StartPacServer();
            InvokeOnPACServerStatusChanged();
        }

        public void SetGlobalProxy(string ip, int port)
        {
            var proxy = new Model.Data.ProxyRegKeyValue();
            proxy.proxyEnable = true;
            proxy.proxyServer = string.Format(
                "{0}:{1}",
                ip == "0.0.0.0" ? "127.0.0.1" : ip,
                port.ToString());
            Lib.Sys.ProxySetter.SetProxy(proxy);
            setting.SaveSysProxySetting(proxy);
            InvokeOnPACServerStatusChanged();
        }

        public void ClearSysProxy()
        {
            var proxy = new Model.Data.ProxyRegKeyValue();
            setting.SaveSysProxySetting(proxy);
            Lib.Sys.ProxySetter.SetProxy(proxy);
            InvokeOnPACServerStatusChanged();
        }

        public void RestartPacServer()
        {
            var pacSetting = setting.GetPacServerSettings();
            var prefix = GenPrefix(pacSetting.port);

            lock (webServerLock)
            {
                if (isWebServRunning)
                {
                    StopPacServer();
                }

                try
                {
                    webServer = new Lib.Nets.SimpleWebServer(WebRequestDispatcher, prefix);
                    webServer.Start();
                    isWebServRunning = true;
                }
                catch
                {
                    Task.Factory.StartNew(
                        () => MessageBox.Show(I18N.StartPacServFail));
                    return;
                }

                StartFileWatcher(pacSetting);
            }
            InvokeOnPACServerStatusChanged();
        }

        void StartFileWatcher(Model.Data.PacServerSettings pacSetting)
        {
            StopCustomPacFileWatcher();
            if (!pacSetting.isUseCustomPac)
            {
                return;
            }

            var filename = pacSetting.customPacFilePath;
            if (!File.Exists(filename))
            {
                return;
            }

            var path = Path.GetDirectoryName(filename);

            customPacFileWatcher = new FileSystemWatcher
            {
                Path = (string.IsNullOrEmpty(path) ? Lib.Utils.GetAppDir() : path),
                Filter = Path.GetFileName(filename),
            };

            customPacFileWatcher.Changed += (s, a) => LazyCustomPacFileCacheUpdate();
            customPacFileWatcher.Created += (s, a) => LazyCustomPacFileCacheUpdate();
            customPacFileWatcher.Deleted += (s, a) => LazyCustomPacFileCacheUpdate();

            customPacFileWatcher.EnableRaisingEvents = true;
        }

        Lib.Sys.CancelableTimeout lazyCustomPacFileCacheUpdateTimer = null;
        void LazyCustomPacFileCacheUpdate()
        {
            // this program is getting lazier and lazier.
            if (lazyCustomPacFileCacheUpdateTimer == null)
            {
                lazyCustomPacFileCacheUpdateTimer = new Lib.Sys.CancelableTimeout(
                    UpdateCustomPacCache, 2000);
            }

            lazyCustomPacFileCacheUpdateTimer.Start();
        }

        void UpdateCustomPacCache()
        {
            var pacSetting = setting.GetPacServerSettings();
            var filename = pacSetting.customPacFilePath;
            customPacCache = string.Empty;
            if (File.Exists(filename))
            {
                try
                {
                    var content = File.ReadAllText(filename);
                    customPacCache = content ?? string.Empty;
                }
                catch { }
            }
            // Debug.WriteLine("content: " + customPacCache);
        }

        public void RegistPostRequestHandler(Func<HttpListenerRequest, string> handler)
        {
            this.postRequestHandler = handler;
        }

        public void Cleanup()
        {
            postRequestHandler = null;
            StopPacServer();
            lazyCustomPacFileCacheUpdateTimer?.Release();
        }

        public string GenPacUrl(Model.Data.PacUrlParams param)
        {
            var pacSetting = setting.GetPacServerSettings();
            // e.g. http://localhost:3000/pac/?&port=5678&ip=1.2.3.4&proto=socks&type=whitelist&key=rnd
            return string.Format(
                "{0}?&type={1}&proto={2}&ip={3}&port={4}&timeout={5}",
                GenPrefix(pacSetting.port),
                param.isWhiteList ? "whitelist" : "blacklist",
                param.isSocks ? "socks" : "http",
                param.ip == "0.0.0.0" ? "127.0.0.1" : param.ip,
                param.port.ToString(),
                Lib.Utils.RandomHex(16));
        }

        public void StopPacServer()
        {
            lock (webServerLock)
            {
                if (isWebServRunning)
                {
                    webServer.Stop();
                    ClearCache();
                    isWebServRunning = false;
                }

                StopCustomPacFileWatcher();
            }
            InvokeOnPACServerStatusChanged();
        }
        #endregion

        #region private method
        void StopCustomPacFileWatcher()
        {
            if (customPacFileWatcher == null)
            {
                return;
            }
            customPacFileWatcher.EnableRaisingEvents = false;
            customPacFileWatcher.Dispose();
            customPacFileWatcher = null;
        }
        void InvokeOnPACServerStatusChanged()
        {
            try
            {
                OnPACServerStatusChanged?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        void ClearCache()
        {
            defaultPacCache = new Dictionary<bool, string[]> {
                { true,null },
                { false,null },
            };
        }

        string GenPrefix(int port)
        {
            return string.Format("http://localhost:{0}/pac/", port);
        }

        Tuple<string, string> WebRequestDispatcher(HttpListenerRequest request)
        {
            if (request.HttpMethod.ToLower() == "post")
            {
                var result = postRequestHandler?.Invoke(request);
                return new Tuple<string, string>(result ?? "no handler", "application/json");
            }

            // e.g. http://localhost:3000/pac/?&port=5678&ip=1.2.3.4&proto=socks&type=whitelist&key=rnd
            var urlParam = Lib.Utils.GetProxyParamsFromUrl(request.Url.AbsoluteUri);
            if (urlParam == null)
            {
                return new Tuple<string, string>("Bad request params.", null);
            }

            if (urlParam.isDebug)
            {
                return ResponsWithDebugger(urlParam);
            }

            if (urlParam.mime == "js")
            {
                return ResponseWithJsFile(urlParam);
            }

            return ResponseWithPacFile(urlParam);
        }

        private Tuple<string, string> ResponseWithJsFile(Model.Data.PacUrlParams urlParam)
        {
            var response = ResponseWithPacFile(urlParam);
            var mime = "application/javascript";
            return new Tuple<string, string>(response.Item1, mime);
        }

        private Tuple<string, string> ResponsWithDebugger(Model.Data.PacUrlParams urlParam)
        {
            var tpl = StrConst.PacDebuggerTpl;
            var url = GenPacUrl(urlParam);
            var prefix = url.Split('?')[0];
            var html = tpl.Replace("__PacServerUrl__", url)
                .Replace("__PacPrefixUrl__", prefix);
            var mime = "text/html; charset=UTF-8";
            return new Tuple<string, string>(html, mime);
        }

        private Tuple<string, string> ResponseWithPacFile(Model.Data.PacUrlParams urlParam)
        {
            // ie require this header
            var mime = "application/x-ns-proxy-autoconfig; charset=UTF-8";

            var pacSetting = setting.GetPacServerSettings();
            StringBuilder content;
            if (pacSetting.isUseCustomPac)
            {
                content = GenCustomPacFile(urlParam, pacSetting);
            }
            else
            {
                content = GenDefaultPacFile(
                    urlParam,
                    pacSetting.customWhiteList,
                    pacSetting.customBlackList);
            }

            return new Tuple<string, string>(content.ToString(), mime);
        }

        StringBuilder GenCustomPacFile(
            Model.Data.PacUrlParams urlParam,
            Model.Data.PacServerSettings pacSetting)
        {
            var header = new Model.Data.PacCustomHeader(
                urlParam,
                pacSetting.customWhiteList,
                pacSetting.customBlackList);

            if (string.IsNullOrEmpty(customPacCache))
            {
                // 抢救一下，还不行就算了
                UpdateCustomPacCache();
            }

            return new StringBuilder("var customSettings = ")
                .Append(JsonConvert.SerializeObject(header))
                .Append(";")
                .Append(customPacCache);
        }

        private StringBuilder GenDefaultPacFile(
            Model.Data.PacUrlParams urlParam,
            string customWhiteList,
            string customBlackList)
        {
            var proxy = urlParam.isSocks ?
                            "SOCKS5 {0}:{1}; SOCKS {0}:{1}; DIRECT" :
                            "PROXY {0}:{1}; DIRECT";
            var mode = urlParam.isWhiteList ? "white" : "black";
            var domainAndCidrs = GenDomainAndCidrContent(
                urlParam.isWhiteList,
                customWhiteList,
                customBlackList);

            var content = new StringBuilder(StrConst.PacJsTpl)
                .Replace("__PROXY__", string.Format(proxy, urlParam.ip, urlParam.port))
                .Replace("__MODE__", mode)
                .Replace("__DOMAINS__", domainAndCidrs[0])
                .Replace("__CIDRS__", domainAndCidrs[1]);
            return content;
        }

        void MergeCustomPacSetting(
            ref List<string> domainList,
            ref List<long[]> cidrList,
            string customList)
        {
            var list = customList.Split(
                new char[] { '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in list)
            {
                var item = line.Trim();

                // ignore single line comment
                if (item.StartsWith("//"))
                {
                    continue;
                }

                if (Lib.Utils.IsIP(item))
                {
                    var v = Lib.Utils.IP2Long(item);
                    cidrList.Add(new long[] { v, v });
                    continue;
                }

                if (Lib.Utils.IsCidr(item))
                {
                    cidrList.Add(Lib.Utils.Cidr2RangeArray(item));
                    continue;
                }

                if (!domainList.Contains(item))
                {
                    domainList.Add(item);
                }
            }
        }

        /// <summary>
        /// string[0] domain list
        /// string[1] cidr list
        /// </summary>
        /// <param name="isWhiteList"></param>
        /// <returns></returns>
        string[] GenDomainAndCidrContent(
            bool isWhiteList,
            string customWhiteList,
            string customBlackList)
        {
            if (defaultPacCache[isWhiteList] != null)
            {
                return defaultPacCache[isWhiteList];
            }

            var domainList = Lib.Utils.GetPacDomainList(isWhiteList);
            var cidrList = Lib.Utils.GetPacCidrList(isWhiteList);

            // merge user settings
            var customUrlList = isWhiteList ? customWhiteList : customBlackList;
            MergeCustomPacSetting(ref domainList, ref cidrList, customUrlList);

            var cidrSimList = Lib.Utils.CompactCidrList(ref cidrList);
            var domainSB = new StringBuilder();
            foreach (var url in domainList)
            {
                domainSB.Append("'")
                    .Append(url)
                    .Append("':1,");
            }
            domainSB.Length--;

            var cidrSB = new StringBuilder();
            foreach (var cidr in cidrSimList)
            {
                cidrSB.Append("[")
                    .Append(cidr[0])
                    .Append(",")
                    .Append(cidr[1])
                    .Append("],");
            }
            cidrSB.Length--;

            defaultPacCache[isWhiteList] = new string[] {
                domainSB.ToString(),
                cidrSB.ToString(),
            };

            return defaultPacCache[isWhiteList];
        }
        #endregion
    }
}
