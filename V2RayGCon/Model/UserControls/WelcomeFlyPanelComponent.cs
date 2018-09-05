using System.Windows.Forms;

namespace V2RayGCon.Model.UserControls
{
    public partial class WelcomeFlyPanelComponent :
        UserControl,
        Model.BaseClass.IFormMainFlyPanelComponent
    {
        public WelcomeFlyPanelComponent()
        {
            InitializeComponent();
        }

        #region public method
        public void Cleanup()
        {
        }
        #endregion

        private void WelcomeFlyPanelComponent_Load(object sender, System.EventArgs e)
        {
            // this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            // this.Dock = DockStyle.;
        }
    }
}
