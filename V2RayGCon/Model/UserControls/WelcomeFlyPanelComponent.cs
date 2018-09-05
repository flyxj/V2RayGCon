using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.UserControls
{
    public partial class WelcomeFlyPanelComponent :
        UserControl,
        Model.BaseClass.IFormMainFlyPanelComponent
    {
        Service.Setting setting;
        public WelcomeFlyPanelComponent()
        {
            setting = Service.Setting.Instance;
            InitializeComponent();
        }

        #region public method
        public void Cleanup()
        {
        }
        #endregion

        private void WelcomeFlyPanelComponent_Load(object sender, System.EventArgs e)
        {
        }

        private void lbDownloadV2rayCore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Views.FormDownloadCore.GetForm();
        }

        private void lbV2rayCoreGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Lib.UI.VisitUrl(I18N("VisitV2rayCoreReleasePage"), StrConst("ReleasePageUrl"));
        }

        private void lbWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Lib.UI.VisitUrl(I18N("VistWikiPage"), Properties.Resources.WikiLink);
        }

        private void lbIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Lib.UI.VisitUrl(I18N("VisitVGCIssuePage"), Properties.Resources.IssueLink);
        }

        private void lbCopyFromClipboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string links = Lib.Utils.GetClipboardText();
            setting.ImportLinks(links);
        }

        private void lbScanQRCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            void Success(string link)
            {
                var msg = Lib.Utils.CutStr(link, 90);
                setting.SendLog($"QRCode: {msg}");
                setting.ImportLinks(link);
            }

            void Fail()
            {
                MessageBox.Show(I18N("NoQRCode"));
            }

            Lib.QRCode.QRCode.ScanQRCode(Success, Fail);
        }

        private void lbSimAddVmessWin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Views.FormSimAddVmessClient.GetForm();
        }

        private void lbConfigEditor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Views.FormConfiger();
        }
    }
}
