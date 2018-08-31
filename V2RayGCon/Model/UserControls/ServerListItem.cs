using System.Windows.Forms;

namespace V2RayGCon.Model.UserControls
{
    public partial class ServerListItem : UserControl
    {
        public ServerListItem()
        {
            InitializeComponent();
        }

        private void ServerListItem_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Lib.UI.CreateCursorIconFromUserControl(this);
            DoDragDrop((ServerListItem)sender, DragDropEffects.Move);
        }
    }
}
