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
        ContextMenu menu;
        bool isRunning;

        public ServerListItem(int index, Model.Data.ServerItem serverItem)
        {
            this.menu = CreateMenu();
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
                    chkSelected, serverItem.isSelected);

                Lib.UI.UpdateControlOnDemand(
                    chkImport, serverItem.isInjectImport);

                ShowOnOffStatus(serverItem.server.isRunning);
            });
        }

        ContextMenu CreateMenu()
        {
            return new ContextMenu(new MenuItem[] {
                new MenuItem(I18N("Log"),(s,a)=>serverItem.ShowLogForm()),
                new MenuItem(I18N("Edit"),(s,a)=>{
                    var item=this.serverItem;
                    var config=item.config;
                    new Views.FormConfiger(this.serverItem.config);
                }),
                new MenuItem(I18N("Delete"),(s,a)=>{
                    if (!Lib.UI.Confirm(I18N("ConfirmDeleteControl")))
                    {
                        return;
                    }
                    Cleanup();
                    serverItem.DeleteSelf();
                }),
                new MenuItem("-"),
                new MenuItem(I18N("Copy"),new MenuItem[]{
                    new MenuItem(I18N("VmessLink"),(s,a)=>{
                        MessageBox.Show(
                            Lib.Utils.CopyToClipboard(
                                Lib.Utils.Vmess2VmessLink(
                                    Lib.Utils.ConfigString2Vmess(
                                        this.serverItem.config)))?
                            I18N("LinksCopied") :
                            I18N("CopyFail"));
                    }),
                    new MenuItem(I18N("V2RayLink"),(s,a)=>{
                        MessageBox.Show(
                            Lib.Utils.CopyToClipboard(
                                Lib.Utils.AddLinkPrefix(
                                    Lib.Utils.Base64Encode(this.serverItem.config),
                                    Model.Data.Enum.LinkTypes.v2ray)) ?
                            I18N("LinksCopied") :
                            I18N("CopyFail"));
                    }),
                }),
                new MenuItem(I18N("SpeedTest"),(s,a)=>{
                    Task.Factory.StartNew(
                        () => serverItem.DoSpeedTest());
                }),
                new MenuItem(I18N("SetAsSysProxy"),(s,a)=>{
                    if (cboxInbound.SelectedIndex != (int)Model.Data.Enum.ProxyTypes.HTTP)
                    {
                        MessageBox.Show(I18N("SysProxyRequireHTTPServer"));
                        return;
                    }

                    Service.Setting.Instance.SetSystemProxy(
                        string.Format("{0}:{1}",
                        this.tboxInboundIP.Text,
                        this.tboxInboundPort.Text));

                    MessageBox.Show(I18N("Done"));
                }),
            });
        }

        private void ShowOnOffStatus(bool isServerOn)
        {
            if (this.isRunning == isServerOn)
            {
                return;
            }

            this.isRunning = isServerOn;

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
            menu.Show(this, pos);
        }

        private void cboxInbound_SelectedIndexChanged(object sender, EventArgs e)
        {
            serverItem.SetOverwriteInboundType(cboxInbound.SelectedIndex);
        }

        private void chkImport_CheckedChanged(object sender, EventArgs e)
        {
            serverItem.SetInjectImport(chkImport.Checked);
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
            serverItem.RestartCoreThen();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            serverItem.StopCoreThen();
        }
        #endregion
    }
}
