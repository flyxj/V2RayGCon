using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormDownloadCore : Form
    {
        #region Sigleton
        static FormDownloadCore _instant;
        public static FormDownloadCore GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormDownloadCore();
            }
            return _instant;
        }
        #endregion

        Service.Core core;
        Service.Setting setting;
        Service.Downloader downloader;

        FormDownloadCore()
        {
            InitializeComponent();
            InitUI();
            setting = Service.Setting.Instance;
            core = Service.Core.Instance;

            this.FormClosed += (s, e) =>
            {
                if (downloader != null)
                {
                    downloader.Cancel();
                }
            };

            this.Show();

        }

        void FillComboBox(ComboBox element, List<string> itemList)
        {
            element.Items.Clear();

            if (itemList == null || itemList.Count <= 0)
            {
                element.SelectedIndex = -1;
                return;
            }

            foreach (var item in itemList)
            {
                element.Items.Add(item);
            }
            element.SelectedIndex = 0;
        }

        void InitUI()
        {
            cboxArch.SelectedIndex = 0;
            var verList = Lib.Utils.Str2ListStr(resData("VerList"));
            FillComboBox(cboxVer, verList);
            pgBarDownload.Value = 0;
        }

        delegate void UpdateVersionListDelegate(List<string> versions);

        void VersionListReciever(List<string> versions)
        {
            UpdateVersionListDelegate updater = new UpdateVersionListDelegate(UpdateVersionList);
            try
            {
                cboxVer?.Invoke(updater, versions);
            }
            catch { }
        }

        void UpdateVersionList(List<string> versions)
        {
            btnRefreshVer.Enabled = true;
            if (versions.Count > 0)
            {
                FillComboBox(cboxVer, versions);
            }
            else
            {
                MessageBox.Show(I18N("GetVersionListFail"));
            }
        }

        delegate void UpdateProgressDelegate(int percentage);

        void UpdateProgressBarReciever(int percentage)
        {
            UpdateProgressDelegate updater = new UpdateProgressDelegate(UpdateProgressBar);
            try
            {
                pgBarDownload?.Invoke(updater, percentage);
            }
            catch { }
        }

        void UpdateProgressBar(int percentage)
        {
            pgBarDownload.Value = Lib.Utils.Clamp(percentage, 0, 101);
        }

        private void btnRefreshVer_Click(object sender, System.EventArgs e)
        {
            btnRefreshVer.Enabled = false;
            Task.Factory.StartNew(() =>
            {
                var versions = Lib.Utils.GetCoreVersions();
                VersionListReciever(versions);
            });
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            if (downloader != null)
            {
                MessageBox.Show(I18N("Downloading"));
                return;
            }
            downloader = new Service.Downloader();
            downloader.SelectArchitecture(cboxArch.SelectedIndex == 1);
            downloader.SetVersion(cboxVer.Text);

            string packageName = downloader.GetPackageName();

            downloader.OnProgress += (s, a) =>
                UpdateProgressBarReciever(a.Data);

            downloader.OnDownloadCancelled += (s, a) =>
            {
                downloader = null;
                UpdateProgressBarReciever(0);
                MessageBox.Show(I18N("DownloadCancelled"));
            };

            downloader.OnDownloadCompleted += (s, a) =>
            {
                Debug.WriteLine("Download completed!");
                string msg = I18N("DownloadCompleted");
                try
                {
                    var isRunning = core.isRunning;
                    if (isRunning)
                    {
                        core.StopCore();
                    }
                    downloader.UnzipPackage();
                    if (isRunning)
                    {
                        setting.ActivateServer();
                    }
                }
                catch
                {
                    msg = I18N("DownloadFail");
                }
                MessageBox.Show(msg);
                downloader = null;
            };

            downloader.GetV2RayCore();

            UpdateProgressBar(1);
        }

        void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (downloader != null && Lib.UI.Confirm(I18N("CancelDownload")))
            {
                downloader.Cancel();
            }
        }
    }
}
