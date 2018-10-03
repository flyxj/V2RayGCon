using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Notifier : Model.BaseClass.SingletonService<Notifier>
    {
        NotifyIcon ni;
        Setting setting;
        Servers servers;

        Notifier()
        {
            setting = Setting.Instance;
            setting.SwitchCulture();

            CreateNotifyIcon();
            Lib.Utils.SupportProtocolTLS12();
            setting.SaveOriginalSystemProxyInfo();
            setting.LoadSystemProxy();
            setting.OnUpdateNotifierText += UpdateNotifierTextHandler;

            servers = Servers.Instance;
            servers.Prepare(setting);

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
            if (servers.IsEmpty())
            {
                Views.FormMain.GetForm();
            }
            else
            {
                servers.WakeupAutorunServersThen();
            }
#endif
        }



        #region DEBUG code TL;DR
#if DEBUG
        void This_function_do_some_tedious_stuff()
        {

            // Some test code
            ni.ContextMenu.MenuItems.Add(0, new MenuItem("-"));
            ni.ContextMenu.MenuItems.Add(0, new MenuItem("Debug", (_s, _a) =>
            {
                servers.DbgFastRestartTest(100);
            }));

            // new Views.FormConfiger(@"{}");
            // new Views.FormConfigTester();
            // Views.FormOption.GetForm();
            Views.FormMain.GetForm();
            Views.FormLog.GetForm();
            // setting.WakeupAutorunServer();
            // Views.FormSimAddVmessClient.GetForm();
            // Views.FormDownloadCore.GetForm();
        }
#endif
        #endregion

        #region public method
        public string GetLogCache()
        {
            return setting.logCache;
        }
        #endregion

        #region private method
        void UpdateNotifierTextHandler(object sender, Model.Data.StrEvent args)
        {
            // https://stackoverflow.com/questions/579665/how-can-i-show-a-systray-tooltip-longer-than-63-chars
            var text = Lib.Utils.CutStr(args.Data, 127);
            Type t = typeof(NotifyIcon);
            BindingFlags hidden = BindingFlags.NonPublic | BindingFlags.Instance;
            t.GetField("text", hidden).SetValue(ni, text);
            if ((bool)t.GetField("added", hidden).GetValue(ni))
                t.GetMethod("UpdateIcon", hidden).Invoke(ni, new object[] { true });
        }

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
                    new MenuItem(I18N("GenQRCode"),(s,a)=>Views.FormQRCode.GetForm() ),
                    new MenuItem(I18N("Log"),(s,a)=>Views.FormLog.GetForm() ),
                    new MenuItem(I18N("Options"),(s,a)=>Views.FormOption.GetForm() ),
                }),

                new MenuItem(I18N("ScanQRCode"),(s,a)=>{
                    void Success(string link)
                    {
                        var msg=Lib.Utils.CutStr(link,90);
                        setting.SendLog($"QRCode: {msg}");
                        servers.ImportLinks(link);
                    }

                    void Fail()
                    {
                        MessageBox.Show(I18N("NoQRCode"));
                    }

                    Lib.QRCode.QRCode.ScanQRCode(Success,Fail);
                }),

                new MenuItem(I18N("ImportLink"),(s,a)=>{
                    string links = Lib.Utils.GetClipboardText();
                    servers.ImportLinks(links);
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

            servers.SaveServerListImmediately();
            servers.DisposeLazyTimers();
            setting.RestoreOriginalSystemProxyInfo();

            AutoResetEvent sayGoodbye = new AutoResetEvent(false);
            servers.StopAllServersThen(() => sayGoodbye.Set());
            sayGoodbye.WaitOne();
        }
        #endregion
    }
}
