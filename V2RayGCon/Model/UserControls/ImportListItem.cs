using System;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.UserControls
{
    public partial class ImportListItem : UserControl
    {
        Action OnDeleted;

        public ImportListItem(Model.Data.ImportItem subItem, Action OnDeleted)
        {
            InitializeComponent();

            lbIndex.Text = "";
            tboxUrl.Text = subItem.url;
            tboxAlias.Text = subItem.alias;
            chkMergeWhenStart.Checked = subItem.isUseOnActivate;
            chkMergeWhenSpeedTest.Checked = subItem.isUseOnSpeedTest;

            this.OnDeleted = OnDeleted;
        }

        public Model.Data.ImportItem GetValue()
        {
            return new Model.Data.ImportItem
            {
                isUseOnActivate = chkMergeWhenStart.Checked,
                isUseOnSpeedTest = chkMergeWhenSpeedTest.Checked,
                alias = tboxAlias.Text,
                url = tboxUrl.Text,
            };
        }

        #region public method
        public void SetIndex(int index)
        {
            lbIndex.Text = index.ToString();
        }
        #endregion

        #region UI event
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!Lib.UI.Confirm(I18N("ConfirmDeleteControl")))
            {
                return;
            }

            var flyPanel = this.Parent as FlowLayoutPanel;
            flyPanel.Controls.Remove(this);

            this.OnDeleted?.Invoke();
        }
        #endregion

        #region private method
        private void UrlListItem_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Lib.UI.CreateCursorIconFromUserControl(this);
            DoDragDrop((ImportListItem)sender, DragDropEffects.Move);
        }
        #endregion
    }
}
