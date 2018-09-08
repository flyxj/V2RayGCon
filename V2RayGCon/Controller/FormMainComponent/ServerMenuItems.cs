using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.FormMainComponent
{
    class ServerMenuItems : FormMainComponentController
    {
        Service.Setting setting;
        Service.Cache cache;

        public ServerMenuItems(
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
            ToolStripMenuItem deleteAllItems)
        {
            setting = Service.Setting.Instance;
            cache = Service.Cache.Instance;

            deleteAllItems.Click += (s, a) =>
            {
                if (Lib.UI.Confirm(I18N("ConfirmDeleteAllServers")))
                    setting.DeleteAllServer();
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
                setting.DeleteSelectedServers();
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

                if (!setting.DoSpeedTestOnSelectedServers())
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
                    setting.StopAllSelectedThen();
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
                    setting.RestartAllSelected();
                }
            };

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


            clearSysProxy.Click += (s, a) =>
            {
                if (Lib.UI.Confirm(I18N("ConfirmClearSysProxy")))
                {
                    setting.ClearSystemProxy();
                }
            };

            refreshSummary.Click += (s, a) =>
            {
                cache.html.Clear();
                setting.UpdateAllServersSummary();
            };
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
        bool CheckSelectedServerCount()
        {
            var count = setting.GetSelectedServersCount();
            if (count <= 0)
            {
                Task.Factory.StartNew(() => MessageBox.Show(I18N("SelectServerFirst")));
                return false;
            }
            return true;
        }

        void CopySelectedAsV2RayLinks()
        {
            var servers = setting.GetServerList();
            string s = string.Empty;

            foreach (var server in servers)
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

        void CopySelectedAsVmessLinks()
        {
            var servers = setting.GetServerList();
            string s = string.Empty;

            foreach (var server in servers)
            {
                if (!server.isSelected)
                {
                    continue;
                }
                var vmess = Lib.Utils.ConfigString2Vmess(server.config);
                var vmessLink = Lib.Utils.Vmess2VmessLink(vmess);

                if (!string.IsNullOrEmpty(vmessLink))
                {
                    s += vmessLink + "\r\n";
                }
            }

            MessageBox.Show(
                Lib.Utils.CopyToClipboard(s) ?
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
