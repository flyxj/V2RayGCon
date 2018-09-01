using System.Drawing;
using System.Windows.Forms;

namespace V2RayGCon.Controller.FormMainComponent
{
    class FlyServer : FormMainComponentController
    {
        FlowLayoutPanel flyPanel;
        Service.Setting setting;
        public FlyServer(
            FlowLayoutPanel panel)
        {
            this.setting = Service.Setting.Instance;
            this.flyPanel = panel;
            BindEvent();
            LoadData();
        }

        #region public method
        public override void Cleanup()
        {
            foreach (Model.UserControls.ServerListItem control in flyPanel.Controls)
            {
                control.Cleanup();
            }
        }

        public override bool RefreshUI()
        {
            flyPanel.Invoke((MethodInvoker)delegate
            {
                Cleanup();
                flyPanel.Controls.Clear();
                LoadData();
            });
            return false;
        }
        #endregion

        #region private method
        private void LoadData()
        {
            var serverList = setting.GetServerList();

            for (int i = 0; i < serverList.Count; i++)
            {
                flyPanel.Controls.Add(
                    new Model.UserControls.ServerListItem(
                        i + 1, serverList[i]));
            }
        }

        private void ResetIndex()
        {
            var controls = flyPanel.Controls;
            for (int i = 0; i < controls.Count; i++)
            {
                var control = controls[i] as Model.UserControls.ServerListItem;
                control.SetIndex(i + 1);
            }
        }

        private void BindEvent()
        {
            flyPanel.DragEnter += (s, a) =>
            {
                a.Effect = DragDropEffects.Move;
            };

            flyPanel.DragDrop += (s, a) =>
            {
                // https://www.codeproject.com/Articles/48411/Using-the-FlowLayoutPanel-and-Reordering-with-Drag

                var data = a.Data.GetData(typeof(Model.UserControls.ServerListItem))
                    as Model.UserControls.ServerListItem;

                var _destination = s as FlowLayoutPanel;
                Point p = _destination.PointToClient(new Point(a.X, a.Y));
                var item = _destination.GetChildAtPoint(p);
                int index = _destination.Controls.GetChildIndex(item, false);
                _destination.Controls.SetChildIndex(data, index);
                _destination.Invalidate();
                ResetIndex();
            };
        }
        #endregion
    }
}
