using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormMain : Form
    {
        Service.Setting setting;
        Service.Core core;

        public event EventHandler ShowFormConfiger, ShowFormQRCode, ShowFormLog;
        delegate void UpdateElementDelegate();

        public FormMain()
        {

            setting = Service.Setting.Instance;
            core = Service.Core.Instance;

            InitializeComponent();
            InitListViewServers();

            UpdateElement();

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= UpdateUI;
                core.OnCoreStatChange -= UpdateUI;
            };

            this.Show();
            setting.OnSettingChange += UpdateUI;
            core.OnCoreStatChange += UpdateUI;
        }

        void UpdateUI(object s, EventArgs e)
        {
            UpdateElementDelegate updateElement =
                new UpdateElementDelegate(UpdateElement);

            lvServers.Invoke(updateElement);
        }

        void InitListViewServers()
        {
            lvServers.Items.Clear();

            lvServers.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right
                && lvServers.FocusedItem.Bounds.Contains(e.Location))
                {
                    ctxMenuStrip.Show(Cursor.Position);
                }
            };

            lvServers.MouseDoubleClick += (s, a) => ActivateServer();
        }

        int GetSelectedServerIndex()
        {
            return Lib.Utils.Str2Int(lvServers.SelectedItems[0].Text) - 1;
        }

        void ActivateServer()
        {
            var index = GetSelectedServerIndex();
            Debug.WriteLine("FormMain: activate server " + index);
            setting.ActivateServer(index);
        }

        public void RefreshServerListView()
        {
            lvServers.Items.Clear();

            var servers = setting.GetAllServers();

            if (servers.Count <= 0)
            {
                Debug.WriteLine("FormMain: servers is empty!");
                return;
            }

            int count = 0;
            int selectedServer = setting.GetSelectedServerIndex();
            string s, proxy;
            JObject o;
            foreach (var server in servers)
            {
                try
                {
                    s = Lib.Utils.Base64Decode(server);
                    o = JObject.Parse(s);

                }
                catch { continue; }

                string ip, port, type, tls, path, streamType, alias;
                proxy = Lib.Utils.GetStrFromJToken(o, "outbound.protocol");
                if (proxy.Equals("shadowsocks"))
                {
                    ip = Lib.Utils.GetStrFromJToken(o, "outbound.settings.servers.0.address");
                    port = Lib.Utils.GetStrFromJToken(o, "outbound.settings.servers.0.port");
                }
                else
                {
                    ip = Lib.Utils.GetStrFromJToken(o, "outbound.settings.vnext.0.address");
                    port = Lib.Utils.GetStrFromJToken(o, "outbound.settings.vnext.0.port");
                }

                streamType = Lib.Utils.GetStrFromJToken(o, "outbound.streamSettings.network");
                type = Lib.Utils.GetStrFromJToken(o, "outbound.streamSettings.kcpSettings.header.type");
                path = Lib.Utils.GetStrFromJToken(o, "outbound.streamSettings.wsSettings.path");
                tls = Lib.Utils.GetStrFromJToken(o, "outbound.streamSettings.security");
                alias = Lib.Utils.GetStrFromJToken(o, "v2raygcon.alias");

                lvServers.Items.Add(new ListViewItem(new string[] {
                    (count+1).ToString(), //no.
                    alias,
                    proxy,
                    ip,
                    port,
                    count == selectedServer?"√":"",  //active
                    path, // Url
                    streamType, //protocol
                    tls, //encryption
                    type // disguise
                }));
                count++;
            }
        }

        private void activateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActivateServer();
        }

        private void deleteStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Lib.Utils.Confirm(I18N("ConfirmDeleteServer")))
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

        void UpdateElement()
        {
            RefreshServerListView();

            proxyAddrToolStripTextBox.Text = setting.proxyAddr;

            var isSocksProxy = setting.proxyType.Equals("socks");
            protocolSocksToolStripMenuItem.Checked = isSocksProxy;
            protocolHttpStripMenuItem.Checked = !isSocksProxy;

            var isCoreRunning = core.IsRunning();
            activateToolStripMenuItem.Enabled = !isCoreRunning;
            stopToolStripMenuItem.Enabled = isCoreRunning;

        }

        void SwitchToProtocal(string protocal)
        {
            setting.proxyType = protocal;
            setting.ActivateServer(setting.GetSelectedServerIndex());
            UpdateElement();
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
                setting.ActivateServer(setting.GetSelectedServerIndex());
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.curEditingIndex = GetSelectedServerIndex();
            ShowFormConfiger?.Invoke(this, new EventArgs());
        }

        private void activateToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            setting.ActivateServer(setting.GetSelectedServerIndex());
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            core.StopCore();
        }

        private void ImportLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string links = Lib.Utils.GetClipboardText();

            Lib.Utils.ShowMsgboxSuccFail(
                setting.ImportLinks(links),
                I18N("ImportLinkSuccess"),
                I18N("ImportLinkFail"));
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.ActivateServer(setting.GetSelectedServerIndex());
        }

        private void CopyAllVmessLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var servers = setting.GetAllServers();
            string s = string.Empty;

            foreach (var server in servers)
            {
                var config = Lib.Utils.Base64Decode(server);
                var vmess = Lib.Utils.ConfigString2Vmess(config);
                var vmessLink = Lib.Utils.Vmess2VmessLink(vmess);

                if (!string.IsNullOrEmpty(vmessLink))
                {
                    s += vmessLink + "\r\n";
                }
            }

            Lib.Utils.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(s),
                I18N("LinksCopied"),
                I18N("CopyFail"));
        }

        private void CopyAllV2RayLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var servers = setting.GetAllServers();
            string s = string.Empty;

            foreach (var server in servers)
            {
                s += "v2ray://" + server + "\r\n";
            }

            Lib.Utils.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(s),
                I18N("LinksCopied"),
                I18N("CopyFail"));
        }

        private void CopyVmessLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var server = setting.GetServer(GetSelectedServerIndex());
            string vmessLink = string.Empty;

            if (!string.IsNullOrEmpty(server))
            {
                var config = Lib.Utils.Base64Decode(server);
                var vmess = Lib.Utils.ConfigString2Vmess(config);
                vmessLink = Lib.Utils.Vmess2VmessLink(vmess);
            }

            Lib.Utils.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(vmessLink),
                I18N("LinksCopied"),
                I18N("CopyFail"));
        }

        private void CopyV2RayLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var server = setting.GetServer(GetSelectedServerIndex());
            string v2rayLink = string.Empty;

            if (!string.IsNullOrEmpty(server))
            {
                v2rayLink = Lib.Utils.LinkAddPerfix(server, Model.Enum.LinkTypes.v2ray);
            }

            Lib.Utils.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(v2rayLink),
                I18N("LinksCopied"),
                I18N("CopyFail"));

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
            ShowFormLog?.Invoke(this, null);
        }

        private void ShowFormQRCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormQRCode?.Invoke(this, null);
        }

        private void ShowFormConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormConfiger?.Invoke(this, null);
        }
    }
}
