using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{

    class Download
    {
        public event EventHandler OnDownloadCompleted;
        
        string packageName;

        public Download()
        {
            packageName = resData("PkgWin32");
        }

        public void SetPackageName(string name)
        {
            string[] packages = { resData("PkgWin32"), resData("PkgWin64") };
            packageName = packages.Contains(name) ? name : packages[0];
        }
       
        public void GetV2RayCore()
        {
            var downloader = new System.Threading.Thread(Downloader);
            downloader.IsBackground = true;
            downloader.Start();
        }

        void Downloader()
        {
            string ver = resData("Version");
            string fileName = packageName;
            string tpl = resData("DownloadLinkTpl");
            string url = string.Format(tpl, ver, fileName);

            Lib.Utils.SupportProtocolTLS12();

            WebClient client = new WebClient();

            client.DownloadFileCompleted += (s, a) =>
            {
                OnDownloadCompleted?.Invoke(this,null);                
            };

            Debug.WriteLine("Download: " + url);
            client.DownloadFileAsync(new Uri(url), fileName);
        }
    }
}
