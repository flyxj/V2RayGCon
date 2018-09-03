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

        Service.Downloader downloader;

        FormDownloadCore()
        {
            InitializeComponent();
            InitUI();

            this.FormClosed += (s, e) =>
            {
                downloader?.Cancel();
                Service.Setting.Instance.LazyGC();
            };

#if DEBUG
            this.Icon = Properties.Resources.icon_light;
#endif

            this.Show();
        }

        private void FormDownloadCore_Shown(object sender, System.EventArgs e)
        {
            RefreshCurrentCoreVersion();
        }

        #region private method

        void RefreshCurrentCoreVersion()
        {
            var el = labelCoreVersion;

            Task.Factory.StartNew(() =>
            {
                var core = new Model.BaseClass.CoreServer();
                var version = core.GetCoreVersion();
                var msg = string.IsNullOrEmpty(version) ?
                    I18N("GetCoreVerFail") :
                    string.Format(I18N("CurrentCoreVerIs"), version);
                try
                {
                    el.Invoke((MethodInvoker)delegate { el.Text = msg; });
                }
                catch { }
            });
        }

        void UpdateProgressBar(int percentage)
        {
            // window may closed before this function is called
            try
            {
                pgBarDownload.Invoke((MethodInvoker)delegate
                {
                    pgBarDownload.Value = Lib.Utils.Clamp(percentage, 0, 101);
                });
            }
            catch { }
        }

        void EnableBtnDownload()
        {
            try
            {
                btnDownload.Invoke((MethodInvoker)delegate
                {
                    btnDownload.Enabled = true;
                });
            }
            catch { }
        }

        void DownloadV2RayCore()
        {
            downloader = new Service.Downloader();
            downloader.SetArchitecture(cboxArch.SelectedIndex == 1);
            downloader.SetVersion(cboxVer.Text);

            downloader.OnProgress += (s, a) =>
            {
                UpdateProgressBar(a.Data);
            };

            downloader.OnDownloadCompleted += (s, a) =>
            {
                ResetUI(100);
                MessageBox.Show(I18N("DownloadCompleted"));
            };

            downloader.OnDownloadCancelled += (s, a) =>
            {
                ResetUI(0);
                MessageBox.Show(I18N("DownloadCancelled"));
            };

            downloader.OnDownloadFail += (s, a) =>
            {
                ResetUI(0);
                MessageBox.Show(I18N("TryManualDownload"));
            };

            downloader.DownloadV2RayCore();
            UpdateProgressBar(1);
        }

        #endregion

        #region UI
        void ResetUI(int progress)
        {
            UpdateProgressBar(progress);
            downloader = null;
            EnableBtnDownload();
        }

        void InitUI()
        {
            cboxArch.SelectedIndex = 0;
            var verList = Lib.Utils.Str2ListStr(StrConst("VerList"));
            Lib.UI.FillComboBox(cboxVer, verList);
            pgBarDownload.Value = 0;
        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnRefreshVer_Click(object sender, System.EventArgs e)
        {
            var elRefresh = btnRefreshVer;
            var elCboxVer = cboxVer;

            elRefresh.Enabled = false;

            Task.Factory.StartNew(() =>
            {
                var versions = Lib.Utils.GetCoreVersions();
                try
                {
                    elRefresh.Invoke((MethodInvoker)delegate
                    {
                        elRefresh.Enabled = true;
                    });

                    elCboxVer.Invoke((MethodInvoker)delegate
                    {
                        if (versions.Count > 0)
                        {
                            Lib.UI.FillComboBox(elCboxVer, versions);
                        }
                        else
                        {
                            MessageBox.Show(I18N("GetVersionListFail"));
                        }
                    });
                }
                catch { }
            });
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {
            if (downloader != null)
            {
                MessageBox.Show(I18N("Downloading"));
                return;
            }

            btnDownload.Enabled = false;
            DownloadV2RayCore();
        }

        void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (downloader != null && Lib.UI.Confirm(I18N("CancelDownload")))
            {
                downloader?.Cancel();
            }
        }

        private void btnCheckVersion_Click(object sender, System.EventArgs e)
        {
            RefreshCurrentCoreVersion();
        }

        #endregion
    }
}
