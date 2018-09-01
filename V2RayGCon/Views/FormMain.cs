using System;
using System.Windows.Forms;

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

        Service.Setting setting;
        Controller.FormMainCtrl formMainCtrl;

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
                setting.OnRequireMenuUpdate -= UpdateMenuHandler;
                formMainCtrl.Cleanup();
            };

            Lib.UI.SetFormLocation<FormMain>(this, Model.Data.Enum.FormLocations.TopLeft);

            this.Text = string.Format(
                "{0} v{1}",
                Properties.Resources.AppName,
                Properties.Resources.Version);

            formMainCtrl = InitFormMainCtrl();

            setting.OnRequireMenuUpdate += UpdateMenuHandler;
        }

        #region private method
        private Controller.FormMainCtrl InitFormMainCtrl()
        {
            var ctrl = new Controller.FormMainCtrl();

            ctrl.Plug(new Controller.FormMainComponent.FlyServer(
                flyServerListContainer));

            ctrl.Plug(new Controller.FormMainComponent.Menus(
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
                toolMenuItemRemoveV2rayCore
                ));

            return ctrl;
        }

        void UpdateMenuHandler(object s, EventArgs e)
        {
            formMainCtrl.RefreshUI();
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
