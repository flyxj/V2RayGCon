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

            InitBatchOperation(
                stopSelected, restartSelected, speedTestOnSelected,
                deleteSelected, copyAsV2rayLinks, copyAsVmessLinks, copyAsSubscriptions,
                deleteAllItems, modifySelected, packSelected,
                moveToTop, moveToBottom, collapsePanel, expansePanel,
                sortBySpeed, sortBySummary);
            InitMisc(clearSysProxy, refreshSummary);
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
        private void InitBatchOperation(
            ToolStripMenuItem stopSelected,
            ToolStripMenuItem restartSelected,
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
            sortBySummary.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                SortServerListBySummary();

            };

            sortBySpeed.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                SortServerListBySpeedTestResult();
            };

            expansePanel.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                ExpansePanel();
            };

            collapsePanel.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                CollapsePanel();
            };

            moveToTop.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                SetServerItemsIndex(0);
            };

            moveToBottom.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                SetServerItemsIndex(int.MaxValue);
            };

            modifySelected.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                Views.FormBatchModifyServerSetting.GetForm();
            };

            packSelected.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }
                servers.PackSelectedServers();
            };

            deleteAllItems.Click += (s, a) =>
            {
                if (!Lib.UI.Confirm(I18N("ConfirmDeleteAllServers")))
                {
                    return;
                }
                Service.Servers.Instance.DeleteAllServersThen();
                Service.Cache.Instance.core.Clear();
            };

            copyAsSubscriptions.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }
                CopySelectedAsSubscription();
            };

            copyAsV2rayLinks.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                CopySelectedAsV2RayLinks();
            };

            copyAsVmessLinks.Click += (s, a) =>
            {

                if (!CheckSelectedServerCount())
                {
                    return;
                }

                CopySelectedAsVmessLinks();
            };

            deleteSelected.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                if (!Lib.UI.Confirm(I18N("ConfirmDeleteSelectedServers")))
                {
                    return;
                }

                servers.DeleteSelectedServersThen();
            };

            speedTestOnSelected.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                if (!Lib.UI.Confirm(I18N("TestWillTakeALongTime")))
                {
                    return;
                }

                if (!servers.RunSpeedTestOnSelectedServers())
                {
                    MessageBox.Show(I18N("LastTestNoFinishYet"));
                }
            };

            stopSelected.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                if (Lib.UI.Confirm(I18N("ConfirmStopAllSelectedServers")))
                {
                    servers.StopAllSelectedThen();
                }
            };

            restartSelected.Click += (s, a) =>
            {
                if (!CheckSelectedServerCount())
                {
                    return;
                }

                if (Lib.UI.Confirm(I18N("ConfirmRestartAllSelectedServers")))
                {
                    servers.RestartAllSelectedServersThen();
                }
            };
        }

        private void SortServerListBySummary()
        {
            var list = servers.GetServerList().Where(s => s.isSelected).ToList();
            if (list.Count < 2)
            {
                return;
            }

            SortServerItemList(ref list, (a, b) => a.summary.CompareTo(b.summary));
            RemoveAllCtrolsAndRefreshFlyPanel();
        }

        void SortServerItemList(
             ref List<Model.Data.ServerItem> list,
             Comparison<Model.Data.ServerItem> comparer)
        {
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
            RemoveAllCtrolsAndRefreshFlyPanel();
        }

        void UpdateServerItemPanelIsCollapse(bool isCollapse)
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

        void RemoveAllCtrolsAndRefreshFlyPanel()
        {
            var panel = GetFlyPanel();
            panel.RemoveAllConrols();
            panel.RefreshUI();
        }

        private void CollapsePanel()
        {
            UpdateServerItemPanelIsCollapse(true);
        }

        private void ExpansePanel()
        {
            UpdateServerItemPanelIsCollapse(false);
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

            RemoveAllCtrolsAndRefreshFlyPanel();
        }

        private void InitMisc(ToolStripMenuItem clearSysProxy, ToolStripMenuItem refreshSummary)
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

        bool CheckSelectedServerCount()
        {
            if (!servers.IsSelecteAnyServer())
            {
                Task.Factory.StartNew(() => MessageBox.Show(I18N("SelectServerFirst")));
                return false;
            }
            return true;
        }

        void CopySelectedAsV2RayLinks()
        {
            var serverList = servers.GetServerList();
            string s = string.Empty;

            foreach (var server in serverList)
            {
                if (server.isSelected)
                {
                    s += "v2ray://" + Lib.Utils.Base64Encode(server.config) + "\r\n";
                }
            }

            MessageBox.Show(
                Lib.Utils.CopyToClipboard(s) ?
                I18N("LinksCopied") :
                I18N("CopyFail"));
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

        void CopySelectedAsSubscription()
        {
            MessageBox.Show(
                Lib.Utils.CopyToClipboard(
                    Lib.Utils.Base64Encode(
                        EncodeAllServersIntoVmessLinks())) ?
                I18N("LinksCopied") :
                I18N("CopyFail"));
        }

        void CopySelectedAsVmessLinks()
        {
            MessageBox.Show(
                Lib.Utils.CopyToClipboard(
                    EncodeAllServersIntoVmessLinks()) ?
                I18N("LinksCopied") :
                I18N("CopyFail"));
        }

        Controller.FormMainComponent.FlyServer GetFlyPanel()
        {
            return this.GetContainer()
                .GetComponent<Controller.FormMainComponent.FlyServer>();
        }
        #endregion
    }
}
