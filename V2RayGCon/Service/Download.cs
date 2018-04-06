using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace V2RayGCon.Service
{

    class Download
    {
        public event EventHandler OnDownloadCompleted;
        Func<string, string> resData;
        string pkgName;

        public Download()
        {
            resData = Lib.Utils.resData;
            pkgName = resData("PkgWin32");
        }

        public void SetPackageName(string name)
        {
            string[] packages = { resData("PkgWin32"), resData("PkgWin64") };
            pkgName = packages.Contains(name) ? name : packages[0];
        }
       
        public void GetV2RayWindowsPackage()
        {
            var worker = new System.Threading.Thread(DownloadPackage);
            worker.IsBackground = true;
            worker.Start();
        }

        void DownloadPackage()
        {
            string ver = resData("Version");
            string fileName = pkgName;
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
