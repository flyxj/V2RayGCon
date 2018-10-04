using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.FormMainComponent
{
    class FlyServer : FormMainComponentController
    {
        FlowLayoutPanel flyPanel;
        Service.Servers servers;
        ToolStripComboBox cboxMarkFilter;
        ToolStripStatusLabel tslbTotal;

        public FlyServer(
            FlowLayoutPanel panel,
            ToolStripComboBox cboxMarkeFilter,
            ToolStripStatusLabel tslbTotal)
        {
            this.servers = Service.Servers.Instance;
            this.flyPanel = panel;
            this.cboxMarkFilter = cboxMarkeFilter;
            this.tslbTotal = tslbTotal;

            InitFormControls();
            BindDragDropEvent();
            RefreshUI();
            servers.OnRequireFlyPanelUpdate += OnRequireFlyPanelUpdateHandler;
        }

        #region public method
        public void SelectNoSpeedTest()
        {
            LoopThroughAllServerItemControl((s) => s.SetSelected(s.isNotRunSpeedTestYet));
        }

        public void SelectNoMark()
        {
            LoopThroughAllServerItemControl((s) => s.SetSelected(s.IsMarkEmpty()));
        }

        public void SelectTimeout()
        {
            LoopThroughAllServerItemControl((s) => s.SetSelected(s.isSpeedTestTimeout));
        }

        public void SelectRunning()
        {
            LoopThroughAllServerItemControl((s) => s.SetSelected(s.isRunning));
        }

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
            RemoveAllConrols(true);
        }

        public void RemoveAllConrols(bool blocking = false)
        {
            var controlList = GetAllControls();

            flyPanel.Invoke((MethodInvoker)delegate
            {
                flyPanel.SuspendLayout();
                flyPanel.Controls.Clear();
                flyPanel.ResumeLayout();
            });

            if (blocking)
            {
                DisposeFlyPanelControlByList(controlList);
            }
            else
            {
                Task.Factory.StartNew(
                    () => DisposeFlyPanelControlByList(
                        controlList));
            }
        }

        public override bool RefreshUI()
        {
            ResetIndex();
            var list = this.GetFilteredList();

            flyPanel.Invoke((MethodInvoker)delegate
            {
                tslbTotal.Text = I18N("Total") + ": " + list.Count;

                if (list == null || list.Count > 0)
                {
                    RemoveWelcomeItem();
                }
                else
                {
                    RemoveAllConrols();
                    if (string.IsNullOrEmpty(this.searchKeywords))
                    {
                        LoadWelcomeItem();
                    }
                    return;
                }

                RemoveDeletedServerItems(ref list);
                AddNewServerItems(list);
            });
            return true;
        }
        #endregion

        #region private method
        string searchKeywords = "";
        Model.BaseClass.CancelableTimeout lazyShowSearchResultTimer = null;
        void LazyShowSearchResult()
        {
            // create on demand
            if (lazyShowSearchResultTimer == null)
            {
                var delay = Lib.Utils.Str2Int(StrConst("LazySaveServerListDelay"));

                lazyShowSearchResultTimer =
                    new Model.BaseClass.CancelableTimeout(
                        () =>
                        {
                            // 如果不RemoveAll会乱序
                            RemoveAllConrols();
                            RefreshUI();
                        },
                        600);
            }

            lazyShowSearchResultTimer.Start();
        }

        private void InitFormControls()
        {
            InitComboBoxMarkFilter();
        }

        private void InitComboBoxMarkFilter()
        {
            UpdateMarkFilterItemList(cboxMarkFilter);

            cboxMarkFilter.DropDown += (s, e) =>
            {
                // cboxMarkFilter has no Invoke method.
                this.flyPanel.Invoke((MethodInvoker)delegate
                {
                    UpdateMarkFilterItemList(cboxMarkFilter);
                    Lib.UI.ResetComboBoxDropdownMenuWidth(cboxMarkFilter);
                });
            };

            cboxMarkFilter.TextChanged += (s, a) =>
            {
                this.searchKeywords = cboxMarkFilter.Text;
                LazyShowSearchResult();
            };
        }

        List<Model.Data.ServerItem> GetFilteredList()
        {
            var list = servers.GetServerList();
            var keywords = (searchKeywords ?? "").Split(
                new char[] { ' ', ',' },
                StringSplitOptions.RemoveEmptyEntries);

            if (keywords.Length < 1)
            {
                return list.ToList();
            }

            return list
                .Where(
                    e => keywords.All(
                        k => e.GetSearchTextList().Any(
                            s => Lib.Utils.PartialMatch(s, k))))
                .ToList();
        }

        void UpdateMarkFilterItemList(ToolStripComboBox marker)
        {
            marker.Items.Clear();
            foreach (var item in servers.GetMarkList())
            {
                marker.Items.Add(item);
            }
        }

        void AddNewServerItems(List<Model.Data.ServerItem> serverList)
        {
            var controls = new List<Model.UserControls.ServerListItem>();

            foreach (var server in serverList)
            {
                controls.Add(new Model.UserControls.ServerListItem(server));
            }

            flyPanel.Controls.AddRange(controls.ToArray());
        }

        void DisposeFlyPanelControlByList(List<UserControl> controlList)
        {
            foreach (var control in controlList)
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
            }

            flyPanel.Invoke((MethodInvoker)delegate
            {
                foreach (var control in controlList)
                {
                    control.Dispose();
                }
            });
        }

        void RemoveDeletedServerItems(ref List<Model.Data.ServerItem> serverList)
        {
            var deletedControlList = GetDeletedControlList(serverList);

            flyPanel.SuspendLayout();
            foreach (var control in deletedControlList)
            {
                flyPanel.Controls.Remove(control);
            }
            flyPanel.ResumeLayout();
            Task.Factory.StartNew(() => DisposeFlyPanelControlByList(deletedControlList));
        }

        List<UserControl> GetDeletedControlList(List<Model.Data.ServerItem> serverList)
        {
            var controlList = GetAllControls();
            var result = new List<UserControl>();

            foreach (Model.UserControls.ServerListItem control in controlList)
            {
                var config = control.GetConfig();
                if (serverList.Where(s => s.config == config)
                    .FirstOrDefault() == null)
                {
                    result.Add(control);
                }
                serverList.RemoveAll(s => s.config == config);
            }

            return result;
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
            if (!welcome.IsDisposed)
            {
                welcome.Dispose();
            }
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
            var list = servers.GetServerList().ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var index = i + 1.0; // closure
                var item = list[i];
                item.ChangeIndex(index);
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
                MoveServerListItem(ref serverItemMoving, ref serverItemDest);
            };
        }

        void MoveServerListItem(ref Model.UserControls.ServerListItem moving, ref Model.UserControls.ServerListItem destination)
        {
            var indexDest = destination.GetIndex();
            var indexMoving = moving.GetIndex();

            if (indexDest == indexMoving)
            {
                return;
            }

            moving.SetIndex(
                indexDest < indexMoving ?
                indexDest - 0.5 :
                indexDest + 0.5);

            RefreshUI();
        }
        #endregion
    }
}
