using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    public class PacServer : Model.BaseClass.SingletonService<PacServer>
    {
        Setting setting;
        public event EventHandler OnPACServerStateChanged;

        Lib.Nets.WebServer webServer = null;
        object webServerLock = new object();

        string customPacFileCache = string.Empty;
        FileSystemWatcher customPacFileWatcher = null;

        Model.Data.PacUrlParams curUrlParam = null;

        PacServer()
        {
            webServer = new Lib.Nets.WebServer(WebRequestDispatcher);
            webServer.OnWebServerStateChange += OnWebServerStateChangeHandler;
        }

        public void Run(Setting setting)
        {
            this.setting = setting;

            ClearDefaultPacCache();
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
        public bool IsPacServerRunning()
        {
            return webServer.isRunning;
        }

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
                    var p = Lib.Utils.GetProxyParamsFromUrl(proxySetting.autoConfigUrl)
                        ?? new Model.Data.PacUrlParams();

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
                if (IsPacServerRunning())
                {
                    return;
                }
                RestartPacServer();
            }
        }

        public void SetPACProx(Model.Data.PacUrlParams param)
        {
            curUrlParam = param;
            var proxy = new Model.Data.ProxyRegKeyValue();
            proxy.autoConfigUrl = GenPacUrl(param);
            Lib.Sys.ProxySetter.SetProxy(proxy);
            setting.SaveSysProxySetting(proxy);
            StartPacServer();
            LazyInvokeOnPacServerStateChange();
        }

        public void SetGlobalProxy(string ip, int port)
        {
            curUrlParam = null;
            var proxy = new Model.Data.ProxyRegKeyValue();
            proxy.proxyEnable = true;
            proxy.proxyServer = string.Format(
                "{0}:{1}",
                ip == "0.0.0.0" ? "127.0.0.1" : ip,
                port.ToString());
            Lib.Sys.ProxySetter.SetProxy(proxy);
            setting.SaveSysProxySetting(proxy);
            LazyInvokeOnPacServerStateChange();
        }

        public void ClearSysProxy()
        {
            var proxy = new Model.Data.ProxyRegKeyValue();
            setting.SaveSysProxySetting(proxy);
            Lib.Sys.ProxySetter.SetProxy(proxy);
            LazyInvokeOnPacServerStateChange();
        }

        public void RestartPacServer()
        {
            var pacSetting = setting.GetPacServerSettings();
            var prefix = GenPrefix(pacSetting.port);

            lock (webServerLock)
            {
                if (IsPacServerRunning())
                {
                    StopPacServer();
                }

                try
                {
                    webServer.Start(prefix);
                }
                catch
                {
                    Task.Factory.StartNew(
                        () => MessageBox.Show(I18N.StartPacServFail));
                    return;
                }

                StartFileWatcher(pacSetting);
            }
            LazyInvokeOnPacServerStateChange();
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
            customPacFileCache = string.Empty;
            if (File.Exists(filename))
            {
                try
                {
                    var content = File.ReadAllText(filename);
                    customPacFileCache = content ?? string.Empty;
                }
                catch { }
            }
            // Debug.WriteLine("content: " + customPacCache);
        }

        public void Cleanup()
        {
            webServer.OnWebServerStateChange -= OnWebServerStateChangeHandler;
            StopPacServer();
            lazyStateChangeTimer?.Release();
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
                if (IsPacServerRunning())
                {
                    webServer.Stop();
                    ClearDefaultPacCache();
                }
                StopCustomPacFileWatcher();
            }
            curUrlParam = null;
            LazyInvokeOnPacServerStateChange();
        }
        #endregion

        #region private method
        void OnWebServerStateChangeHandler(object sender, EventArgs args)
        {
            LazyInvokeOnPacServerStateChange();
        }

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

        Lib.Sys.CancelableTimeout lazyStateChangeTimer = null;
        void LazyInvokeOnPacServerStateChange()
        {
            // Create on demand.
            if (lazyStateChangeTimer == null)
            {
                lazyStateChangeTimer = new Lib.Sys.CancelableTimeout(
                    () =>
                    {
                        try
                        {
                            OnPACServerStateChanged?.Invoke(this, EventArgs.Empty);
                        }
                        catch { }
                    },
                    1000);
            }
            lazyStateChangeTimer.Start();
        }

        void ClearDefaultPacCache()
        {
            Lib.Nets.PacGenerator.ClearCache();
        }

        string GenPrefix(int port)
        {
            return string.Format("http://localhost:{0}/pac/", port);
        }

        Tuple<string, string> WebRequestDispatcher(HttpListenerRequest request)
        {
            // e.g. http://localhost:3000/pac/?&port=5678&ip=1.2.3.4&proto=socks&type=whitelist&key=rnd
            var urlParam =
                Lib.Utils.GetProxyParamsFromUrl(request.Url.AbsoluteUri)
                ?? curUrlParam;  // fall back

            if (urlParam == null)
            {
                return new Tuple<string, string>("Bad request.", null);
            }

            var pacSetting = setting.GetPacServerSettings();
            var pacFileResponse =
                Lib.Nets.PacGenerator.GenPacFileResponse(
                    urlParam, pacSetting, customPacFileCache);

            return urlParam.isDebug ?
                GenPacDebuggerResponse(GenPacUrl(urlParam), pacFileResponse.Item1) :
                pacFileResponse;
        }

        Tuple<string, string> GenPacDebuggerResponse(
            string url, string pacContent)
        {
            var tpl = StrConst.PacDebuggerTpl;
            var html = tpl.Replace("__PacServerUrl__", url)
                .Replace("__PACFileContent__", pacContent);
            var mime = "text/html; charset=UTF-8";
            return new Tuple<string, string>(html, mime);
        }
        #endregion
    }
}
