using System;
using System.Net;

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

                webServer = new Lib.Net.SimpleWebServer(SendResponse, prefix);
                webServer.Run();
                isWebServRunning = true;
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
            return string.Format("http://localhost:{0}/", port);
        }

        string SendResponse(HttpListenerRequest request)
        {
            return string.Format(
                "<HTML><BODY>My web page.<br>{0}</BODY></HTML>",
                DateTime.Now);
        }
        #endregion
    }
}
