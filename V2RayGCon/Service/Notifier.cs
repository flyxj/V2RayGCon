using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Notifier : Model.BaseClass.SingletonService<Notifier>
    {
        NotifyIcon ni;
        Core core;
        Setting setting;
        Downloader downloader = null;

        Notifier()
        {
            setting = Setting.Instance;
            core = Core.Instance;
            CreateNotifyIcon();

            Application.ApplicationExit += (s, a) => Cleanup();
            core.OnCoreStatChange += (s, a) => UpdateNotifyText();

#if DEBUG
            This_function_do_some_tedious_stuff();
#else
            ni.MouseClick += (s, a) =>
            {
                if (a.Button == MouseButtons.Left)
                {
                    Views.FormMain.GetForm();
                }
            };

            if (setting.GetServerCount() > 0)
            {
                setting.ActivateServer();
            }
            else
            {
                Views.FormMain.GetForm();
            }
#endif
        }

        #region DEBUG code TL;DR
#if DEBUG
        void This_function_do_some_tedious_stuff()
        {
            ni.DoubleClick += (s, a) =>
            {
                Debug.WriteLine("Some test code:");
            };

            bool debug_form_config = false;
            // bool debug_form_config = true;

            if (debug_form_config)
            {
                new Views.FormConfiger(0);
            }
            else
            {
                Views.FormMain.GetForm();
                Views.FormLog.GetForm();
            }
        }
#endif
        #endregion

        #region private method
        void UpdateNotifyText()
        {
            var type = setting.proxyType;
            var protocol = Model.Data.Table.proxyTypesString[type];

            var proxy = string.Empty;
            if (type == (int)Model.Data.Enum.ProxyTypes.config)
            {
                proxy = setting.GetInbountInfo();
            }
            else
            {
                proxy = string.Format("{0}://{1}", protocol, setting.proxyAddr);
            }

            var title = string.Format("{0} {1}", setting.GetCurAlias(), proxy);

            ni.Text = core.isRunning ? title : I18N("Description");
        }

        void CreateNotifyIcon()
        {
            ni = new NotifyIcon();
            ni.Text = I18N("Description");
            ni.Icon = Properties.Resources.icon_dark;
            ni.BalloonTipTitle = I18N("AppName");
            ni.ContextMenu = CreateMenu();
            ni.Visible = true;
        }

        bool CheckVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                MessageBox.Show(I18N("GetLatestVerFail"));
                return false;
            }

            if (version.Equals(resData("Version")))
            {
                if (!Lib.UI.Confirm(string.Format(I18N("DLCurVerTpl"), version)))
                {
                    return false;
                }
            }
            else
            {
                if (!Lib.UI.Confirm(string.Format(I18N("DLNewVerTpl"), version)))
                {
                    return false;
                }
            }

            return true;
        }

        void DownloadCore(bool latest, bool win64)
        {
            if (downloader != null)
            {
                MessageBox.Show(I18N("Downloading"));
                return;
            }

            var version = resData("Version");


            if (latest)
            {
                var latestVersion = Lib.Utils.GetLatestVersion();
                if (!CheckVersion(latestVersion))
                {
                    return;
                }
                version = latestVersion;
            }

            downloader = new Downloader();
            downloader.SelectArchitecture(win64);
            downloader.SetVersion(version);

            string packageName = downloader.GetPackageName();

            downloader.OnDownloadCompleted += (s, a) => UpdateCore(packageName);

            downloader.GetV2RayCore();
        }

        ContextMenu CreateMenu()
        {
            return new ContextMenu(new MenuItem[] {

                new MenuItem(I18N("ShowMainWin"),(s,a)=>Views.FormMain.GetForm()),

                new MenuItem(I18N("ShowLogWin"),(s,a)=>Views.FormLog.GetForm()),

                new MenuItem(I18N("ScanQRCode"),(s,a)=>{
                    void Success(string link)
                    {
                        var msg=Lib.Utils.CutStr(link,90);
                        setting.SendLog($"QRCode: {msg}");
                        MessageBox.Show(
                            setting.ImportLinks(link)?
                            I18N("ImportLinkSuccess"):
                            I18N("NotSupportLinkType"));
                    }

                    void Fail()
                    {
                        MessageBox.Show(I18N("NoQRCode"));
                    }

                    Lib.QRCode.QRCode.ScanQRCode(Success,Fail);
                }),

                new MenuItem(I18N("ImportLink"),(s,a)=>{
                    string links = Lib.Utils.GetClipboardText();
                    MessageBox.Show(
                        setting.ImportLinks(links)?
                        I18N("ImportLinkSuccess"):
                        I18N("ImportLinkFail"));

                }),

                new MenuItem(I18N("DLv2rayCore"),new MenuItem[]{

                    GenComment(I18N("Latest")),

                    new MenuItem(I18N("Win32"),(s,a)=>{
                        Task.Factory.StartNew(()=> DownloadCore(true,false));
                    }),

                    new MenuItem(I18N("Win64"),(s,a)=>{
                        Task.Factory.StartNew(()=> DownloadCore(true,true));
                    }),

                    new MenuItem("-"),

                    GenComment(resData("Version")),

                    new MenuItem(I18N("Win32"),(s,a)=>{
                        Task.Factory.StartNew(()=> DownloadCore(false,false));
                    }),

                    new MenuItem(I18N("Win64"),(s,a)=>{
                        Task.Factory.StartNew(()=> DownloadCore(false,true));
                    }),
                }),

                new MenuItem("-"),

                new MenuItem(I18N("About"),(s,a)=>MessageBox.Show(
                    Properties.Resources.ProjectLink)),

                new MenuItem(I18N("Exit"),(s,a)=>{
                    if(Lib.UI.Confirm(I18N("ConfirmExitApp"))){
                        Application.Exit();
                    }
                })
            });
        }

        MenuItem GenComment(string comment)
        {
            var item = new MenuItem(comment);
            item.Enabled = false;
            return item;
        }

        void UpdateCore(string packageName)
        {
            string msg = I18N("DLComplete");
            try
            {
                var isRunning = core.isRunning;
                if (isRunning)
                {
                    core.StopCore();
                }
                Lib.Utils.ZipFileDecompress(packageName);
                if (isRunning)
                {
                    setting.ActivateServer();
                }
            }
            catch
            {
                msg = I18N("DLFail");
            }

            downloader = null;
            MessageBox.Show(msg);
        }

        void Cleanup()
        {
            Debug.WriteLine("Call cleanup");
            ni.Visible = false;
            core.StopCore();
            Lib.ProxySetter.setProxy("", false);
        }
        #endregion
    }
}
