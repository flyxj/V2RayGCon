using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.FormMainComponent
{
    class FlyServer : FormMainComponentController
    {
        FlowLayoutPanel flyPanel;
        Service.Servers servers;
        ComboBox cboxMark;
        List<Model.Data.ServerItem> serverList = null;
        int preSelectedMarkFilterIndex;


        public FlyServer(
            FlowLayoutPanel panel,
            ComboBox cboxMarkeFilter)
        {
            this.servers = Service.Servers.Instance;
            this.flyPanel = panel;
            this.cboxMark = cboxMarkeFilter;
            this.serverList = servers.GetServerList().ToList();

            InitMarkFilter(cboxMarkeFilter);
            BindDragDropEvent();
            ResetIndex();
            RefreshUI();
            servers.OnRequireFlyPanelUpdate += OnRequireFlyPanelUpdateHandler;
        }

        #region public method
        public void SelectAutorun()
        {
            LoopThroughAllServerItemControl((s) => s.SetSelected(s.GetAutorunStatus()));
        }

        public void SelectAll()
        {
            LoopThroughAllServerItemControl((s) => s.SetSelected(true));
        }

        public void SelectNone()
        {
            LoopThroughAllServerItemControl((s) => s.SetSelected(false));
        }

        public void SelectInvert()
        {
            LoopThroughAllServerItemControl((s) => s.SetSelected(!s.GetSelectStatus()));
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
                if (serverList == null || serverList.Count > 0)
                {
                    RemoveWelcomeItem();
                }
                else
                {
                    RemoveAllConrols();
                    if (preSelectedMarkFilterIndex <= 0)
                    {
                        LoadWelcomeItem();
                    }
                    return;
                }

                RemoveDeletedServerItems(ref serverList);
                AddNewServerItems(serverList);
                //  ResetIndex();
            });
            return true;
        }
        #endregion

        #region private method
        private void InitMarkFilter(ComboBox cboxMarkFilter)
        {
            UpdateMarkFilterItemList(cboxMarkFilter);
            cboxMarkFilter.DropDown += (s, e) =>
            {
                cboxMarkFilter.Invoke((MethodInvoker)delegate
                {
                    UpdateMarkFilterItemList(cboxMarkFilter);
                    Lib.UI.ResetComboBoxDropdownMenuWidth(cboxMarkFilter);
                });
            };
            cboxMarkFilter.SelectedIndexChanged += MarkFilterChangeHandler;
            preSelectedMarkFilterIndex = -1;
            cboxMarkFilter.SelectedIndex = 0;
        }

        void MarkFilterChangeHandler(object sernder, EventArgs args)
        {
            var index = cboxMark.SelectedIndex;
            if (preSelectedMarkFilterIndex == index)
            {
                return;
            }

            preSelectedMarkFilterIndex = index;
            SelectNone();
            this.serverList = GetFilteredList();
            RemoveAllConrols();
            RefreshUI();
        }

        List<Model.Data.ServerItem> GetFilteredList()
        {
            var list = servers.GetServerList();
            if (preSelectedMarkFilterIndex < 0)
            {
                return list.ToList();
            }
            switch (preSelectedMarkFilterIndex)
            {
                case 0:
                    return list.ToList();
                case 1:
                    return list.Where(s => string.IsNullOrEmpty(s.mark)).ToList();
            }

            var markList = servers.GetMarkList();
            return list.Where(s => s.mark == markList[preSelectedMarkFilterIndex - 2]).ToList();
        }

        void UpdateMarkFilterItemList(ComboBox marker)
        {
            var itemList = servers.GetMarkList().ToList();
            itemList.Insert(0, I18N("NoMark"));
            itemList.Insert(0, I18N("All"));

            marker.Items.Clear();
            foreach (var item in itemList)
            {
                marker.Items.Add(item);
            }
        }

        void AddNewServerItems(List<Model.Data.ServerItem> serverList)
        {
            foreach (var server in serverList)
            {
                flyPanel.Controls.Add(
                    new Model.UserControls.ServerListItem(server));
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

        void LoopThroughAllServerItemControl(Action<Model.UserControls.ServerListItem> worker)
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
            this.serverList = GetFilteredList();
            RefreshUI();
        }

        private void LoadWelcomeItem()
        {
            flyPanel.Controls.Add(
                new Model.UserControls.WelcomeFlyPanelComponent());
        }

        private void ResetIndex()
        {
            var list = servers.GetServerList().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var index = i + 1; // closure
                var item = list[i];
                item.SetPropertyOnDemand(ref item.index, index);
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

                var serverItemMoving = a.Data.GetData(typeof(Model.UserControls.ServerListItem))
                    as Model.UserControls.ServerListItem;

                if (serverItemMoving == null)
                {
                    return;
                }

                var panel = s as FlowLayoutPanel;
                Point p = panel.PointToClient(new Point(a.X, a.Y));
                var controlDest = panel.GetChildAtPoint(p);
                int index = panel.Controls.GetChildIndex(controlDest, false);
                panel.Controls.SetChildIndex(serverItemMoving, index);

                var serverItemDest = controlDest as Model.UserControls.ServerListItem;
                var indexDest = serverItemDest.GetIndex();
                var newIndex = indexDest < serverItemMoving.GetIndex() ? indexDest - 1 : indexDest + 1;
                serverItemMoving.SetIndex(newIndex);
                ResetIndex();
                this.serverList = servers.GetServerList().ToList();
                RemoveAllConrols();
                RefreshUI();
            };
        }
        #endregion
    }
}
