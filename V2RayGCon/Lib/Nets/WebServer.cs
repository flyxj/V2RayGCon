using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace V2RayGCon.Lib.Nets
{
    // 由v2rayP的代码魔改而成
    // https://raw.githubusercontent.com/PoseidonM4A4/v2rayP/master/v2rayP/Manager/PacManager.cs
    class WebServer
    {
        public event EventHandler OnWebServerStateChange;

        Thread ThreadPacServer = null;
        HttpListener WebListener = null;

        Func<HttpListenerRequest, Tuple<string, string>> ResponseGenerator;

        public WebServer(
            Func<HttpListenerRequest, Tuple<string, string>>
                genResponsMethod)
        {
            this.ResponseGenerator = genResponsMethod
                ?? throw new ArgumentException("Response method must not null.");
        }

        #region properties
        bool _isRunning = false;
        public bool isRunning
        {
            get { return _isRunning; }
            private set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    try
                    {
                        OnWebServerStateChange?.Invoke(this, EventArgs.Empty);
                    }
                    catch { }
                }
            }
        }
        #endregion

        #region public method
        public void Start(string prefix)
        {
            Stop();
            WebListener = new HttpListener()
            {
                Prefixes = { prefix }
            };

            try
            {
                WebListener.Start();
            }
            catch
            {
                WebListener = null;
                throw;
            }

            ThreadPacServer = new Thread(new ThreadStart(WebRequestWorker));
            ThreadPacServer.Start();
            isRunning = true;
        }

        public void Stop()
        {
            if (ThreadPacServer != null)
            {
                try
                {
                    ThreadPacServer.Abort();
                }
                catch { }
                ThreadPacServer = null;
            }

            if (WebListener != null)
            {
                try
                {
                    WebListener.Abort();
                }
                catch { }
                WebListener = null;
            }

            isRunning = false;
        }

        #endregion

        #region private method
        void RequestHandler(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string html = "error";
            string mime = null;

            try
            {
                var result = ResponseGenerator(request);
                html = result.Item1;
                mime = result.Item2;
            }
            catch { }

            ResponseWith(response, html, mime);
        }

        void ResponseWith(HttpListenerResponse response, string html, string mime)
        {
            if (!string.IsNullOrEmpty(mime))
            {
                response.ContentType = mime;
            }

            try
            {
                response.ContentLength64 = Encoding.UTF8.GetByteCount(html ?? "");

                using (var writer = new StreamWriter(response.OutputStream))
                {
                    writer.Write(html);
                }
            }
            catch { }
        }

        private void WebRequestWorker()
        {
            try
            {
                while (true)
                {
                    var context = WebListener.GetContext();
                    RequestHandler(context);
                }
            }
            catch
            {
                isRunning = false;
            }
        }
        #endregion
    }
}
