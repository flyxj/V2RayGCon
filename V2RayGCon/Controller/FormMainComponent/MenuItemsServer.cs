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
            ToolStripMenuItem selectAllAutorun,
            ToolStripMenuItem selectAll,
            ToolStripMenuItem selectNone,
            ToolStripMenuItem selectInvert,
            ToolStripMenuItem speedTestOnSelected,
            ToolStripMenuItem deleteSelected,
            ToolStripMenuItem copyAsV2rayLinks,
            ToolStripMenuItem copyAsVmessLinks,
            ToolStripMenuItem copyAsSubscriptions,
            ToolStripMenuItem deleteAllItems,
            ToolStripMenuItem modifySelected,
            ToolStripMenuItem packSelected)
        {
            cache = Service.Cache.Instance;
            servers = Service.Servers.Instance;

            InitBatchOperation(stopSelected, restartSelected, speedTestOnSelected, deleteSelected, copyAsV2rayLinks, copyAsVmessLinks, copyAsSubscriptions, deleteAllItems, modifySelected, packSelected);
            InitSelection(selectAllAutorun, selectAll, selectNone, selectInvert);
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
            ToolStripMenuItem speedTestOnSelected, ToolStripMenuItem deleteSelected, ToolStripMenuItem copyAsV2rayLinks, ToolStripMenuItem copyAsVmessLinks,
            ToolStripMenuItem copyAsSubscriptions,
            ToolStripMenuItem deleteAllItems,
            ToolStripMenuItem modifySelected,
            ToolStripMenuItem packSelected)
        {

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

        private void InitSelection(ToolStripMenuItem selectAllAutorun, ToolStripMenuItem selectAll, ToolStripMenuItem selectNone, ToolStripMenuItem selectInvert)
        {
            // selection

            selectAllAutorun.Click += (s, a) =>
            {
                var panel = GetFlyPanel();
                panel.SelectAutorun();
            };

            selectAll.Click += (s, a) =>
            {
                var panel = GetFlyPanel();
                panel.SelectAll();
            };

            selectNone.Click += (s, a) =>
            {
                var panel = GetFlyPanel();
                panel.SelectNone();
            };

            selectInvert.Click += (s, a) =>
            {
                var panel = GetFlyPanel();
                panel.SelectInvert();
            };
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
