using System.Diagnostics;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Notifier : Model.BaseClass.SingletonService<Notifier>
    {
        NotifyIcon ni;
        Core core;
        Setting setting;
        Download downloader = null;

        Notifier()
        {
            setting = Setting.Instance;
            core = Core.Instance;
            CreateNotifyIcon();

            Application.ApplicationExit += (s, a) => Cleanup();
            core.OnCoreStatChange += (s, a) => UpdateNotifyText();

#if DEBUG
            EnableDebug();
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
                Debug.WriteLine("Test JObject:");
                Debug.WriteLine("Done!");
            };

            ContextMenu ctxm = ni.ContextMenu;

            bool debug_form_config = false;
            // bool debug_form_config = true;

            if (debug_form_config)
            {
                setting.curEditingIndex = 0;
                new Views.FormConfiger();
            }
            else
            {
                Views.FormMain.GetForm();
                Views.FormLog.GetForm();
            }
        }

        void EnableDebug()
        {
            var mis = ni.ContextMenu.MenuItems;
            mis.Add(0, new MenuItem("-"));
            mis.Add(0, CreateDebugMenu());

            This_function_do_some_tedious_stuff();
        }

        MenuItem CreateDebugMenu()
        {
            return new MenuItem("Debug", new MenuItem[]{

                new MenuItem("Add dummy server1",(s,a)=>{
                    setting.ImportLinks(resData("DummyServ1"));
                    Debug.WriteLine("Done!");
                }),

                new MenuItem("Add dummy server2",(s,a)=>{
                    setting.ImportLinks(resData("DummyServ2"));
                    Debug.WriteLine("Done!");
                }),

                new MenuItem("Show Form in background",(s,a)=>{

                    Debug.WriteLine("Done!");
                }),
            });
        }

#endif
        #endregion

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

            ni.Text = core.IsRunning() ? title : I18N("Description");
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

        void DownloadCore(bool win64 = false)
        {
            if (downloader != null)
            {
                MessageBox.Show(I18N("Downloading"));
                return;
            }

            string coreName = win64 ? resData("PkgWin64") : resData("PkgWin32");

            downloader = new Download();
            downloader.SetPackageName(coreName);

            downloader.OnDownloadCompleted += (dls, dla) =>
            {
                string msg = I18N("DLComplete");
                try
                {
                    Lib.Utils.ZipFileDecompress(coreName);
                }
                catch
                {
                    msg = I18N("DLFail");
                }

                downloader = null;
                MessageBox.Show(msg);
            };

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
                        var msg=Lib.Utils.CutStr(link,48);
                        setting.SendLog("QRCode: " + msg);

                        if (setting.ImportLinks(link))
                        {
                            MessageBox.Show(I18N("ImportLinkSuccess"));
                        }
                        else
                        {
                            MessageBox.Show(I18N("NotSupportLinkType"));
                        }
                    }

                    void Fail()
                    {
                        MessageBox.Show(I18N("NoQRCode"));
                    }

                    Lib.QRCode.QRCode.ScanQRCode(Success,Fail);
                }),

                new MenuItem(I18N("ImportLink"),(s,a)=>{
                    string links = Lib.Utils.GetClipboardText();
                    Lib.UI.ShowMsgboxSuccFail(
                        setting.ImportLinks(links),
                        I18N("ImportLinkSuccess"),
                        I18N("ImportLinkFail"));

                }),

                new MenuItem("-"),

                new MenuItem(I18N("CopyPacUrl"),(s,a)=>{
                    if (Lib.Utils.CopyToClipboard(setting.GetPacUrl()))
                    {
                        MessageBox.Show(
                            I18N("WarnIENotSupportPac"),
                            I18N("CopySuccess"));
                    } else{
                        MessageBox.Show(I18N("CopyFail"));
                    }
                }),

                new MenuItem(I18N("DLv2rayCore"),new MenuItem[]{
                    new MenuItem(I18N("Win32"),(s,a)=>{
                        DownloadCore(false);
                    }),
                    new MenuItem(I18N("Win64"),(s,a)=>{
                        DownloadCore(true);
                    }),
                }),

                new MenuItem("-"),

                new MenuItem(I18N("About"),(s,a)=>MessageBox.Show(
                    Properties.Resources.ProjectLink
                    )),

                new MenuItem(I18N("Exit"),(s,a)=>{
                    if(Lib.UI.Confirm(I18N("ConfirmExitApp"))){
                        Application.Exit();
                    }
                })
            });
        }

        private void Cleanup()
        {
            Debug.WriteLine("Call cleanup");
            ni.Visible = false;
            core.StopCore();
            Lib.ProxySetter.setProxy("", false);
        }
    }
}
