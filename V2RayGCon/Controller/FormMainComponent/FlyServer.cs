using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            BindDragDropEvent();
            LoadServerItems();
            setting.OnRequireFlyPanelUpdate += OnRequireFlyPanelUpdateHandler;
        }

        #region public method
        public override void Cleanup()
        {
            setting.OnRequireFlyPanelUpdate -= OnRequireFlyPanelUpdateHandler;
            RemoveAllConrols();
        }

        public override bool RefreshUI()
        {
            flyPanel.Invoke((MethodInvoker)delegate
            {
                RemoveAllConrols();
                LoadServerItems();
            });
            return false;
        }
        #endregion

        #region private method

        void RemoveAllConrols()
        {
            List<UserControl> listControls = new List<UserControl>();

            foreach (Model.BaseClass.IFormMainFlyPanelComponent control in flyPanel.Controls)
            {
                control.Cleanup();
                if (control is Model.UserControls.ServerListItem)
                {
                    listControls.Add(control as Model.UserControls.ServerListItem);
                }
                if (control is Model.UserControls.WelcomeFlyPanelComponent)
                {
                    listControls.Add(control as Model.UserControls.WelcomeFlyPanelComponent);
                }
            }

            foreach (UserControl control in listControls)
            {
                flyPanel.Controls.Remove(control);
                control.Dispose();
            }
        }

        void OnRequireFlyPanelUpdateHandler(object sender, EventArgs args)
        {
            RefreshUI();
        }

        private void LoadWelcomeItem()
        {
            flyPanel.Controls.Add(
                new Model.UserControls.WelcomeFlyPanelComponent());
        }
        private void LoadServerItems()
        {
            var serverList =
                setting.GetServerList()
                .OrderBy(o => o.index)
                .ToList();

            if (serverList.Count <= 0)
            {
                LoadWelcomeItem();
                return;
            }

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

        private void BindDragDropEvent()
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

                if (data == null)
                {
                    return;
                }

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
