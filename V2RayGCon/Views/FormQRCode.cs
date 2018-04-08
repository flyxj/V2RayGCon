using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace V2RayGCon.Views
{
    public partial class FormQRCode : Form
    {

        Service.Setting setting;
        Func<string, string> resData;
        List<string> servers;
        int index, linkType;
        Point formSize;

        public FormQRCode()
        {
            InitializeComponent();
            setting = Service.Setting.Instance;
            resData = Lib.Utils.resData;
            formSize = new Point(this.Width, this.Height);

            UpdateServList();
            this.Show();
        }

        void UpdateServList()
        {
            index = 0;
            linkType = 0;
            servers = setting.GetAllServers();
            cboxServList.Items.Clear();
            int count = 1;
            foreach (var s in servers)
            {
                cboxServList.Items.Add(count++);
            }
            if (cboxServList.Items.Count > 0)
            {
                cboxServList.SelectedIndex = 0;
            }
            cboxLinkType.SelectedIndex = 0;

        }

        void ShowQRCode()
        {
            picQRCode.InitialImage = null;

            if (servers.Count <= 0)
            {
                return;
            }

            var server = servers[index];

            if (!string.IsNullOrEmpty(server))
            {
                string link = string.Empty;
                if (linkType == 0)
                {
                    string configString = Lib.Utils.Base64Decode(server);
                    var vmess = Lib.Utils.ConfigString2Vmess(configString);
                    link = Lib.Utils.Vmess2VmessLink(vmess);
                    if (!string.IsNullOrEmpty(link))
                    {
                        picQRCode.Image = Lib.QRCode.QRCode.GenQRCode(link,180);
                    }
                }
                else
                {
                    link = resData("V2RayLinkPerfix") + server;
                    picQRCode.Image = Lib.QRCode.QRCode.GenQRCode(link,320);
                }
            }
        }

        private void cboxLinkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            linkType = Lib.Utils.Clamp(cboxLinkType.SelectedIndex, 0, 1);
            ShowQRCode();
            int delta;
            // delta = linkType == 0 ? 0 : 320 - 320;
            delta = 0;
            this.Width = formSize.X + delta;
            this.Height = formSize.Y + delta;
        }

        private void btnSavePic_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = resData("ExtPng");
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    picQRCode.Image.Save(myStream, System.Drawing.Imaging.ImageFormat.Png);
                    myStream.Close();
                }
            }

        }

        private void cboxServList_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = Lib.Utils.Clamp(cboxServList.SelectedIndex, 0, servers.Count);
            ShowQRCode();
        }
    }
}
