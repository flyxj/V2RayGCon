using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    class PacServer : Model.BaseClass.SingletonService<PacServer>
    {
        public event EventHandler OnPACServerStatusChanged;

        Model.Data.ProxyRegKeyValue orgSysProxySetting;
        Lib.Net.SimpleWebServer webServer = null;
        object webServerLock = new object();
        Setting setting;
        Dictionary<bool, string[]> pacCache = null;

        PacServer()
        {
            setting = Setting.Instance;
            orgSysProxySetting = Lib.ProxySetter.GetProxySetting();
            ClearCache();
            var pacSetting = setting.GetPacServerSettings();
            var proxySetting = setting.GetSysProxySetting();

            if (pacSetting.isAutorun || !string.IsNullOrEmpty(proxySetting.autoConfigUrl))
            {
                RestartPacServer();
            }

            // becareful issue #9 #3
            if (!Lib.Utils.IsProxySettingEmpty(proxySetting))
            {
                Lib.ProxySetter.SetProxy(proxySetting);
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

        Lib.CancelableTimeout lazySysProxyUpdaterTimer = null;
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

            lazySysProxyUpdaterTimer = new Lib.CancelableTimeout(setProxy, 1000);
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
            Lib.ProxySetter.SetProxy(proxy);
            setting.SaveSysProxySetting(proxy);
            StartPacServer();
            InvokeOnPACServerStatusChanged();
        }

        public void SetGlobalProxy(string ip, int port)
        {
            var proxy = new Model.Data.ProxyRegKeyValue();
            proxy.proxyEnable = true;
            proxy.proxyServer = string.Format("{0}:{1}", ip, port.ToString());
            Lib.ProxySetter.SetProxy(proxy);
            setting.SaveSysProxySetting(proxy);
            InvokeOnPACServerStatusChanged();
        }

        public void ClearSysProxy()
        {
            var proxy = new Model.Data.ProxyRegKeyValue();
            setting.SaveSysProxySetting(proxy);
            Lib.ProxySetter.SetProxy(proxy);
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
                    webServer = new Lib.Net.SimpleWebServer(WebRequestDispatcher, prefix);
                    webServer.Run();
                    isWebServRunning = true;
                }
                catch
                {
                    Task.Factory.StartNew(
                        () => MessageBox.Show(I18N.StartPacServFail));
                }
            }
            InvokeOnPACServerStatusChanged();
        }

        public void Cleanup()
        {
            StopPacServer();
            Lib.ProxySetter.SetProxy(orgSysProxySetting);
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
                param.ip,
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
            }
            InvokeOnPACServerStatusChanged();
        }
        #endregion

        #region private method
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
            pacCache = new Dictionary<bool, string[]> {
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
            var pacUrl = GenPacUrl(urlParam) + "&mime=js";
            var tpl = StrConst.PacDebuggerTpl;
            var html = tpl.Replace("__PacServerUrl__", pacUrl);
            var mime = "text/html; charset=utf-8";
            return new Tuple<string, string>(html, mime);
        }

        private Tuple<string, string> ResponseWithPacFile(Model.Data.PacUrlParams urlParam)
        {
            // ie require this header
            var mime = "application/x-ns-proxy-autoconfig";

            var proxy = urlParam.isSocks ?
                "SOCKS5 {0}:{1}; SOCKS {0}:{1}; DIRECT" :
                "PROXY {0}:{1}; DIRECT";
            var mode = urlParam.isWhiteList ? "white" : "black";
            var domainAndCidrs = GenDomainAndCidrContent(urlParam.isWhiteList);

            var content = StrConst.PacJsTpl
                .Replace("__PROXY__", string.Format(proxy, urlParam.ip, urlParam.port))
                .Replace("__MODE__", mode)
                .Replace("__DOMAINS__", domainAndCidrs[0])
                .Replace("__CIDRS__", domainAndCidrs[1]);

            return new Tuple<string, string>(content, mime);
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
        string[] GenDomainAndCidrContent(bool isWhiteList)
        {
            if (pacCache[isWhiteList] != null)
            {
                return pacCache[isWhiteList];
            }

            var domainList = Lib.Utils.GetPacDomainList(isWhiteList);
            var cidrList = Lib.Utils.GetPacCidrList(isWhiteList);

            // merge user settings
            var customSetting = setting.GetPacServerSettings();
            var customUrlList = isWhiteList ? customSetting.customWhiteList : customSetting.customBlackList;
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

            pacCache[isWhiteList] = new string[] {
                domainSB.ToString(),
                cidrSB.ToString(),
            };

            return pacCache[isWhiteList];
        }
        #endregion
    }
}
