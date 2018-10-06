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
        Service.Setting setting;
        ToolStripComboBox cboxMarkFilter;
        ToolStripStatusLabel tslbTotal, tslbPrePage, tslbNextPage;
        ToolStripDropDownButton tsdbtnPager;
        Lib.CancelableTimeout lazyStatusBarUpdateTimer = null;
        Model.UserControls.WelcomeUI welcomeItem = null;
        int[] paging = new int[] { 0, 1 }; // 0: current page 1: total page

        public FlyServer(
            FlowLayoutPanel panel,
            ToolStripComboBox cboxMarkeFilter,
            ToolStripStatusLabel tslbTotal,
            ToolStripDropDownButton tsdbtnPager,
            ToolStripStatusLabel tslbPrePage,
            ToolStripStatusLabel tslbNextPage)
        {
            this.servers = Service.Servers.Instance;
            this.setting = Service.Setting.Instance;

            this.flyPanel = panel;
            this.cboxMarkFilter = cboxMarkeFilter;
            this.tsdbtnPager = tsdbtnPager;
            this.tslbTotal = tslbTotal;
            this.tslbPrePage = tslbPrePage;
            this.tslbNextPage = tslbNextPage;
            this.welcomeItem = new Model.UserControls.WelcomeUI();

            InitFormControls();
            BindDragDropEvent();
            RefreshUI();
            servers.OnRequireFlyPanelUpdate += OnRequireFlyPanelUpdateHandler;
            servers.OnRequireStatusBarUpdate += OnRequireStatusBarUpdateHandler;
        }


        #region public method
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
            RemoveAllServersConrol(true);
        }

        public void RemoveAllServersConrol(bool blocking = false)
        {
            var controlList = GetAllServersControl();

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
                    new Lib.CancelableTimeout(
                        UpdateStatusBar,
                        300);
            }

            lazyStatusBarUpdateTimer.Start();
        }

        public override bool RefreshUI()
        {
            ResetIndex();
            var list = this.GetFilteredList();
            var pagedList = GenPagedServerList(list);

            flyPanel?.Invoke((MethodInvoker)delegate
            {
                if (pagedList.Count > 0)
                {
                    RemoveWelcomeItem();
                }
                else
                {
                    RemoveAllServersConrol();
                    if (string.IsNullOrEmpty(this.searchKeywords))
                    {
                        LoadWelcomeItem();
                    }
                    return;
                }

                RemoveDeletedServerItems(ref pagedList);
                AddNewServerItems(pagedList);
            });
            LazyStatusBarUpdater();
            return true;
        }
        #endregion

        #region private method
        List<Controller.ServerCtrl> GenPagedServerList(List<Controller.ServerCtrl> serverList)
        {
            var count = serverList.Count;
            var pageSize = setting.serverPanelPageSize;
            paging[1] = (int)Math.Ceiling(1.0 * count / pageSize);
            paging[0] = Lib.Utils.Clamp(paging[0], 0, paging[1]);

            if (serverList.Count <= 0)
            {
                return serverList;
            }

            var begin = paging[0] * pageSize;
            var num = Math.Min(pageSize, count - begin);
            return serverList.GetRange(begin, num);
        }

        void OnRequireStatusBarUpdateHandler(object sender, EventArgs args)
        {
            LazyStatusBarUpdater();
        }

        void UpdateStatusBar()
        {
            var text = string.Format(
                I18N("StatusBarServerCountTpl"),
                    servers.GetTotalServerCount())
                + " "
                + string.Format(
                    I18N("StatusBarTplSelectedItem"),
                    servers.GetTotalSelectedServerCount(),
                    GetAllServersControl().Count());

            var showPager = paging[1] > 1;

            flyPanel?.Invoke((MethodInvoker)delegate
            {
                if (showPager)
                {
                    if (paging[1] != tsdbtnPager.DropDownItems.Count)
                    {
                        UpdateStatusBarPagerMenu();
                    }

                    UpdateStatusBarPagerCheckStatus();

                    tsdbtnPager.Text = string.Format(
                        I18N("StatusBarPagerInfoTpl"),
                        paging[0] + 1,
                        paging[1]);
                }

                tsdbtnPager.Visible = showPager;
                tslbNextPage.Visible = showPager;
                tslbPrePage.Visible = showPager;

                if (text != tslbTotal.Text)
                {
                    tslbTotal.Text = text;
                }
            });
        }

        private void UpdateStatusBarPagerCheckStatus()
        {
            for (int i = 0; i < tsdbtnPager.DropDownItems.Count; i++)
            {
                (tsdbtnPager.DropDownItems[i] as ToolStripMenuItem)
                    .Checked = paging[0] == i;
            }
        }

        private void UpdateStatusBarPagerMenu()
        {
            tsdbtnPager.DropDownItems.Clear();
            for (int i = 0; i < paging[1]; i++)
            {
                var index = i;
                var item = new ToolStripMenuItem(
                    string.Format(I18N("StatusBarPagerMenuTpl"), (index + 1)),
                    null,
                    (s, a) =>
                    {
                        paging[0] = index;

                        // 切换分页的时候应保留原选定状态
                        // 否则无法批量对大量服务进行测速排序
                        // servers.ClearSelection();

                        RefreshUI();
                    });
                tsdbtnPager.DropDownItems.Add(item);
            }
        }

        string searchKeywords = "";
        Lib.CancelableTimeout lazyShowSearchResultTimer = null;
        void LazyShowSearchResult()
        {
            // create on demand
            if (lazyShowSearchResultTimer == null)
            {
                var delay = Lib.Utils.Str2Int(StrConst("LazySaveServerListDelay"));

                lazyShowSearchResultTimer =
                    new Lib.CancelableTimeout(
                        () =>
                        {
                            // 如果不RemoveAll会乱序
                            RemoveAllServersConrol();

                            // 修改搜索项时应该清除选择,否则会有可显示列表外的选中项
                            servers.SetAllServerIsSelected(false);

                            RefreshUI();
                        },
                        600);
            }

            lazyShowSearchResultTimer.Start();
        }

        private void InitFormControls()
        {
            InitComboBoxMarkFilter();
            tslbPrePage.Click += (s, a) =>
            {
                paging[0]--;
                RefreshUI();
            };

            tslbNextPage.Click += (s, a) =>
            {
                paging[0]++;
                RefreshUI();
            };
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

        List<Controller.ServerCtrl> GetFilteredList()
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

        void AddNewServerItems(List<Controller.ServerCtrl> serverList)
        {
            flyPanel.Controls.AddRange(
                serverList
                    .Select(s => new Model.UserControls.ServerUI(s))
                    .ToArray());
        }

        void DisposeFlyPanelControlByList(List<Model.UserControls.ServerUI> controlList)
        {
            foreach (var control in controlList)
            {
                control.Cleanup();
            }

            flyPanel.Invoke((MethodInvoker)delegate
            {
                foreach (var control in controlList)
                {
                    control.Dispose();
                }
            });
        }

        void RemoveDeletedServerItems(ref List<Controller.ServerCtrl> serverList)
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

        List<Model.UserControls.ServerUI> GetDeletedControlList(List<Controller.ServerCtrl> serverList)
        {
            var result = new List<Model.UserControls.ServerUI>();

            foreach (var control in GetAllServersControl())
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
                .OfType<Model.UserControls.WelcomeUI>()
                .Select(e =>
                {
                    flyPanel.Controls.Remove(e);
                    return true;
                })
                .ToList();
        }

        void LoopThroughServerItemControls(Action<Model.UserControls.ServerUI> worker)
        {
            var list = flyPanel.Controls
                .OfType<Model.UserControls.ServerUI>()
                .Select(e =>
                {
                    worker(e);
                    return true;
                })
                .ToList();
        }

        List<Model.UserControls.ServerUI> GetAllServersControl()
        {
            return flyPanel.Controls
                .OfType<Model.UserControls.ServerUI>()
                .ToList();
        }

        void OnRequireFlyPanelUpdateHandler(object sender, EventArgs args)
        {
            RefreshUI();
        }

        private void LoadWelcomeItem()
        {
            var welcome = flyPanel.Controls
                .OfType<Model.UserControls.WelcomeUI>()
                .FirstOrDefault();

            if (welcome == null)
            {
                flyPanel.Controls.Add(welcomeItem);
            }
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

                var serverItemMoving = a.Data.GetData(typeof(Model.UserControls.ServerUI))
                    as Model.UserControls.ServerUI;

                if (serverItemMoving == null)
                {
                    return;
                }

                var panel = s as FlowLayoutPanel;
                Point p = panel.PointToClient(new Point(a.X, a.Y));
                var controlDest = panel.GetChildAtPoint(p);
                int index = panel.Controls.GetChildIndex(controlDest, false);
                panel.Controls.SetChildIndex(serverItemMoving, index);
                var serverItemDest = controlDest as Model.UserControls.ServerUI;
                MoveServerListItem(ref serverItemMoving, ref serverItemDest);
            };
        }

        void MoveServerListItem(ref Model.UserControls.ServerUI moving, ref Model.UserControls.ServerUI destination)
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
