using System;
using System.Net;
using System.Threading.Tasks;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Downloader
    {
        public event EventHandler OnDownloadCompleted;
        Service.Setting setting;
        string _packageName;
        string _version;

        public Downloader()
        {
            setting = Service.Setting.Instance;
            SelectArchitecture(false);
            _version = resData("Version");
        }

        #region public method
        public void SelectArchitecture(bool win64 = false)
        {
            _packageName = win64 ? resData("PkgWin64") : resData("PkgWin32");
        }

        public void SetVersion(string version)
        {
            _version = version;
        }

        public string GetPackageName()
        {
            return _packageName;
        }

        public void GetV2RayCore()
        {
            Download();
        }
        #endregion

        #region private method
        void Download()
        {
            string fileName = _packageName;
            string tpl = resData("DownloadLinkTpl");
            string url = string.Format(tpl, _version, fileName);

            Lib.Utils.SupportProtocolTLS12();
            using (WebClient client = new WebClient())
            {
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
        #endregion
    }
}
