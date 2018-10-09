using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class PACServer : Model.BaseClass.SingletonService<PACServer>
    {
        public event EventHandler OnPACServerStatusChanged;

        Model.Data.ProxyRegKeyValue orgSysProxySetting;
        Lib.Net.SimpleWebServer webServer = null;
        object webServerLock = new object();
        Setting setting;
        Dictionary<bool, string> pacCache = null;

        PACServer()
        {
            setting = Setting.Instance;
            orgSysProxySetting = Lib.ProxySetter.GetProxySetting();
            ClearCache();
            var pacSet = setting.GetPacServerSettings();
            var proxy = setting.GetSysProxySetting();
            Lib.ProxySetter.SetProxy(proxy);

            if (pacSet.isAutorun || !string.IsNullOrEmpty(proxy.autoConfigUrl))
            {
                RestartPacServer();
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

        #region public method
        public void SetPACProx(string ip, int port, bool isSocks, bool isWhiteList)
        {
            var proxy = new Model.Data.ProxyRegKeyValue();
            proxy.autoConfigUrl = GetPacUrl(isWhiteList, isSocks, ip, port);
            Lib.ProxySetter.SetProxy(proxy);
            setting.SaveSysProxySetting(proxy);
            StartPacServer(this, EventArgs.Empty);
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
                    webServer.Stop();
                }

                try
                {
                    webServer = new Lib.Net.SimpleWebServer(
                        SendResponse,
                        prefix,
                        "application/x-ns-proxy-autoconfig");

                    webServer.Run();
                    isWebServRunning = true;
                }
                catch
                {
                    Task.Factory.StartNew(
                        () => MessageBox.Show(I18N("StartPacServFail")));
                }
            }
            InvokeOnPACServerStatusChanged();
        }

        public void StopPacServer()
        {
            StopPacServer(this, EventArgs.Empty);
        }

        public void Cleanup()
        {
            StopPacServer(this, EventArgs.Empty);
            Lib.ProxySetter.SetProxy(orgSysProxySetting);
        }

        public string GetPacUrl(bool isWhiteList, bool isSocks, string ip, int port)
        {
            var pacSetting = setting.GetPacServerSettings();
            // e.g. http://localhost:3000/pac/?&port=5678&ip=1.2.3.4&proto=socks&type=whitelist&key=rnd
            return string.Format(
                "{0}?&type={1}&proto={2}&ip={3}&port={4}&timeout={5}",
                GenPrefix(pacSetting.port),
                isWhiteList ? "whitelist" : "blacklist",
                isSocks ? "socks" : "http",
                ip,
                port.ToString(),
                Lib.Utils.RandomHex(16));
        }
        #endregion

        #region private method
        void StopPacServer(object sender, EventArgs args)
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
            pacCache = new Dictionary<bool, string> {
                { true,null },
                { false,null },
            };
        }

        void StartPacServer(object sender, EventArgs args)
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

        string GenPrefix(int port)
        {
            return string.Format("http://localhost:{0}/pac/", port);
        }

        string SendResponse(HttpListenerRequest request)
        {
            // e.g. http://localhost:3000/pac/?&port=5678&ip=1.2.3.4&proto=socks&type=whitelist&key=rnd
            var query = request.QueryString;
            var isWhiteList = query["type"] == "whitelist";

            return string.Format(
                query["proto"] == "socks" ?
                "var proxy = 'SOCKS5 {0}:{1}; SOCKS {0}:{1}; DIRECT', mode='{2}', {3}" :
                "var proxy = 'PROXY {0}:{1}; DIRECT', mode='{2}', {3}",
                query["ip"] ?? "127.0.0.1",
                query["port"] ?? "1080",
                isWhiteList ? "white" : "black",
                GenPacBody(isWhiteList));
        }

        string GenPacBody(bool isWhiteList)
        {
            if (pacCache[isWhiteList] != null)
            {
                return pacCache[isWhiteList];
            }

            var urlList = Lib.Utils.GetPacUrlList(isWhiteList);
            var cidrList = Lib.Utils.GetPacCidrList(isWhiteList);
            var cidrSimList = Lib.Utils.CompactCidrList(ref cidrList);

            var pac = new StringBuilder("domains={");
            foreach (var url in urlList)
            {
                pac.Append("'")
                    .Append(url)
                    .Append("':1,");
            }
            pac.Length--;
            pac.Append(" },cidrs = [");
            foreach (var cidr in cidrSimList)
            {
                pac.Append("[")
                    .Append(cidr[0])
                    .Append(",")
                    .Append(cidr[1])
                    .Append("],");
            }
            pac.Length--;
            pac.Append(StrConst("PacTemplateTail"));

            pacCache[isWhiteList] = pac.ToString();
            return pacCache[isWhiteList];
        }
        #endregion
    }
}
