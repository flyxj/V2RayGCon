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

namespace V2RayGCon.Views
{
    public partial class FormMain : Form
    {
        Func<string, string> I18N;
        Service.Setting setting;
        Service.Core core;

        public event EventHandler OpenEditor, ShowQRCodeForm, ShowLogForm;
        delegate void UpdateElementDelegate();

        public FormMain()
        {

            setting = Service.Setting.Instance;
            core = Service.Core.Instance;

            InitializeComponent();
            InitServerListView();

            UpdateElement();

            I18N = Lib.Utils.I18N;

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= UpdateUI;
                core.OnCoreStatChange -= UpdateUI;
            };

            setting.OnSettingChange += UpdateUI;
            core.OnCoreStatChange += UpdateUI;

            this.Show();
        }

        void UpdateUI(object s, EventArgs e)
        {
            UpdateElementDelegate updateElement = new UpdateElementDelegate(UpdateElement);
            lvServers.Invoke(updateElement);
        }


        void InitServerListView()
        {
            lvServers.Items.Clear();

            lvServers.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (lvServers.FocusedItem.Bounds.Contains(e.Location) == true)
                    {
                        ctxMenuStrip.Show(Cursor.Position);
                    }
                }
            };

            lvServers.MouseDoubleClick += ActivateServer;
        }

        int GetSelectedItemIndex()
        {
            return Lib.Utils.Str2Int(lvServers.SelectedItems[0].Text) - 1;
        }

        void ActivateServer(object sender, MouseEventArgs e)
        {
            var index = GetSelectedItemIndex();
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
            int actServ = setting.GetSelectedServerIndex();
            string s, proxy;
            JObject o;
            foreach (var b64cfgStr in servers)
            {
                try
                {
                    s = Lib.Utils.Base64Decode(b64cfgStr);
                    o = JObject.Parse(s);

                }
                catch { continue; }

                string ip, port, type, tls, path, streamType;
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

                lvServers.Items.Add(new ListViewItem(new string[] {
                    (count+1).ToString(), //no.
                    proxy, // proxy
                    ip,  // ip
                    port,  // port
                    count == actServ?"√":"",  //active
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
            ActivateServer(null, null);
        }

        private void deleteStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Lib.Utils.Confirm(I18N("Confirm"), I18N("ConfirmDeleteServer")))
            {
                setting.DeleteServer(GetSelectedItemIndex());
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
            if (setting.proxyType.Equals("http"))
            {
                protocolSocksToolStripMenuItem.Checked = false;
                protocolHttpStripMenuItem.Checked = true;
            }
            if (setting.proxyType.Equals("socks"))
            {
                protocolSocksToolStripMenuItem.Checked = true;
                protocolHttpStripMenuItem.Checked = false;
            }

            if (core.IsRunning())
            {
                activateToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;
            }
            else
            {
                activateToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
            }
        }

        void ChangeProtocal(string protocal)
        {
            setting.proxyType = protocal;
            setting.ActivateServer(setting.GetSelectedServerIndex());
            UpdateElement();
        }

        private void protocolHttpStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeProtocal("http");
        }

        private void protocolSocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeProtocal("socks");
        }

        private void proxyAddrToolStripTextBox_TextChange(object sender, EventArgs e)
        {
            setting.proxyAddr = proxyAddrToolStripTextBox.Text;
            setting.ActivateServer(setting.GetSelectedServerIndex());
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.curEditingIndex = GetSelectedItemIndex();
            OpenEditor?.Invoke(this, new EventArgs());
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
            core.RestartCore();
        }

        private void vmessToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var b64cfgStrs = setting.GetAllServers();
            string s = string.Empty;

            foreach (var b64cfg in b64cfgStrs)
            {
                var cfg = Lib.Utils.Base64Decode(b64cfg);
                var vmess = Lib.Utils.ConfigString2Vmess(cfg);
                if (vmess == null)
                {
                    continue;
                }
                var vlink = Lib.Utils.Vmess2VmessLink(vmess);
                s += vlink + "\r\n";
            }

            Lib.Utils.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(s),
                I18N("LinksCopied"),
                I18N("CopyFail"));
        }

        private void v2rayToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var b64cfgStrs = setting.GetAllServers();
            string s = string.Empty;

            foreach (var b64cfg in b64cfgStrs)
            {
                s += "v2ray://" + b64cfg + "\r\n";
            }

            Lib.Utils.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(s),
                I18N("LinksCopied"),
                I18N("CopyFail"));
        }

        private void vmessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var b64cfgStr = setting.GetServer(GetSelectedItemIndex());
            Model.Vmess vmess = null;
            string vmessLink = string.Empty;

            if (!string.IsNullOrEmpty(b64cfgStr))
            {
                var cfg = Lib.Utils.Base64Decode(b64cfgStr);
                vmess = Lib.Utils.ConfigString2Vmess(cfg);
            }

            if (vmess != null)
            {
                vmessLink = Lib.Utils.Vmess2VmessLink(vmess);
            }

            Lib.Utils.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(vmessLink),
                I18N("LinksCopied"),
                I18N("CopyFail"));
        }

        private void v2rayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var b64cfgStr = setting.GetServer(GetSelectedItemIndex());
            string v2rayLink = string.Empty;

            if (!string.IsNullOrEmpty(b64cfgStr))
            {
                v2rayLink = Lib.Utils.LinkAddPerfix(b64cfgStr, Model.Enum.LinkTypes.v2ray);
            }

            Lib.Utils.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(v2rayLink),
                I18N("LinksCopied"),
                I18N("CopyFail"));

        }

        private void MoveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.MoveItemDown(GetSelectedItemIndex());
        }

        private void MoveToButtomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int idx = GetSelectedItemIndex();
            setting.MoveItemToButtom(idx);
        }

        private void MoveToTopStripMenuItem_Click(object sender, EventArgs e)
        {
            int idx = GetSelectedItemIndex();
            setting.MoveItemToTop(idx);
        }

        private void MoveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting.MoveItemUp(GetSelectedItemIndex());
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLogForm?.Invoke(this, null);
        }

        private void ShowQRCodeFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowQRCodeForm?.Invoke(this, null);
        }

        private void ShowConfigEditorFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenEditor?.Invoke(this, null);
        }
    }
}
