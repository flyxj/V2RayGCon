using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormQRCode : Form
    {
        #region Sigleton
        static FormQRCode _instant;
        public static FormQRCode GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormQRCode();
            }
            return _instant;
        }
        #endregion

        Service.Setting setting;
        int servIndex, linkType;

        FormQRCode()
        {
            setting = Service.Setting.Instance;
            servIndex = 0;
            linkType = 0;

            InitializeComponent();
            this.Show();
        }

        private void FormQRCode_Shown(object sender, EventArgs e)
        {
            UpdateServerList();
            cboxLinkType.SelectedIndex = linkType;

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= SettingChange;
            };

            setting.OnSettingChange += SettingChange;
        }

        #region public methods
        void SettingChange(object sender, EventArgs args)
        {
            try
            {
                cboxServList.Invoke((MethodInvoker)delegate
                {
                    UpdateServerList();
                });
            }
            catch { }
        }

        void UpdateServerList(int index = -1)
        {
            var oldIndex = index < 0 ? cboxServList.SelectedIndex : index;

            cboxServList.Items.Clear();

            var aliases = setting.GetAllAliases();

            if (aliases.Count <= 0)
            {
                return;
            }

            foreach (var alias in aliases)
            {
                cboxServList.Items.Add(alias);
            }

            servIndex = Lib.Utils.Clamp(oldIndex, 0, aliases.Count);
            cboxServList.SelectedIndex = servIndex;
            UpdateLink();
        }

        void UpdateLink()
        {
            var server = setting.GetServer(servIndex);

            if (string.IsNullOrEmpty(server))
            {
                tboxLink.Text = string.Empty;
            }

            string link = string.Empty;

            if (linkType == 0)
            {
                link = Lib.Utils.Vmess2VmessLink(
                    Lib.Utils.ConfigString2Vmess(
                        Lib.Utils.Base64Decode(server)));
            }
            else
            {
                link = Lib.Utils.AddLinkPrefix(
                    server,
                    Model.Data.Enum.LinkTypes.v2ray);
            }

            tboxLink.Text = link;
        }

        void ShowQRCode()
        {
            picQRCode.InitialImage = null;

            var link = tboxLink.Text;

            if (string.IsNullOrEmpty(link))
            {
                return;
            }

            Tuple<Bitmap, Lib.QRCode.QRCode.WriteErrors> pair =
                Lib.QRCode.QRCode.GenQRCode(
                    link, linkType == 0 ? 180 : 320);

            switch (pair.Item2)
            {
                case Lib.QRCode.QRCode.WriteErrors.Success:
                    picQRCode.Image = pair.Item1;
                    break;
                case Lib.QRCode.QRCode.WriteErrors.DataEmpty:
                    picQRCode.Image = null;
                    MessageBox.Show(I18N("EmptyLink"));
                    break;
                case Lib.QRCode.QRCode.WriteErrors.DataTooBig:
                    picQRCode.Image = null;
                    MessageBox.Show(I18N("DataTooBig"));
                    break;
            }
        }
        #endregion

        #region UI event handler
        private void cboxLinkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            linkType = cboxLinkType.SelectedIndex;
            UpdateLink();
        }

        private void btnSavePic_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = resData("ExtPng"),
                FilterIndex = 1,
                RestoreDirectory = true,
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    picQRCode.Image.Save(myStream, System.Drawing.Imaging.ImageFormat.Png);
                    myStream.Close();
                }
            }
        }

        private void tboxLink_TextChanged(object sender, EventArgs e)
        {
            ShowQRCode();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                Lib.Utils.CopyToClipboard(tboxLink.Text) ?
                I18N("CopySuccess") :
                I18N("CopyFail"));
        }

        private void cboxServList_SelectedIndexChanged(object sender, EventArgs e)
        {
            servIndex = cboxServList.SelectedIndex;
            UpdateLink();
        }
        #endregion
    }
}
