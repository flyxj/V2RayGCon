using System;
using System.Reflection;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    class Notifier : Model.BaseClass.SingletonService<Notifier>
    {
        NotifyIcon ni;
        Setting setting;
        Servers servers;

        Notifier() { }

        public void Run(Setting setting, Servers servers)
        {
            this.setting = setting;
            this.servers = servers;

            CreateNotifyIcon();
            servers.OnRequireNotifierUpdate += UpdateNotifierTextHandler;

            ni.MouseClick += (s, a) =>
            {
                if (a.Button == MouseButtons.Left)
                {
                    Views.WinForms.FormMain.GetForm();
                }
            };
        }

        #region public method
#if DEBUG
        public void InjectDebugMenuItem(ToolStripMenuItem menu)
        {
            ni.ContextMenuStrip.Items.Insert(0, new ToolStripSeparator());
            ni.ContextMenuStrip.Items.Insert(0, menu);
        }
#endif

        public void Cleanup()
        {
            ni.Visible = false;
            servers.OnRequireNotifierUpdate -= UpdateNotifierTextHandler;
        }
        #endregion

        #region private method
        void UpdateNotifierTextHandler(object sender, Model.Data.StrEvent args)
        {
            // https://stackoverflow.com/questions/579665/how-can-i-show-a-systray-tooltip-longer-than-63-chars
            var text = Lib.Utils.CutStr(args.Data, 127);
            if (ni.Text == text)
            {
                return;
            }

            Type t = typeof(NotifyIcon);
            BindingFlags hidden = BindingFlags.NonPublic | BindingFlags.Instance;
            t.GetField("text", hidden).SetValue(ni, text);
            if ((bool)t.GetField("added", hidden).GetValue(ni))
                t.GetMethod("UpdateIcon", hidden).Invoke(ni, new object[] { true });
        }

        void CreateNotifyIcon()
        {
            ni = new NotifyIcon
            {
                Text = I18N.Description,
#if DEBUG
                Icon = Properties.Resources.icon_light,
#else
                Icon = Properties.Resources.icon_dark,
#endif
                BalloonTipTitle = Properties.Resources.AppName,

                ContextMenuStrip = CreateMenu(),
                Visible = true
            };
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
        #endregion
    }
}
