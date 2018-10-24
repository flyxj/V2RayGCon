using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace V2RayGCon.Lib.Nets
{
    // 由v2rayP的代码魔改而成
    // https://raw.githubusercontent.com/PoseidonM4A4/v2rayP/master/v2rayP/Manager/PacManager.cs
    class SimpleWebServer
    {
        Thread ThreadPacServer = null;
        HttpListener PacListener = null;

        private readonly Func<HttpListenerRequest, Tuple<string, string>> GenResponse;
        string prefix;

        public SimpleWebServer(
            Func<HttpListenerRequest, Tuple<string, string>> genResponsMethod,
            string prefix)
        {
            this.GenResponse = genResponsMethod;
            this.prefix = prefix;

        }

        #region public method
        public void Start()
        {
            Stop();

            PacListener = new HttpListener()
            {
                Prefixes = { prefix }
            };

            ThreadPacServer = new Thread(new ThreadStart(PacServerThreadWorker));
            ThreadPacServer.Start();
        }

        public void Stop()
        {
            if (PacListener != null)
            {
                PacListener.Abort();
                PacListener = null;
            }

            if (ThreadPacServer != null)
            {
                ThreadPacServer.Abort();
                ThreadPacServer = null;
            }
        }
        #endregion

        #region private method
        private void PacServerThreadWorker()
        {
            PacListener.Start();

            try
            {
                while (true)
                {
                    var context = PacListener.GetContext();
                    var request = context.Request;
                    var response = context.Response;

                    // tuple<content,mime>
                    var result = GenResponse(request);

                    var mime = result.Item2;
                    if (!string.IsNullOrEmpty(mime))
                    {
                        response.ContentType = mime;
                    }
                    var html = result.Item1;
                    response.ContentLength64 = Encoding.UTF8.GetByteCount(html);
                    using (var writer = new StreamWriter(response.OutputStream))
                    {
                        writer.Write(html);
                    }
                }
            }
            catch (HttpListenerException ex)
            {
                // listener is stopped 
                if (ex.NativeErrorCode == 995) return;
                else throw;
            }
        }
        #endregion
    }
}
