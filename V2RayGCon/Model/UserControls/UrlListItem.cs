using System;
using System.Drawing;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.UserControls
{
    public partial class UrlListItem : UserControl
    {
        Action OnDeleted;

        public UrlListItem(Model.Data.SubscriptionItem subItem, Action OnDeleted)
        {
            InitializeComponent();

            lbIndex.Text = "";
            tboxUrl.Text = subItem.url;
            tboxAlias.Text = subItem.alias;
            cboxInUse.Checked = subItem.inUse;

            this.OnDeleted = OnDeleted;
        }

        public Model.Data.SubscriptionItem GetValue()
        {
            return new Model.Data.SubscriptionItem
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


            Bitmap bmp = new Bitmap(this.Size.Width, this.Size.Height);
            this.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
            //optionally define a transparent color
            //bmp.MakeTransparent(Color.White);

            Cursor cur = new Cursor(bmp.GetHicon());
            Cursor.Current = cur;

            DoDragDrop((UrlListItem)sender, DragDropEffects.Move);
        }
        #endregion
    }
}
