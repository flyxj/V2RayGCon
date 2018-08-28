using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

            this.Show();
        }

        private void FormOption_Shown(object sender, System.EventArgs e)
        {
            this.optionCtrl = InitOptionCtrl();
        }


        #region public method
       
        #endregion

        #region private method
        private Controller.OptionCtrl InitOptionCtrl()
        {
            var ctrl = new Controller.OptionCtrl();

            ctrl.Plug(new Controller.OptionComponent.Subscription(
                flySubsUrlContainer,
                btnAddSubsUrl,
                btnSaveSubsUrl,
                btnUpdateViaSubscription));

            return ctrl;
        }

        #endregion
    }
}
