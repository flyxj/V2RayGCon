using System;
using System.Diagnostics;
using System.IO;
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

        FormMain()
        {
            setting = Service.Setting.Instance;
            core = Service.Core.Instance;

            InitializeComponent();
            this.Show();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
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

            setting.OnSettingChange += SettingChangeHandler;
            core.OnCoreStatChange += SettingChangeHandler;
        }

        #region private method
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.RefreshSummaries();
        }

        void SettingChangeHandler(object s, EventArgs e)
        {
            try
            {
                lvServers.Invoke((MethodInvoker)delegate
                {
                    UpdateUI();
                });
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

            if (setting.isSysProxyHasSet)
            {
                if (setting.proxyType == (int)Model.Data.Enum.ProxyTypes.http)
                {
                    Lib.ProxySetter.setProxy(setting.proxyAddr, true);
                }
                else
                {
                    // e.g. http->socks should clear system proxy setting
                    Lib.ProxySetter.setProxy("", false);
                    setting.isSysProxyHasSet = false;
                }
            }

            setting.ActivateServer(index);
        }

        void UpdateUI()
        {
            // update list view
            lvServers.Items.Clear();
            var servers = setting.GetAllServersSummary();
            var curServIndex = setting.GetCurServIndex();
            var curServNum = (curServIndex + 1).ToString();

            foreach (var server in servers)
            {
                server[5] =
                    server[0] == curServNum
                    && core.isRunning ?
                    "√" : string.Empty;

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


            sysProxyHttpToolStripMenuItem.Checked = setting.isSysProxyHasSet;

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
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Views.FormOption.GetForm();
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
            if (setting.proxyAddr != proxyAddrToolStripTextBox.Text)
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
            core.StopCoreThen(null);
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
            setting.isSysProxyHasSet = true;
            Lib.ProxySetter.setProxy(setting.proxyAddr, true);

            var http = (int)Model.Data.Enum.ProxyTypes.http;
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
            setting.isSysProxyHasSet = false;
            Lib.ProxySetter.setProxy("", false);
            UpdateUI();
        }

        private void protocolConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwitchToProtocal(Model.Data.Enum.ProxyTypes.config);
        }

        private void addVmessServToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Views.FormSimAddVmessClient.GetForm();
        }

        private void downloadV2rayCoreToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormDownloadCore.GetForm();
        }

        private void checkUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(formMainCtrl.CheckUpdate);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lib.UI.VisitUrl(I18N("VistPorjectPage"), Properties.Resources.ProjectLink);
        }

        private void exportAllServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMainCtrl.ExportAllServersToTextFile();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMainCtrl.ImportServersFromTextFile();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lib.UI.VisitUrl(I18N("VistWikiPage"), Properties.Resources.WikiLink);
        }

        private void configTesterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormConfigTester();
        }

        private void removeV2rayCoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Lib.UI.Confirm(I18N("ConfirmRemoveV2RayCore")))
            {
                return;
            }

            if (!Directory.Exists(Lib.Utils.GetAppDataFolder()))
            {
                MessageBox.Show(I18N("Done"));
                return;
            }

            core.StopCoreThen(() =>
            {
                try
                {
                    Lib.Utils.DeleteAppDataFolder();
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show(I18N("FileInUse"));
                    return;
                }
                MessageBox.Show(I18N("Done"));
            });
        }

        #endregion


    }
}
