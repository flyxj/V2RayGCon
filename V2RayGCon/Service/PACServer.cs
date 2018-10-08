using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class PACServer : Model.BaseClass.SingletonService<PACServer>
    {
        Lib.Net.SimpleWebServer webServer = null;
        object webServerLock = new object();
        int port = 3000;
        Setting setting;

        PACServer()
        {
            setting = Setting.Instance;
            setting.OnRequirePACServerStart += StartPacServer;
            setting.OnRequirePACServerStop += StopPacServer;
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
        public void RestartPacServer()
        {
            var pacSetting = setting.GetPacServerSettings();
            this.port = pacSetting.port;

            var prefix = GenPrefix(port);
            lock (webServerLock)
            {
                if (isWebServRunning)
                {
                    webServer.Stop();
                }

                try
                {
                    webServer = new Lib.Net.SimpleWebServer(SendResponse, prefix);
                    webServer.Run();
                    isWebServRunning = true;
                }
                catch
                {
                    Task.Factory.StartNew(
                        () => MessageBox.Show(I18N("StartPacServFail")));
                }
            }
        }

        public string GetPacPrefix()
        {
            if (!isWebServRunning)
            {
                return null;
            }
            return GenPrefix(this.port);
        }

        public void Cleanup()
        {
            StopPacServer(this, EventArgs.Empty);
            setting.OnRequirePACServerStart -= StartPacServer;
            setting.OnRequirePACServerStop -= StopPacServer;
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
                    isWebServRunning = false;
                }
            }
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
            // e.g. http://localhost:3001/pac/?&port=5678&ip=1.2.3.4&mode=socks/http&type=whitelist/blacklist&key=rnd
            var query = request.QueryString;
            var ip = query["ip"] ?? "127.0.0.1";
            var port = query["port"] ?? "1080";
            var isSocks = query["proto"] == "socks";
            var listMode = query["type"] == "whitelist" ? "white" : "black";
            return GenPacFile(listMode, isSocks, ip, port);
        }

        string GenPacFile(string listMode, bool isSocks, string ip, string port)
        {
            var headTpl = isSocks ?
              "var proxy = 'SOCKS5 {0}:{1}; SOCKS {0}:{1}; DIRECT'" :
              "var proxy = 'PROXY {0}:{1}; DIRECT'";
            var head = string.Format(headTpl, ip, port);
            var mode = ",mode = '" + listMode + "',domains={";
            var cidrs = ": 1 },cidrs = [";
            var tail = StrConst("PacTemplateTail");

            return "hello";
        }
        #endregion
    }
}
