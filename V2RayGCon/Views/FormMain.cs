using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormMain : Form
    {
        #region Sigleton
        static FormMain _instant;
        public static FormMain GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormMain();
            }
            return _instant;
        }
        #endregion

        Service.Setting setting;
        Service.Core core;
        Controller.FormMainCtrl formMainCtrl;

        delegate void UpdateElementDelegate();

        FormMain()
        {
            setting = Service.Setting.Instance;
            core = Service.Core.Instance;

            InitializeComponent();

            formMainCtrl = new Controller.FormMainCtrl();

            ListViewSupportRightClickMenu();

            UpdateUI();

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= SettingChangeHandler;
                core.OnCoreStatChange -= SettingChangeHandler;
            };

            Lib.UI.SetFormLocation<FormMain>(this, Model.Data.Enum.FormLocations.TopLeft);

            this.Text = string.Format(
                "{0} v{1}",
                Properties.Resources.AppName,
                Properties.Resources.Version);

            this.Show();
            setting.OnSettingChange += SettingChangeHandler;
            core.OnCoreStatChange += SettingChangeHandler;
        }

        #region private method
        void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDownloadCore.GetForm();
        }

        void SettingChangeHandler(object s, EventArgs e)
        {
            UpdateElementDelegate updater =
                new UpdateElementDelegate(UpdateUI);

            try
            {
                lvServers?.Invoke(updater);
            }
            catch { }
        }

        void ListViewSupportRightClickMenu()
        {
            lvServers.Items.Clear();

            lvServers.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right
                && lvServers.FocusedItem != null
                && lvServers.FocusedItem.Bounds.Contains(e.Location))
                {
                    ctxMenuStrip.Show(Cursor.Position);
                }
            };

            lvServers.MouseDoubleClick += (s, a) => ActivateServer();
        }

        int GetSelectedServerIndex()
        {
            try
            {
                return Lib.Utils.Str2Int(lvServers.SelectedItems[0].Text) - 1;
            }
            catch { }
            return -1;
        }

        void ActivateServer()
        {
            var index = GetSelectedServerIndex();
            Debug.WriteLine("FormMain: activate server " + index);

            if (setting.proxyType == (int)Model.Data.Enum.ProxyTypes.http)
            {
                if (Lib.ProxySetter.getProxyState())
                {
                    Lib.ProxySetter.setProxy(setting.proxyAddr, true);
                }
            }
            else
            {
                Lib.ProxySetter.setProxy("", false);
            }

            setting.ActivateServer(index);
        }

        void UpdateUI()
        {
            // update list view
            lvServers.Items.Clear();
            var servers = setting.GetAllServerSummarys();
            var curServIndex = setting.GetCurServIndex();
            var curServNum = (curServIndex + 1).ToString();

            foreach (var server in servers)
            {
                server[5] = server[0].Equals(curServNum) ? "√" : string.Empty;
                lvServers.Items.Add(new ListViewItem(server));
            }

            // main menu check state
            proxyAddrToolStripTextBox.Text = setting.proxyAddr;

            protocolSocksToolStripMenuItem.Checked =
                setting.proxyType == (int)Model.Data.Enum.ProxyTypes.socks;

            protocolHttpStripMenuItem.Checked =
                setting.proxyType == (int)Model.Data.Enum.ProxyTypes.http;

            protocolConfigToolStripMenuItem.Checked =
                setting.proxyType == (int)Model.Data.Enum.ProxyTypes.config;

            var isSystemProxySet = Lib.ProxySetter.getProxyState();
            sysProxyDirectToolStripMenuItem.Checked = !isSystemProxySet;
            sysProxyHttpToolStripMenuItem.Checked = isSystemProxySet;

            var isCoreRunning = core.isRunning;
            activateToolStripMenuItem.Enabled = !isCoreRunning;
            activateToolStripMenuItem.Checked = isCoreRunning;
            stopToolStripMenuItem.Enabled = isCoreRunning;
            stopToolStripMenuItem.Checked = !isCoreRunning;

        }

        void SwitchToProtocal(Model.Data.Enum.ProxyTypes type)
        {
            setting.proxyType = (int)type;
            ActivateServer();
            UpdateUI();
        }
        #endregion

        #region UI event handler
        private void activateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActivateServer();
        }

        private void deleteStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Lib.UI.Confirm(I18N("ConfirmDeleteServer")))
            {
                setting.DeleteServer(GetSelectedServerIndex());
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteAllServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Lib.UI.Confirm(I18N("ConfirmDeleteAllServers")))
            {
                setting.DeleteAllServers();
            }
        }

        private void protocolHttpStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchToProtocal(Model.Data.Enum.ProxyTypes.http);
        }

        private void protocolSocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchToProtocal(Model.Data.Enum.ProxyTypes.socks);
        }

        private void proxyAddrToolStripTextBox_TextChange(object sender, EventArgs e)
        {
            if (!setting.proxyAddr.Equals(proxyAddrToolStripTextBox.Text))
            {
                setting.proxyAddr = proxyAddrToolStripTextBox.Text;
            }
        }

        private void proxyAddrToolStripTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // Debug.WriteLine("key: " + e.KeyCode);
            if (e.KeyCode == Keys.Enter)
            {
                setting.proxyAddr = proxyAddrToolStripTextBox.Text;
                ActivateServer();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormConfiger(GetSelectedServerIndex());
        }

        private void activateToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ActivateServer();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.StopCore();
        }

        private void ImportLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMainCtrl.ImportLinks();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActivateServer();
        }

        private void CopyAllVmessLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMainCtrl.CopyAllVmessLink();
        }

        private void CopyAllV2RayLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMainCtrl.CopyAllV2RayLink();
        }

        private void CopyVmessLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMainCtrl.CopyVmessLink(GetSelectedServerIndex());
        }

        private void CopyV2RayLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMainCtrl.CopyV2RayLink(GetSelectedServerIndex());
        }

        private void MoveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.MoveItemDown(GetSelectedServerIndex());
        }

        private void MoveToButtomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.MoveItemToButtom(GetSelectedServerIndex());
        }

        private void MoveToTopStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.MoveItemToTop(GetSelectedServerIndex());
        }

        private void MoveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.MoveItemUp(GetSelectedServerIndex());
        }

        private void ShowFormLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Views.FormLog.GetForm();
        }

        private void ShowFormQRCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Views.FormQRCode.GetForm();
        }

        private void ShowFormConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormConfiger();
        }

        private void sysProxyHttpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var http = (int)Model.Data.Enum.ProxyTypes.http;

            Lib.ProxySetter.setProxy(setting.proxyAddr, true);
            if (core.isRunning && setting.proxyType == http)
            {
                UpdateUI();
            }
            else
            {
                setting.proxyType = http;
                ActivateServer();
            }
        }

        private void sysProxyDirectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lib.ProxySetter.setProxy("", false);
            UpdateUI();
        }

        private void protocolConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchToProtocal(Model.Data.Enum.ProxyTypes.config);
        }

        private void updateV2rayGConToolStripMenuItem_Click(object sender, EventArgs e)
        {
            void CheckUpdate()
            {
                var version = Lib.Utils.GetLatestVGCVersion();
                if (string.IsNullOrEmpty(version))
                {
                    MessageBox.Show(I18N("GetVGCVerFail"));
                    return;
                }

                var verNew = new Version(version);
                var verCur = new Version(Properties.Resources.Version);

                var result = verCur.CompareTo(verNew);
                if (result >= 0)
                {
                    MessageBox.Show(I18N("NoNewVGC"));
                    return;
                }

                var confirmTpl = I18N("ConfirmDownloadNewVGC");
                var msg = string.Format(confirmTpl, version);
                if (Lib.UI.Confirm(msg))
                {
                    var tpl = resData("TplUrlVGCRelease");
                    var url = string.Format(tpl, version);
                    System.Diagnostics.Process.Start(url);
                }


            };
            // todo check update
            Task.Factory.StartNew(CheckUpdate);
        }

        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lib.UI.ShowAboutBox();
        }

        private void addVmessServToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Views.FormSimAddVmessClient.GetForm();
        }
    }
}
