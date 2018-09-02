using System;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormMain : Form
    {
        #region Sigleton
        static FormMain _instant;
        public static FormMain GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormMain();
            }
            return _instant;
        }
        #endregion

        Controller.FormMainCtrl formMainCtrl;
        Service.Setting setting;

        FormMain()
        {
            setting = Service.Setting.Instance;

            InitializeComponent();

#if DEBUG
            this.Icon = Properties.Resources.icon_light;
#endif
            this.Show();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {

            this.FormClosed += (s, a) =>
            {
                setting.OnSysProxyChanged -= OnSysProxyChangedHandler;
                formMainCtrl.Cleanup();
            };

            Lib.UI.SetFormLocation<FormMain>(this, Model.Data.Enum.FormLocations.TopLeft);

            this.Text = string.Format(
                "{0} v{1}",
                Properties.Resources.AppName,
                Properties.Resources.Version);

            formMainCtrl = InitFormMainCtrl();

            toolMenuItemCurrentSysProxy.Text = GetCurrentSysProxySetting();
            setting.OnSysProxyChanged += OnSysProxyChangedHandler;
        }

        #region private method
        private Controller.FormMainCtrl InitFormMainCtrl()
        {
            var ctrl = new Controller.FormMainCtrl();

            ctrl.Plug(new Controller.FormMainComponent.FlyServer(
                flyServerListContainer));

            ctrl.Plug(new Controller.FormMainComponent.MenuItems(
                toolMenuItemSimAddVmessServer,
                toolMenuItemImportLinkFromClipboard,
                toolMenuItemExportAllServerToFile,
                toolMenuItemImportFromFile,
                toolMenuItemCheckUpdate,
                toolMenuItemAbout,
                toolMenuItemHelp,
                toolMenuItemConfigEditor,
                toolMenuItemConfigTester,
                toolMenuItemQRCode,
                toolMenuItemLog,
                toolMenuItemOptions,
                toolMenuItemDownloadV2rayCore,
                toolMenuItemRemoveV2rayCore,
                toolMenuItemDeleteAll,
                toolStripMenuItemStopAllServers,
                toolStripMenuItemRestartAllServers,
                toolMenuItemClearSysProxy));

            return ctrl;
        }

        void OnSysProxyChangedHandler(object sender, EventArgs args)
        {
            menuStrip1.Invoke((MethodInvoker)delegate
            {
                toolMenuItemCurrentSysProxy.Text = GetCurrentSysProxySetting();
            });
        }

        string GetCurrentSysProxySetting()
        {
            var s = I18N("CurSysProxy");
            if (string.IsNullOrEmpty(setting.curSysProxy))
            {
                s = string.Format("{0}:{1}", s, I18N("NotSet"));
            }
            else
            {
                s = string.Format("{0} http://{1}", s, setting.curSysProxy);
            }
            return s;
        }
        #endregion

        #region UI event handler
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
