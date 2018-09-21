using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.UserControls
{
    public partial class ServerListItem : UserControl, Model.BaseClass.IFormMainFlyPanelComponent
    {
        Model.Data.ServerItem serverItem;
        bool isRunning;

        public ServerListItem(int index, Model.Data.ServerItem serverItem)
        {
            this.serverItem = serverItem;
            SetIndex(index);
            InitializeComponent();
        }

        private void ServerListItem_Load(object sender, EventArgs e)
        {
            isRunning = !serverItem.isServerOn;
            SetStatusThen(string.Empty);
            RefreshUI(this, EventArgs.Empty);
            this.serverItem.OnPropertyChanged += RefreshUI;
        }

        #region private method
        void RefreshUI(object sender, EventArgs arg)
        {
            lbSummary.Invoke((MethodInvoker)delegate
            {
                Lib.UI.UpdateControlOnDemand(
                    cboxInbound, serverItem.overwriteInboundType);

                Lib.UI.UpdateControlOnDemand(
                    lbIndex, serverItem.index.ToString());

                Lib.UI.UpdateControlOnDemand(
                    lbSummary, serverItem.summary);

                Lib.UI.UpdateControlOnDemand(
                    tboxInboundIP, serverItem.inboundIP);

                Lib.UI.UpdateControlOnDemand(
                    tboxInboundPort, serverItem.inboundPort.ToString());

                Lib.UI.UpdateControlOnDemand(
                    chkAutoRun, serverItem.isAutoRun);

                Lib.UI.UpdateControlOnDemand(
                    lbStatus, serverItem.status);

                Lib.UI.UpdateControlOnDemand(
                    chkImport, serverItem.isInjectImport);

                UpdateChkSelected();
                ShowOnOffStatus(serverItem.server.isRunning);
                UpdateServerMark();
            });
        }

        void UpdateServerMark()
        {
            if (cboxMark.Text == serverItem.mark)
            {
                return;
            }

            cboxMark.Text = serverItem.mark;
        }

        void UpdateChkSelected()
        {
            if (serverItem.isSelected == chkSelected.Checked)
            {
                return;
            }

            chkSelected.Checked = serverItem.isSelected;
            HighlightSelectedServerItem(chkSelected.Checked);
        }

        void HighlightSelectedServerItem(bool selected)
        {
            var fontStyle = new Font(lbSummary.Font, selected ? FontStyle.Bold : FontStyle.Regular);
            var colorRed = selected ? Color.OrangeRed : Color.Black;
            lbSummary.Font = fontStyle;
            lbStatus.Font = fontStyle;
            lbStatus.ForeColor = colorRed;
        }

        private void ShowOnOffStatus(bool isServerOn)
        {
            if (this.isRunning == isServerOn)
            {
                return;
            }

            this.isRunning = isServerOn;

            tboxInboundPort.ReadOnly = this.isRunning;
            tboxInboundIP.ReadOnly = this.isRunning;

            if (isServerOn)
            {
                lbRunning.ForeColor = Color.DarkOrange;
                lbRunning.Text = "ON";
            }
            else
            {
                lbRunning.ForeColor = Color.Green;
                lbRunning.Text = "OFF";
            }
        }
        #endregion

        #region public method
        public string GetConfig()
        {
            return serverItem.config;
        }

        public bool GetAutorunStatus()
        {
            return serverItem.isAutoRun;
        }

        public bool GetSelectStatus()
        {
            return chkSelected.Checked;
        }

        public void SetStatusThen(string status, Action next = null)
        {
            if (lbStatus.IsDisposed)
            {
                next?.Invoke();
                return;
            }

            try
            {
                lbStatus?.Invoke((MethodInvoker)delegate
                {
                    Lib.UI.UpdateControlOnDemand(lbStatus, status);
                });
            }
            catch { }
            next?.Invoke();
        }

        public void SetSelected(bool selected)
        {
            serverItem.SetPropertyOnDemand(
               ref serverItem.isSelected,
               selected);
        }

        public void SetIndex(int index)
        {
            serverItem.SetPropertyOnDemand(
              ref serverItem.index,
              index);
        }

        public void Cleanup()
        {
            this.serverItem.OnPropertyChanged -= RefreshUI;
        }
        #endregion

        #region UI event
        private void ServerListItem_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Lib.UI.CreateCursorIconFromUserControl(this);
            DoDragDrop((ServerListItem)sender, DragDropEffects.Move);
        }

        private void btnAction_Click(object sender, System.EventArgs e)
        {
            Button btnSender = sender as Button;
            Point pos = new Point(btnSender.Left, btnSender.Top + btnSender.Height);
            ctxMenuStripMore.Show(this, pos);
            //menu.Show(this, pos);
        }

        private void cboxInbound_SelectedIndexChanged(object sender, EventArgs e)
        {
            serverItem.SetOverwriteInboundType(cboxInbound.SelectedIndex);
        }

        private void chkImport_CheckedChanged(object sender, EventArgs e)
        {
            serverItem.SetIsInjectImport(chkImport.Checked);
        }

        private void chkAutoRun_CheckedChanged(object sender, EventArgs e)
        {
            serverItem.SetPropertyOnDemand(
              ref serverItem.isAutoRun,
              chkAutoRun.Checked);
        }

        private void chkSelected_CheckedChanged(object sender, EventArgs e)
        {
            serverItem.SetPropertyOnDemand(
               ref serverItem.isSelected,
               chkSelected.Checked);
            HighlightSelectedServerItem(chkSelected.Checked);
        }

        private void tboxInboundIP_TextChanged(object sender, EventArgs e)
        {
            serverItem.SetPropertyOnDemand(
                ref serverItem.inboundIP,
                tboxInboundIP.Text,
                true);
        }

        private void tboxInboundPort_TextChanged(object sender, EventArgs e)
        {
            serverItem.SetPropertyOnDemand(
                ref serverItem.inboundPort,
                Lib.Utils.Str2Int(tboxInboundPort.Text),
                true);
        }

        private void lbSummary_Click(object sender, EventArgs e)
        {
            chkSelected.Checked = !chkSelected.Checked;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var server = this.serverItem;
            server.parent.StopAllServersThen(
                () => server.RestartCoreThen());
        }

        private void multiboxingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverItem.RestartCoreThen();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = this.serverItem;
            var config = item.config;
            new Views.FormConfiger(this.serverItem.config);
        }

        private void vmessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                           Lib.Utils.CopyToClipboard(
                               Lib.Utils.Vmess2VmessLink(
                                   Lib.Utils.ConfigString2Vmess(
                                       this.serverItem.config))) ?
                           I18N("LinksCopied") :
                           I18N("CopyFail"));
        }

        private void v2rayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                           Lib.Utils.CopyToClipboard(
                               Lib.Utils.AddLinkPrefix(
                                   Lib.Utils.Base64Encode(this.serverItem.config),
                                   Model.Data.Enum.LinkTypes.v2ray)) ?
                           I18N("LinksCopied") :
                           I18N("CopyFail"));
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Lib.UI.Confirm(I18N("ConfirmDeleteControl")))
            {
                return;
            }
            Cleanup();
            serverItem.parent.DeleteServerByConfig(serverItem.config);

        }

        private void speedTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(
                        () => serverItem.RunSpeedTest());
        }

        private void logOfThisServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverItem.ShowLogForm();
        }

        private void setAsSystemProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cboxInbound.SelectedIndex != (int)Model.Data.Enum.ProxyTypes.HTTP)
            {
                MessageBox.Show(I18N("SysProxyRequireHTTPServer"));
                return;
            }

            Service.Setting.Instance.SetSystemProxy(
                string.Format("{0}:{1}",
                this.tboxInboundIP.Text,
                this.tboxInboundPort.Text));

            // issue #9
            MessageBox.Show(I18N("SetSysProxyDone"));
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverItem.StopCoreThen();
        }

        private void cboxMark_TextChanged(object sender, EventArgs e)
        {
            this.serverItem.SetMark(cboxMark.Text);
        }

        private void cboxMark_DropDown(object sender, EventArgs e)
        {
            var servers = Service.Servers.Instance;
            cboxMark.Items.Clear();
            foreach (var item in servers.GetMarkList())
            {
                cboxMark.Items.Add(item);
            }
        }
        #endregion
    }
}
