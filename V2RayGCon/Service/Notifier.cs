using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    class Notifier : Model.BaseClass.SingletonService<Notifier>
    {
        NotifyIcon ni;
        Setting setting;
        Servers servers;
        PACServer pacServer;

        Notifier()
        {
            setting = Setting.Instance;
            setting.SwitchCulture();
            servers = Servers.Instance;
            pacServer = Service.PACServer.Instance;

            CreateNotifyIcon();
            Lib.Utils.SupportProtocolTLS12();

            setting.OnUpdateNotifierText += UpdateNotifierTextHandler;
            servers.Prepare(setting);

            Application.ApplicationExit += (s, a) => Cleanup();
            Microsoft.Win32.SystemEvents.SessionEnding += (s, a) => Application.Exit();

            ni.MouseClick += (s, a) =>
            {
                if (a.Button == MouseButtons.Left)
                {
                    Views.WinForms.FormMain.GetForm();
                }
            };

#if DEBUG
            This_function_do_some_tedious_stuff();
#endif
            if (servers.IsEmpty())
            {
                Views.WinForms.FormMain.GetForm();
            }
            else
            {
                servers.WakeupAutorunServersThen(RestoreTracker);
            }
        }

        #region DEBUG code TL;DR
#if DEBUG
        void This_function_do_some_tedious_stuff()
        {

            // Some test code
            ni.ContextMenuStrip.Items.Insert(0, new ToolStripSeparator());
            ni.ContextMenuStrip.Items.Insert(0, new ToolStripMenuItem(
                "Debug", null, (_s, _a) =>
             {
                 servers.DbgFastRestartTest(100);
             }));

            // new Views.WinForms.FormConfiger(@"{}");
            // new Views.WinForms.FormConfigTester();
            // Views.WinForms.FormOption.GetForm();
            Views.WinForms.FormMain.GetForm();
            Views.WinForms.FormLog.GetForm();
            // setting.WakeupAutorunServer();
            // Views.WinForms.FormSimAddVmessClient.GetForm();
            // Views.WinForms.FormDownloadCore.GetForm();
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
        private void RestoreTracker()
        {
            var trackerSetting = setting.GetServerTrackerSetting();

            // wake up track server
            if (!trackerSetting.isTrackerOn)
            {
                return;
            }

            var trackerList = trackerSetting.serverList;
            if (trackerList.Count < 1)
            {
                setting.isServerTrackerOn = true;
                return;
            }

            Action done = () =>
            {
                var trackedServerList = servers.GetServerList()
                    .Where(s => s.config == trackerList[0])
                    .ToList();

                setting.isServerTrackerOn = true;
                if (trackedServerList.Any())
                {
                    servers.RestartServersByListThen(trackedServerList);
                }
            };

            if (trackerList.Count > 1)
            {
                var list = servers.GetServerList()
                .Where(s =>
                    trackerList.Contains(s.config)
                    && !s.isServerOn
                    && s.config != trackerSetting.serverList[0]
                ).ToList();
                servers.RestartServersByListThen(list, done);
            }
            else
            {
                done();
            }
        }

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
            ni.Text = I18N.Description;
#if DEBUG
            ni.Icon = Properties.Resources.icon_light;
#else
            ni.Icon = Properties.Resources.icon_dark;
#endif
            ni.BalloonTipTitle = Properties.Resources.AppName;

            ni.ContextMenuStrip = CreateMenu();
            ni.Visible = true;
        }

        ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var factor = Lib.UI.GetScreenScalingFactor();
            if (factor > 1)
            {
                menu.ImageScalingSize = new System.Drawing.Size(
                    (int)(menu.ImageScalingSize.Width * factor),
                    (int)(menu.ImageScalingSize.Height * factor));
            }

            menu.Items.AddRange(new ToolStripMenuItem[] {
                new ToolStripMenuItem(
                    I18N.MainWin,
                    Properties.Resources.WindowsForm_16x,
                    (s,a)=>Views.WinForms.FormMain.GetForm()),

                new ToolStripMenuItem(
                    I18N.OtherWin,
                    Properties.Resources.CPPWin32Project_16x,
                    new ToolStripMenuItem[]{
                        new ToolStripMenuItem(
                            I18N.ConfigEditor,
                            Properties.Resources.EditWindow_16x,
                            (s,a)=>new Views.WinForms.FormConfiger() ),
                        new ToolStripMenuItem(
                            I18N.GenQRCode,
                            Properties.Resources.AzureVirtualMachineExtension_16x,
                            (s,a)=>Views.WinForms.FormQRCode.GetForm()),
                        new ToolStripMenuItem(
                            I18N.Log,
                            Properties.Resources.FSInteractiveWindow_16x,
                            (s,a)=>Views.WinForms.FormLog.GetForm() ),
                        new ToolStripMenuItem(
                            I18N.Options,
                            Properties.Resources.Settings_16x,
                            (s,a)=>Views.WinForms.FormOption.GetForm() ),
                    }),

                new ToolStripMenuItem(
                    I18N.ScanQRCode,
                    Properties.Resources.ExpandScope_16x,
                    (s,a)=>{
                        void Success(string link)
                        {
                            var msg=Lib.Utils.CutStr(link,90);
                            setting.SendLog($"QRCode: {msg}");
                            servers.ImportLinks(link);
                        }

                        void Fail()
                        {
                            MessageBox.Show(I18N.NoQRCode);
                        }

                        Lib.QRCode.QRCode.ScanQRCode(Success,Fail);
                    }),

                new ToolStripMenuItem(
                    I18N.ImportLink,
                    Properties.Resources.CopyLongTextToClipboard_16x,
                    (s,a)=>{
                        string links = Lib.Utils.GetClipboardText();
                        servers.ImportLinks(links);
                    }),

                new ToolStripMenuItem(
                    I18N.DownloadV2rayCore,
                    Properties.Resources.ASX_TransferDownload_blue_16x,
                    (s,a)=>Views.WinForms.FormDownloadCore.GetForm()),
            });

            menu.Items.Add(new ToolStripSeparator());

            menu.Items.AddRange(new ToolStripMenuItem[] {
                new ToolStripMenuItem(
                    I18N.Help,
                    Properties.Resources.StatusHelp_16x,
                    (s,a)=>Lib.UI.VisitUrl(I18N.VistWikiPage,Properties.Resources.WikiLink)),

                new ToolStripMenuItem(
                    I18N.Exit,
                    Properties.Resources.CloseSolution_16x,
                    (s,a)=>{
                        if (Lib.UI.Confirm(I18N.ConfirmExitApp)){
                            Application.Exit();
                        }
                    })
            });

            return menu;
        }

        void Cleanup()
        {
            ni.Visible = false;

            servers.SaveServerListImmediately();
            servers.DisposeLazyTimers();
            pacServer.Cleanup();
            AutoResetEvent sayGoodbye = new AutoResetEvent(false);
            servers.StopAllServersThen(() => sayGoodbye.Set());
            sayGoodbye.WaitOne();
        }
        #endregion
    }
}
