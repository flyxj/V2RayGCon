using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Download
    {
        public event EventHandler OnDownloadCompleted;
        Service.Setting setting;
        string packageName;

        public Download()
        {
            setting = Service.Setting.Instance;
            packageName = resData("PkgWin32");
        }

        public void SetPackageName(string name)
        {
            string[] packages = { resData("PkgWin32"), resData("PkgWin64") };
            packageName = packages.Contains(name) ? name : packages[0];
        }

        public void GetV2RayCore()
        {
            Task task = new Task(Downloader);
            task.Start();
        }

        void Downloader()
        {
            string ver = resData("Version");
            string fileName = packageName;
            string tpl = resData("DownloadLinkTpl");
            string url = string.Format(tpl, ver, fileName);

            Lib.Utils.SupportProtocolTLS12();
            WebClient client = new WebClient();

            int preProgress = -100;
            client.DownloadProgressChanged += (s, a) =>
            {
                var percentage = a.ProgressPercentage;
                if (percentage - preProgress >= 20)
                {
                    preProgress = percentage;
                    setting.SendLog(string.Format("{0}: {1}%", I18N("DLProgress"), percentage));
                }
            };

            client.DownloadFileCompleted += (s, a) =>
            {
                setting.SendLog(string.Format("{0}", I18N("DLComplete")));
                OnDownloadCompleted?.Invoke(this, EventArgs.Empty);
            };

            setting.SendLog(string.Format("{0}:{1}", I18N("Download"), url));
            client.DownloadFileAsync(new Uri(url), fileName);
        }
    }
}
