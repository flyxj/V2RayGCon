using System;
using System.Windows.Forms;

namespace V2RayGCon.Model.UserControls
{
    public partial class UrlListItem : UserControl
    {
        public UrlListItem(Model.Data.SubscribeItem subItem)
        {
            InitializeComponent();

            lbIndex.Text = "";
            tboxUrl.Text = subItem.url;
            tboxAlias.Text = subItem.alias;
            cboxInUse.Checked = subItem.inUse;
        }

        public Model.Data.SubscribeItem GetValue()
        {
            return new Model.Data.SubscribeItem
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

        #region private method
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var flyPanel = this.Parent as FlowLayoutPanel;
            var form = this.FindForm() as Views.FormOption;
            flyPanel.Controls.Remove(this);
            form.UpdateFlySubUrlItemIndex();
        }

        private void UrlListItem_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((UrlListItem)sender, DragDropEffects.Copy);
        }
        #endregion

    }
}
