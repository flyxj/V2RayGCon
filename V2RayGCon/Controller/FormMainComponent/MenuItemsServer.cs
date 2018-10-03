using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.FormMainComponent
{
    class MenuItemsServer : FormMainComponentController
    {
        Service.Cache cache;
        Service.Servers servers;

        public MenuItemsServer(
            ToolStripMenuItem stopSelected,
            ToolStripMenuItem restartSelected,
            ToolStripMenuItem clearSysProxy,
            ToolStripMenuItem refreshSummary,
            ToolStripMenuItem speedTestOnSelected,
            ToolStripMenuItem deleteSelected,
            ToolStripMenuItem copyAsV2rayLinks,
            ToolStripMenuItem copyAsVmessLinks,
            ToolStripMenuItem copyAsSubscriptions,
            ToolStripMenuItem deleteAllItems,
            ToolStripMenuItem modifySelected,
            ToolStripMenuItem packSelected,
            ToolStripMenuItem moveToTop,
            ToolStripMenuItem moveToBottom,
            ToolStripMenuItem collapsePanel,
            ToolStripMenuItem expansePanel,
            ToolStripMenuItem sortBySpeed,
            ToolStripMenuItem sortBySummary)
        {
            cache = Service.Cache.Instance;
            servers = Service.Servers.Instance;

            InitCtrlSorting(sortBySpeed, sortBySummary);
            InitCtrlView(moveToTop, moveToBottom, collapsePanel, expansePanel);
            InitCtrlCopyToClipboard(copyAsV2rayLinks, copyAsVmessLinks, copyAsSubscriptions);
            InitCtrlDelete(deleteSelected, deleteAllItems);
            InitCtrlBatchOperation(stopSelected, restartSelected, speedTestOnSelected, modifySelected, packSelected);
            InitCtrlSysProxy(clearSysProxy, refreshSummary);
        }

        #region public method
        public override bool RefreshUI()
        {
            return false;
        }

        public override void Cleanup()
        {
        }
        #endregion

        #region private method
        EventHandler GenSelectedServerHandler(Action lambda)
        {
            return (s, a) =>
            {
                if (!servers.IsSelecteAnyServer())
                {
                    Task.Factory.StartNew(() => MessageBox.Show(I18N("SelectServerFirst")));
                    return;
                }
                lambda();
            };
        }

        private void InitCtrlBatchOperation(ToolStripMenuItem stopSelected, ToolStripMenuItem restartSelected, ToolStripMenuItem speedTestOnSelected, ToolStripMenuItem modifySelected, ToolStripMenuItem packSelected)
        {
            modifySelected.Click += GenSelectedServerHandler(
                () => Views.FormBatchModifyServerSetting.GetForm());


            packSelected.Click += GenSelectedServerHandler(
                () => servers.PackSelectedServers());

            speedTestOnSelected.Click += GenSelectedServerHandler(() =>
            {
                if (!Lib.UI.Confirm(I18N("TestWillTakeALongTime")))
                {
                    return;
                }

                if (!servers.RunSpeedTestOnSelectedServers())
                {
                    MessageBox.Show(I18N("LastTestNoFinishYet"));
                }
            });

            stopSelected.Click += GenSelectedServerHandler(() =>
            {
                if (Lib.UI.Confirm(I18N("ConfirmStopAllSelectedServers")))
                {
                    servers.StopAllSelectedThen();
                }
            });

            restartSelected.Click += GenSelectedServerHandler(() =>
            {
                if (Lib.UI.Confirm(I18N("ConfirmRestartAllSelectedServers")))
                {
                    servers.RestartAllSelectedServersThen();
                }
            });
        }

        private void InitCtrlDelete(ToolStripMenuItem deleteSelected, ToolStripMenuItem deleteAllItems)
        {
            deleteAllItems.Click += (s, a) =>
            {
                if (!Lib.UI.Confirm(I18N("ConfirmDeleteAllServers")))
                {
                    return;
                }
                Service.Servers.Instance.DeleteAllServersThen();
                Service.Cache.Instance.core.Clear();
            };

            deleteSelected.Click += GenSelectedServerHandler(() =>
            {
                if (!Lib.UI.Confirm(I18N("ConfirmDeleteSelectedServers")))
                {
                    return;
                }
                servers.DeleteSelectedServersThen();
            });
        }

        private void InitCtrlCopyToClipboard(ToolStripMenuItem copyAsV2rayLinks, ToolStripMenuItem copyAsVmessLinks, ToolStripMenuItem copyAsSubscriptions)
        {
            copyAsSubscriptions.Click += GenSelectedServerHandler(() =>
            {
                MessageBox.Show(
                Lib.Utils.CopyToClipboard(
                    Lib.Utils.Base64Encode(
                        EncodeAllServersIntoVmessLinks())) ?
                I18N("LinksCopied") :
                I18N("CopyFail"));
            });

            copyAsV2rayLinks.Click += GenSelectedServerHandler(() =>
            {
                var list = servers.GetServerList()
                    .Where(s => s.isSelected)
                    .Select(s => Lib.Utils.AddLinkPrefix(
                        Lib.Utils.Base64Encode(s.config),
                        Model.Data.Enum.LinkTypes.v2ray))
                    .ToList();

                MessageBox.Show(
                    Lib.Utils.CopyToClipboard(
                        string.Join(Environment.NewLine, list)) ?
                    I18N("LinksCopied") :
                    I18N("CopyFail"));
            });

            copyAsVmessLinks.Click += GenSelectedServerHandler(() =>
            {
                MessageBox.Show(
                   Lib.Utils.CopyToClipboard(
                       EncodeAllServersIntoVmessLinks()) ?
                   I18N("LinksCopied") :
                   I18N("CopyFail"));
            });
        }

        private void InitCtrlView(ToolStripMenuItem moveToTop, ToolStripMenuItem moveToBottom, ToolStripMenuItem collapsePanel, ToolStripMenuItem expansePanel)
        {
            expansePanel.Click += GenSelectedServerHandler(() =>
            {
                SetServerItemPanelIsCollapseProperty(false);
            });

            collapsePanel.Click += GenSelectedServerHandler(() =>
            {
                SetServerItemPanelIsCollapseProperty(true);
            });

            moveToTop.Click += GenSelectedServerHandler(() =>
            {
                SetServerItemsIndex(0);
            });

            moveToBottom.Click += GenSelectedServerHandler(() =>
            {
                SetServerItemsIndex(double.MaxValue);
            });
        }

        private void InitCtrlSorting(ToolStripMenuItem sortBySpeed, ToolStripMenuItem sortBySummary)
        {
            sortBySummary.Click += GenSelectedServerHandler(
                SortServerListBySummary);

            sortBySpeed.Click += GenSelectedServerHandler(
                SortServerListBySpeedTestResult);
        }

        private void SortServerListBySummary()
        {
            var list = servers.GetServerList().Where(s => s.isSelected).ToList();
            if (list.Count < 2)
            {
                return;
            }

            SortServerItemList(ref list, (a, b) => a.summary.CompareTo(b.summary));
            RemoveAllControlsAndRefreshFlyPanel();
        }

        void SortServerItemList(
             ref List<Model.Data.ServerItem> list,
             Comparison<Model.Data.ServerItem> comparer)
        {
            if (list == null || list.Count < 2)
            {
                return;
            }

            list.Sort(comparer);
            var minIndex = list.First().index;
            var delta = 1.0 / 2 / list.Count;
            for (int i = 1; i < list.Count; i++)
            {
                list[i].index = minIndex + delta * i;
            }
        }

        private void SortServerListBySpeedTestResult()
        {
            var list = servers.GetServerList().Where(s => s.isSelected).ToList();
            if (list.Count < 2)
            {
                return;
            }

            SortServerItemList(ref list, (a, b) => a.speedTestResult.CompareTo(b.speedTestResult));
            RemoveAllControlsAndRefreshFlyPanel();
        }

        void SetServerItemPanelIsCollapseProperty(bool isCollapse)
        {
            servers
                .GetServerList()
                .Where(s => s.isSelected)
                .Select(s =>
                {
                    if (s.isCollapse != isCollapse)
                    {
                        s.ToggleIsCollapse();
                    }
                    return s;
                })
                .ToList(); // force linq to execute
        }

        void RemoveAllControlsAndRefreshFlyPanel()
        {
            var panel = GetFlyPanel();
            panel.RemoveAllConrols();
            panel.RefreshUI();
        }

        private void SetServerItemsIndex(double index)
        {
            servers.GetServerList()
                .Where(s => s.isSelected)
                .Select(s =>
                {
                    s.ChangeIndex(index);
                    return true;
                })
                .ToList(); // force linq to execute

            RemoveAllControlsAndRefreshFlyPanel();
        }

        private void InitCtrlSysProxy(ToolStripMenuItem clearSysProxy, ToolStripMenuItem refreshSummary)
        {
            // misc
            clearSysProxy.Click += (s, a) =>
            {
                if (Lib.UI.Confirm(I18N("ConfirmClearSysProxy")))
                {
                    Service.Setting.Instance.ClearSystemProxy();
                }
            };

            refreshSummary.Click += (s, a) =>
            {
                cache.html.Clear();
                servers.UpdateAllServersSummary();
            };
        }

        string EncodeAllServersIntoVmessLinks()
        {
            var serverList = servers.GetServerList();
            string result = string.Empty;

            foreach (var server in serverList)
            {
                if (!server.isSelected)
                {
                    continue;
                }
                var vmess = Lib.Utils.ConfigString2Vmess(server.config);
                var vmessLink = Lib.Utils.Vmess2VmessLink(vmess);

                if (!string.IsNullOrEmpty(vmessLink))
                {
                    result += vmessLink + System.Environment.NewLine;
                }
            }

            return result;
        }

        Controller.FormMainComponent.FlyServer GetFlyPanel()
        {
            return this.GetContainer()
                .GetComponent<Controller.FormMainComponent.FlyServer>();
        }
        #endregion
    }
}
