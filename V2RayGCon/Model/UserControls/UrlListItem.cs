using System;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.UserControls
{
    public partial class UrlListItem : UserControl
    {
        Action OnDeleted;

        public UrlListItem(Model.Data.UrlItem subItem, Action OnDeleted)
        {
            InitializeComponent();

            lbIndex.Text = "";
            tboxUrl.Text = subItem.url;
            tboxAlias.Text = subItem.alias;
            cboxInUse.Checked = subItem.inUse;

            this.OnDeleted = OnDeleted;
        }

        public Model.Data.UrlItem GetValue()
        {
            return new Model.Data.UrlItem
            {
                inUse = cboxInUse.Checked,
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
            var form = this.FindForm() as Views.FormOption;
            flyPanel.Controls.Remove(this);

            this.OnDeleted?.Invoke();
        }
        #endregion

        #region private method
        private void UrlListItem_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Lib.UI.CreateCursorIconFromUserControl(this);
            DoDragDrop((UrlListItem)sender, DragDropEffects.Move);
        }
        #endregion
    }
}
