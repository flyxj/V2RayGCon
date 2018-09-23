using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V2RayGCon.Views
{
    public partial class FormBatchModifyServerSetting : Form
    {
        #region Sigleton
        static FormBatchModifyServerSetting _instant;
        public static FormBatchModifyServerSetting GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormBatchModifyServerSetting();
            }
            return _instant;
        }
        #endregion

        Service.Servers servers;

        FormBatchModifyServerSetting()
        {
            servers = Service.Servers.Instance;

            InitializeComponent();

#if DEBUG
            this.Icon = Properties.Resources.icon_light;
#else
            this.Icon = Properties.Resources.icon_dark;
#endif
            this.Show();
        }

        private void FormBatchModifyServerInfo_Shown(object sender, EventArgs e)
        {
            this.cboxMark.Items.Clear();
            foreach (var mark in servers.GetMarkList())
            {
                this.cboxMark.Items.Add(mark);
            }

            var first = servers.GetServerList().Where(s => s.isSelected).FirstOrDefault();
            if (first == null)
            {
                return;
            }
            this.cboxInMode.SelectedIndex = first.overwriteInboundType;
            this.tboxInIP.Text = first.inboundIP;
            this.tboxInPort.Text = first.inboundPort.ToString();
            this.cboxMark.Text = first.mark;
            this.cboxAutorun.SelectedIndex = first.isAutoRun ? 0 : 1;
            this.cboxImport.SelectedIndex = first.isInjectImport ? 0 : 1;
        }

        #region UI event

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            var list = servers.GetServerList().Where(s => s.isSelected).ToList();
            var newMode = chkInMode.Checked ? cboxInMode.SelectedIndex : -1;
            var newIP = chkInIP.Checked ? tboxInIP.Text : null;
            var newPort = chkInPort.Checked ? Lib.Utils.Str2Int(tboxInPort.Text) : -1;
            var newMark = chkMark.Checked ? cboxMark.Text : null;
            var newAutorun = chkAutorun.Checked ? cboxAutorun.SelectedIndex : -1;
            var newImport = chkImport.Checked ? cboxImport.SelectedIndex : -1;

            ModifyServersSetting(
                list,
                newMode, newIP, newPort,
                newMark, newAutorun, newImport);
        }

        #endregion

        #region private method
        void ModifyServersSetting(
            List<Model.Data.ServerItem> list,
            int newMode, string newIP, int newPort,
            string newMark, int newAutorun, int newImport)
        {
            Action<int, Action> worker = (index, next) =>
            {
                var server = list[index];
                if (!server.isServerOn)
                {
                    ModifyServerSetting(
                        ref server,
                        newMode, newIP, newPort,
                        newMark, newAutorun, newImport);
                    server.InvokeEventOnPropertyChange();
                    next();
                    return;
                }

                server.StopCoreThen(() =>
                {
                    ModifyServerSetting(
                        ref server, 
                        newMode, newIP, newPort,
                        newMark, newAutorun, newImport);
                    server.RestartCoreThen();
                    next();
                });
            };

            var that = this;
            Action done = () =>
            {
                servers.UpdateMarkList();
                btnModify.Invoke((MethodInvoker)delegate
                {
                    that.Close();
                });
            };

            Lib.Utils.ChainActionHelperAsync(list.Count, worker, done);

        }

        void ModifyServerSetting(
            ref Model.Data.ServerItem server,
            int newMode,            string newIP,            int newPort,
            string newMark, int newAutorun, int newImport)
        {
            if (newAutorun >= 0)
            {
                server.isAutoRun = newAutorun == 0;
            }

            if (newImport >= 0)
            {
                server.isInjectImport = newImport == 0;
            }

            if (newMode >= 0)
            {
                server.overwriteInboundType = newMode;
            }

            if (newIP != null)
            {
                server.inboundIP = newIP;
            }
            if (newPort >= 0)
            {
                server.inboundPort = newPort;
            }

            if (newMark != null)
            {
                server.mark = newMark;
            }
        }
    }
    #endregion
}
