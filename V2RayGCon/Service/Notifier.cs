using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Notifier : Model.BaseClass.SingletonService<Notifier>
    {
        NotifyIcon ni;
        Setting setting;
        static AutoResetEvent sayGoodbye = new AutoResetEvent(false);

        Notifier()
        {
            setting = Setting.Instance;
            CreateNotifyIcon();

            Application.ApplicationExit += (s, a) => Cleanup();
            Microsoft.Win32.SystemEvents.SessionEnding += (s, a) => Application.Exit();

            ni.MouseClick += (s, a) =>
            {
                if (a.Button == MouseButtons.Left)
                {
                    Views.FormMain.GetForm();
                }
            };

#if DEBUG
            This_function_do_some_tedious_stuff();
#else
            if (setting.GetServerCount() > 0)
            {
                setting.WakeupAutorunServer();
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

            // new Views.FormConfiger(0);
            // new Views.FormConfigTester();
            // Views.FormOption.GetForm();
            // Views.FormMain.GetForm();
            // Views.FormLog.GetForm();
            // setting.WakeupAutorunServer();
            // Views.FormSimAddVmessClient.GetForm();
            // Views.FormDownloadCore.GetForm();
        }
#endif
        #endregion

        #region private method

        void CreateNotifyIcon()
        {
            ni = new NotifyIcon();
            ni.Text = I18N("Description");
#if DEBUG
            ni.Icon = Properties.Resources.icon_light;
#else
            ni.Icon = Properties.Resources.icon_dark;
#endif
            ni.BalloonTipTitle = Properties.Resources.AppName;
            ni.ContextMenu = CreateMenu();
            ni.Visible = true;
        }

        ContextMenu CreateMenu()
        {
            return new ContextMenu(new MenuItem[] {

                new MenuItem(I18N("MainWin"),(s,a)=>Views.FormMain.GetForm()),

                new MenuItem(I18N("OtherWin"),new MenuItem[]{
                    new MenuItem(I18N("ConfigEditor"),(s,a)=>new Views.FormConfiger() ),
                    new MenuItem(I18N("ConfigTester"),(s,a)=>new Views.FormConfigTester() ),
                    new MenuItem(I18N("GenQRCode"),(s,a)=>Views.FormQRCode.GetForm() ),
                    new MenuItem(I18N("Log"),(s,a)=>Views.FormLog.GetForm() ),
                    new MenuItem(I18N("Options"),(s,a)=>Views.FormOption.GetForm() ),
                }),

                new MenuItem(I18N("ScanQRCode"),(s,a)=>{
                    void Success(string link)
                    {
                        var msg=Lib.Utils.CutStr(link,90);
                        setting.SendLog($"QRCode: {msg}");
                        setting.ImportLinks(link);
                    }

                    void Fail()
                    {
                        MessageBox.Show(I18N("NoQRCode"));
                    }

                    Lib.QRCode.QRCode.ScanQRCode(Success,Fail);
                }),

                new MenuItem(I18N("ImportLink"),(s,a)=>{
                    string links = Lib.Utils.GetClipboardText();
                    setting.ImportLinks(links);
                }),

                new MenuItem(I18N("DownloadV2rayCore"),(s,a)=>Views.FormDownloadCore.GetForm()),

                new MenuItem("-"),

                new MenuItem(I18N("Help"),(s,a)=>Lib.UI.VisitUrl(I18N("VistWikiPage"),Properties.Resources.WikiLink)),

                new MenuItem(I18N("Exit"),(s,a)=>{
                    if(Lib.UI.Confirm(I18N("ConfirmExitApp"))){
                        Application.Exit();
                    }
                })
            });
        }

        void Cleanup()
        {
            ni.Visible = false;
            setting.DisposeLazyTimers();
            setting.SaveServerList();

            if (!string.IsNullOrEmpty(setting.curSysProxy))
            {
                setting.ClearSysProxy();
            }

            setting.StopAllServersThen(() => sayGoodbye.Set());
            sayGoodbye.WaitOne();
        }
        #endregion
    }
}
