using System;
using System.Drawing;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.UserControls
{
    public partial class ServerListItem : UserControl
    {
        Model.Data.ServerItem serverItem;
        ContextMenu menu;
        int preIndex;

        public ServerListItem(int index, Model.Data.ServerItem serverItem)
        {
            menu = CreateMenu();
            InitializeComponent();
            this.serverItem = serverItem;
            SetIndex(index);
        }

        private void ServerListItem_Load(object sender, System.EventArgs e)
        {
            RefreshUI(this, EventArgs.Empty);
            preIndex = cboxInbound.SelectedIndex;
            this.serverItem.OnPropertyChanged += RefreshUI;
        }


        #region private method
        void RefreshUI(object sender, EventArgs arg)
        {
            lbSummary.Invoke((MethodInvoker)delegate
            {
                lbSummary.Text = serverItem.summary;
                SetRunning(serverItem.server.isRunning);
                tboxInboundIP.Text = serverItem.inboundIP;
                tboxInboundPort.Text = serverItem.inboundPort.ToString();
                cboxInbound.SelectedIndex = serverItem.inboundOverwriteType;
                chkEnv.Checked = serverItem.isInjectEnv;
                chkImport.Checked = serverItem.isInjectImport;
            });
        }

        ContextMenu CreateMenu()
        {
            return new ContextMenu(new MenuItem[] {
                new MenuItem(I18N("Start"),(s,a)=>MessageBox.Show("hello")),
                new MenuItem(I18N("Stop"),(s,a)=>MessageBox.Show("hello")),
                new MenuItem("-"),
                new MenuItem(I18N("Edit"),(s,a)=>{
                    var item=this.serverItem;
                    var config=item.config;
                    new Views.FormConfiger(this.serverItem.config);
                }),
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
            });
        }

        private void SetRunning(bool isRunning)
        {
            if (isRunning)
            {
                lbRunning.ForeColor = Color.Green;
                lbRunning.Text = "ON";
            }
            else
            {
                lbRunning.ForeColor = Color.DarkOrange;
                lbRunning.Text = "OFF";
            }
        }
        #endregion

        #region public method

        public void SetIndex(int index)
        {
            this.lbIndex.Text = index.ToString();
            this.serverItem.SetIndex(index);
        }

        public void Cleanup()
        {
            this.serverItem.OnPropertyChanged -= RefreshUI;
            this.serverItem = null;
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
            if (preIndex == cboxInbound.SelectedIndex)
            {
                return;
            }

            var index = cboxInbound.SelectedIndex;
            serverItem.SetInboundType(index);
            preIndex = index;
        }

        private void chkEnv_CheckedChanged(object sender, EventArgs e)
        {
            var check = chkEnv.Checked;
            if (serverItem.isInjectEnv != check)
            {
                serverItem.SetInjectEnv(check);
            }
        }

        private void chkImport_CheckedChanged(object sender, EventArgs e)
        {
            var check = chkImport.Checked;
            if (serverItem.isInjectImport != check)
            {
                serverItem.SetInjectImport(check);
            }
        }

        private void tboxInboundIP_TextChanged(object sender, EventArgs e)
        {
            var txt = tboxInboundIP.Text;
            if (txt == serverItem.inboundIP)
            {
                return;
            }
            serverItem.SetInboundIP(txt);
        }

        private void tboxInboundPort_TextChanged(object sender, EventArgs e)
        {
            var txt = tboxInboundPort.Text;
            if (txt == serverItem.inboundPort.ToString())
            {
                return;
            }
            serverItem.SetInboundPort(txt);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!Lib.UI.Confirm(I18N("ConfirmDeleteControl")))
            {
                return;
            }

            serverItem.Delete();
        }

        #endregion
    }
}
