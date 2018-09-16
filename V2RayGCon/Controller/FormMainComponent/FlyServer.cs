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
        Service.Servers servers;

        public FlyServer(FlowLayoutPanel panel)
        {
            this.servers = Service.Servers.Instance;

            this.flyPanel = panel;
            BindDragDropEvent();
            RefreshUI();
            servers.OnRequireFlyPanelUpdate += OnRequireFlyPanelUpdateHandler;
        }

        #region public method
        public void SelectAutorun()
        {
            LoopThroughAllServer((s) => s.SetSelected(s.GetAutorunStatus()));
        }

        public void SelectAll()
        {
            LoopThroughAllServer((s) => s.SetSelected(true));
        }

        public void SelectNone()
        {
            LoopThroughAllServer((s) => s.SetSelected(false));
        }

        public void SelectInvert()
        {
            LoopThroughAllServer((s) => s.SetSelected(!s.GetSelectStatus()));
        }

        public override void Cleanup()
        {
            servers.OnRequireFlyPanelUpdate -= OnRequireFlyPanelUpdateHandler;
            RemoveAllConrols();
        }

        public override bool RefreshUI()
        {
            flyPanel.Invoke((MethodInvoker)delegate
            {
                var serverList = servers.GetServerList().ToList();

                if (serverList.Count > 0)
                {
                    RemoveWelcomeItem();
                }
                else
                {
                    RemoveAllConrols();
                    LoadWelcomeItem();
                    return;
                }

                RemoveDeletedServerItems(ref serverList);
                AddNewServerItems(serverList);
                ResetIndex();
            });
            return true;
        }
        #endregion

        #region private method
        void AddNewServerItems(List<Model.Data.ServerItem> serverList)
        {
            foreach (var server in serverList)
            {
                flyPanel.Controls.Add(
                    new Model.UserControls.ServerListItem(
                        int.MaxValue, server));
            }
        }

        void RemoveDeletedServerItems(ref List<Model.Data.ServerItem> serverList)
        {
            var controlList = GetAllControls();
            foreach (Model.UserControls.ServerListItem control in controlList)
            {
                var config = control.GetConfig();
                if (serverList.Where(s => s.config == config)
                    .FirstOrDefault() == null)
                {
                    control.Cleanup();
                    flyPanel.Controls.Remove(control);
                    control.Dispose();
                }
                serverList.RemoveAll(s => s.config == config);
            }
        }

        void RemoveWelcomeItem()
        {
            var welcome = GetAllControls()
                .Where(c => c is Model.UserControls.WelcomeFlyPanelComponent)
                .FirstOrDefault();

            if (welcome == null)
            {
                return;
            }

            flyPanel.Controls.Remove(welcome);
        }

        void LoopThroughAllServer(Action<Model.UserControls.ServerListItem> worker)
        {
            foreach (Model.BaseClass.IFormMainFlyPanelComponent control in flyPanel.Controls)
            {
                if (!(control is Model.UserControls.ServerListItem))
                {
                    continue;
                }
                worker(control as Model.UserControls.ServerListItem);
            }
        }

        List<UserControl> GetAllControls()
        {
            List<UserControl> controlList = new List<UserControl>();
            foreach (Model.BaseClass.IFormMainFlyPanelComponent control in flyPanel.Controls)
            {
                switch (control)
                {
                    case Model.UserControls.ServerListItem serv:
                        controlList.Add(serv);
                        break;
                    case Model.UserControls.WelcomeFlyPanelComponent welcome:
                        controlList.Add(welcome);
                        break;
                }
            }
            return controlList;
        }

        void RemoveAllConrols()
        {
            var listControls = GetAllControls();
            foreach (UserControl control in listControls)
            {
                switch (control)
                {
                    case Model.UserControls.ServerListItem serv:
                        serv.Cleanup();
                        break;
                    case Model.UserControls.WelcomeFlyPanelComponent welcome:
                        welcome.Cleanup();
                        break;
                }
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
