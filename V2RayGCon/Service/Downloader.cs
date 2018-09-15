using System;
using System.IO;
using System.Net;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Downloader
    {
        public event EventHandler OnDownloadCompleted, OnDownloadCancelled, OnDownloadFail;
        public event EventHandler<Model.Data.IntEvent> OnProgress;

        Setting setting;
        string _packageName;
        string _version;
        WebClient client;

        public Downloader()
        {
            setting = Setting.Instance;

            SetArchitecture(false);
            _version = StrConst("DefCoreVersion");
            client = null;
        }

        #region public method

        public void SetArchitecture(bool win64 = false)
        {
            _packageName = win64 ? StrConst("PkgWin64") : StrConst("PkgWin32");
        }

        public void SetVersion(string version)
        {
            _version = version;
        }

        public string GetPackageName()
        {
            return _packageName;
        }

        public void DownloadV2RayCore()
        {
            Download();
        }

        public bool UnzipPackage()
        {
            try
            {
                Lib.Utils.ZipFileDecompress(
                    GetLocalFilePath(),
                    Lib.Utils.GetAppDataFolder());
                return true;
            }
            catch { }
            return false;
        }

        public void Cancel()
        {
            client?.CancelAsync();
        }
        #endregion

        #region private method
        void SendProgress(int percentage)
        {
            try
            {
                OnProgress?.Invoke(this,
                    new Model.Data.IntEvent(Math.Max(1, percentage)));
            }
            catch { }
        }

        void NotifyDownloadResults(bool status)
        {
            try
            {
                if (status)
                {
                    OnDownloadCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    OnDownloadFail?.Invoke(this, EventArgs.Empty);
                }
            }
            catch { }
        }

        void UpdateCore()
        {
            var serverList = setting.GetServerListInstance();
            var servers = serverList.GetActiveServersList();

            serverList.StopAllServersThen(() =>
            {
                var status = UnzipPackage();
                NotifyDownloadResults(status);
                if (servers.Count > 0)
                {
                    serverList.RestartServersByList(servers);
                }
            });
        }

        void DownloadCompleted(bool cancelled)
        {
            client.Dispose();
            client = null;

            if (cancelled)
            {
                try
                {
                    OnDownloadCancelled?.Invoke(this, EventArgs.Empty);
                }
                catch { }
                return;
            }

            setting.SendLog(string.Format("{0}", I18N("DownloadCompleted")));

            UpdateCore();
        }

        string GetLocalFilePath()
        {
            var appData = Lib.Utils.GetAppDataFolder();
            return Path.Combine(appData, _packageName);
        }

        void Download()
        {
            string tpl = StrConst("DownloadLinkTpl");
            string url = string.Format(tpl, _version, _packageName);

            Lib.Utils.SupportProtocolTLS12();
            client = new WebClient();

            client.DownloadProgressChanged += (s, a) =>
            {
                SendProgress(a.ProgressPercentage);
            };

            client.DownloadFileCompleted += (s, a) =>
            {
                DownloadCompleted(a.Cancelled);
            };

            Lib.Utils.CreateAppDataFolder();
            setting.SendLog(string.Format("{0}:{1}", I18N("Download"), url));
            client.DownloadFileAsync(new Uri(url), GetLocalFilePath());
        }

        #endregion
    }
}
