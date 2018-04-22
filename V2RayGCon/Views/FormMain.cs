using System;
using System.Diagnostics;
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

            BindListViewEvents();

            UpdateUI();

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= UpdateUIHandler;
                core.OnCoreStatChange -= UpdateUIHandler;
            };

            Lib.UI.SetFormLocation<FormMain>(this, Model.Data.Enum.FormLocations.TopLeft);

            this.Show();
            setting.OnSettingChange += UpdateUIHandler;
            core.OnCoreStatChange += UpdateUIHandler;
        }

        void UpdateUIHandler(object s, EventArgs e)
        {
            UpdateElementDelegate updater =
                new UpdateElementDelegate(UpdateUI);

            lvServers?.Invoke(updater);
        }

        void BindListViewEvents()
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

            if (setting.proxyType == "http")
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

        public void UpdateListView()
        {
            lvServers.Items.Clear();

            var servers = formMainCtrl.GetServerInfo();
            if (servers.Count <= 0)
            {
                return;
            }

            foreach (var server in servers)
            {
                lvServers.Items.Add(new ListViewItem(server));

            }
        }

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
            setting.DeleteAllServers();
        }

        void UpdateUI()
        {
            UpdateListView();

            proxyAddrToolStripTextBox.Text = setting.proxyAddr;

            protocolSocksToolStripMenuItem.Checked = setting.proxyType.Equals("socks");
            protocolHttpStripMenuItem.Checked = setting.proxyType.Equals("http");
            protocolConfigToolStripMenuItem.Checked = setting.proxyType.Equals("config");

            var isSystemProxySet = Lib.ProxySetter.getProxyState();
            sysProxyDirectToolStripMenuItem.Checked = !isSystemProxySet;
            sysProxyHttpToolStripMenuItem.Checked = isSystemProxySet;

            var isCoreRunning = core.IsRunning();
            activateToolStripMenuItem.Enabled = !isCoreRunning;
            stopToolStripMenuItem.Enabled = isCoreRunning;

        }

        void SwitchToProtocal(string protocal)
        {
            setting.proxyType = protocal;
            ActivateServer();
            UpdateUI();
        }

        private void protocolHttpStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchToProtocal("http");
        }

        private void protocolSocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchToProtocal("socks");
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
            setting.curEditingIndex = GetSelectedServerIndex();
            // Views.FormConfiger.GetForm();
            new FormConfiger();
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
            // Views.FormConfiger.GetForm();
            new FormConfiger();
        }

        private void sysProxyHttpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lib.ProxySetter.setProxy(setting.proxyAddr, true);
            if (core.IsRunning() && setting.proxyType == "http")
            {
                UpdateUI();
            }
            else
            {
                setting.proxyType = "http";
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
            SwitchToProtocal("config");
        }
    }
}
