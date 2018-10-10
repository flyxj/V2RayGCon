using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace V2RayGCon.Lib.Net
{
    // https://codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server

    /* usage
     class Program
    {
        static void Main(string[] args)
        {
            WebServer ws = new WebServer(SendResponse, "http://localhost:8080/test/");
            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }
 
        public static string SendResponse(HttpListenerRequest request)
        {
            return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);    
        }
    }
    */

    public class SimpleWebServer
    {
        private readonly HttpListener _listener = new HttpListener();

        // tuple<string content, string mimeType>
        private readonly Func<HttpListenerRequest, Tuple<string, string>> _responderMethod;

        public SimpleWebServer(Func<HttpListenerRequest, Tuple<string, string>> method, string prefix)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException("prefix");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            _listener.Prefixes.Add(prefix);
            Debug.WriteLine("Prefix:" + prefix);
            _responderMethod = method;
            _listener.Start();
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Debug.WriteLine("Webserver running...");

                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                // tuple<content,mime>
                                Tuple<string, string> rsp = _responderMethod(ctx.Request);
                                byte[] buf = Encoding.UTF8.GetBytes(rsp.Item1);
                                if (!string.IsNullOrEmpty(rsp.Item2))
                                {
                                    ctx.Response.ContentType = rsp.Item2;
                                }
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch { } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
