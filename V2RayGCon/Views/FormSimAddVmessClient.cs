using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormSimAddVmessClient : Form
    {

        #region Sigleton
        static FormSimAddVmessClient _instant;
        public static FormSimAddVmessClient GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormSimAddVmessClient();
            }
            return _instant;
        }
        #endregion

        Service.Setting setting;

        FormSimAddVmessClient()
        {
            InitializeComponent();
            Fill(cboxKCP, Model.Data.Table.kcpTypes);
            setting = Service.Setting.Instance;
            this.Show();
        }

        void Fill(ComboBox cbox, Dictionary<int, string> table)
        {
            cbox.Items.Clear();
            foreach (var item in table)
            {
                cbox.Items.Add(item.Value);
            }
            cbox.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            var vmess = new Model.Data.Vmess();

            vmess.add = tboxHost.Text;
            vmess.port = tboxPort.Text;
            vmess.aid = tboxAID.Text;
            vmess.id = tboxUID.Text;
            vmess.ps = tboxAlias.Text;

            if (rbtnWS.Checked)
            {
                vmess.net = "ws";
                vmess.host = tboxWSPath.Text;
            }

            if (rbtnKCP.Checked)
            {
                vmess.net = "kcp";
                var index = Math.Max(0, cboxKCP.SelectedIndex);
                vmess.type = Model.Data.Table.kcpTypes[index];
            }

            if (rbtnTCP.Checked)
            {
                vmess.net = "tcp";
            }

            if (chkboxTLS.Checked)
            {
                vmess.tls = "tls";
            }

            var link = Lib.Utils.Vmess2VmessLink(vmess);

            if (setting.ImportLinks(link))
            {
                MessageBox.Show(I18N("AddServSuccess"));
                this.Close();
            }
            else
            {
                MessageBox.Show(I18N("AddServFail"));
            }

        }
    }
}
