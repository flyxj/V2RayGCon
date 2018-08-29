using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormOption : Form
    {
        #region Sigleton
        static FormOption _instant;
        public static FormOption GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormOption();
            }
            return _instant;
        }
        #endregion

        Service.Setting setting;
        Controller.OptionCtrl optionCtrl;

        FormOption()
        {
            this.setting = Service.Setting.Instance;

            InitializeComponent();

#if DEBUG
            this.Icon = Properties.Resources.icon_light;
#endif
            this.Show();
        }

        private void FormOption_Shown(object sender, System.EventArgs e)
        {
            this.optionCtrl = InitOptionCtrl();

            this.FormClosing += (s, a) =>
            {
                if (!this.optionCtrl.IsOptionsSaved())
                {
                    a.Cancel = !Lib.UI.Confirm(I18N("ConfirmCloseWinWithoutSave"));
                }
            };

            // this.FormClosed += (s, a) => { };
        }


        #region public method

        #endregion

        #region private method
        private Controller.OptionCtrl InitOptionCtrl()
        {
            var ctrl = new Controller.OptionCtrl();

            ctrl.Plug(
                new Controller.OptionComponent.Import(
                    flyImportPanel,
                    btnImportAdd));

            ctrl.Plug(
                new Controller.OptionComponent.Subscription(
                    flySubsUrlContainer,
                    btnAddSubsUrl,
                    btnUpdateViaSubscription));

            return ctrl;
        }

        #endregion

        #region UI event
        private void btnOptionExit_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnOptionSave_Click(object sender, System.EventArgs e)
        {
            this.optionCtrl.SaveAllOptions();
            MessageBox.Show(I18N("Done"));
        }

        private void btnBakBackup_Click(object sender, System.EventArgs e)
        {
            optionCtrl.BackupOptions();
        }

        private void btnBakRestore_Click(object sender, System.EventArgs e)
        {
            optionCtrl.RestoreOptions();
        }
        #endregion
    }
}
