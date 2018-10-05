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
        Model.BaseClass.CancelableTimeout lazyStatusBarUpdateTimer = null;

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
            servers.OnRequireStatusBarUpdate += OnRequireStatusBarUpdateHandler;
        }


        #region public method
        public void SelectNoSpeedTest()
        {
            LoopThroughServerItemControls((s) => s.SetSelected(s.isNotRunSpeedTestYet));
        }

        public void SelectNoMark()
        {
            LoopThroughServerItemControls((s) => s.SetSelected(s.isMarkEmpty));
        }

        public void SelectTimeout()
        {
            LoopThroughServerItemControls((s) => s.SetSelected(s.isSpeedTestTimeout));
        }

        public void SelectRunning()
        {
            LoopThroughServerItemControls((s) => s.SetSelected(s.isRunning));
        }

        public void SelectAutorun()
        {
            LoopThroughServerItemControls((s) => s.SetSelected(s.isAutoRun));
        }

        public void SelectAll()
        {
            LoopThroughServerItemControls((s) => s.SetSelected(true));
        }

        public void SelectNone()
        {
            LoopThroughServerItemControls((s) => s.SetSelected(false));
        }

        public void SelectInvert()
        {
            LoopThroughServerItemControls((s) => s.SetSelected(!s.isSelected));
        }

        public override void Cleanup()
        {
            servers.OnRequireFlyPanelUpdate -= OnRequireFlyPanelUpdateHandler;
            servers.OnRequireStatusBarUpdate -= OnRequireStatusBarUpdateHandler;
            lazyStatusBarUpdateTimer?.Release();
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

        public void LazyStatusBarUpdater()
        {
            // create on demand
            if (lazyStatusBarUpdateTimer == null)
            {
                lazyStatusBarUpdateTimer =
                    new Model.BaseClass.CancelableTimeout(
                        UpdateStatusBar,
                        300);
            }

            lazyStatusBarUpdateTimer.Start();
        }

        public override bool RefreshUI()
        {
            ResetIndex();
            var list = this.GetFilteredList();
            flyPanel?.Invoke((MethodInvoker)delegate
            {
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
            LazyStatusBarUpdater();
            return true;
        }
        #endregion

        #region private method
        void OnRequireStatusBarUpdateHandler(object sender, EventArgs args)
        {
            LazyStatusBarUpdater();
        }

        void UpdateStatusBar()
        {
            var list = flyPanel.Controls
                .OfType<Model.UserControls.ServerListItem>();

            var total = list.Count();
            var selected = list.Where(c => c.isSelected).Count();

            var text = string.Format(
                "{0}: {1} {2}: {3}",
                I18N("Selected"),
                selected,
                I18N("Total"),
                total);

            flyPanel?.Invoke((MethodInvoker)delegate
            {
                if (text != tslbTotal.Text)
                {
                    tslbTotal.Text = text;
                }
            });
        }

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

                            // cleanup selection
                            servers.GetServerList()
                                .Select(s => s.isSelected = false)
                                .ToList();

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
            marker.Items.AddRange(
                servers.GetMarkList().ToArray());
        }

        void AddNewServerItems(List<Model.Data.ServerItem> serverList)
        {
            flyPanel.Controls.AddRange(
                serverList
                    .Select(s => new Model.UserControls.ServerListItem(s))
                    .ToArray());
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
            var list = flyPanel.Controls
                .OfType<Model.UserControls.WelcomeFlyPanelComponent>()
                .Select(e =>
                {
                    flyPanel.Controls.Remove(e);
                    if (!e.IsDisposed)
                    {
                        e.Dispose();
                    }
                    return true;
                })
                .ToList();
        }

        void LoopThroughServerItemControls(Action<Model.UserControls.ServerListItem> worker)
        {
            var list = flyPanel.Controls
                .OfType<Model.UserControls.ServerListItem>()
                .Select(e =>
                {
                    worker(e);
                    return true;
                })
                .ToList();
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
