using System;
using System.Net;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Downloader
    {
        public event EventHandler OnDownloadCompleted, OnDownloadCancelled;
        public event EventHandler<Model.Data.IntEvent> OnProgress;
        Service.Setting setting;
        string _packageName;
        string _version;
        WebClient client;

        public Downloader()
        {
            setting = Service.Setting.Instance;
            SelectArchitecture(false);
            _version = resData("DefCoreVersion");
            client = null;
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

        public void UnzipPackage()
        {
            Lib.Utils.ZipFileDecompress(_packageName);
        }

        public void Cancel()
        {
            if (client != null)
            {
                client.CancelAsync();
            }
        }
        #endregion

        #region private method
        void Download()
        {
            string fileName = _packageName;
            string tpl = resData("DownloadLinkTpl");
            string url = string.Format(tpl, _version, fileName);

            Lib.Utils.SupportProtocolTLS12();
            client = new WebClient();

            int preProgress = -100;

            client.DownloadProgressChanged += (s, a) =>
            {
                var percentage = a.ProgressPercentage;

                if (percentage >= 1)
                {
                    var e = new Model.Data.IntEvent(a.ProgressPercentage);
                    try
                    {
                        OnProgress?.Invoke(this, e);
                    }
                    catch { }
                }

                if (percentage - preProgress >= 20)
                {
                    preProgress = percentage;
                    setting.SendLog(string.Format("{0}: {1}%", I18N("DownloadProgress"), percentage));
                }
            };

            client.DownloadFileCompleted += (s, a) =>
            {
                if (a.Cancelled)
                {
                    OnDownloadCancelled?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    setting.SendLog(string.Format("{0}", I18N("DownloadCompleted")));
                    OnDownloadCompleted?.Invoke(this, EventArgs.Empty);
                }
                client.Dispose();
                client = null;
            };

            setting.SendLog(string.Format("{0}:{1}", I18N("Download"), url));
            client.DownloadFileAsync(new Uri(url), fileName);

        }

        #endregion
    }
}
