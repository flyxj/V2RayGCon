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
            ShowQRCode();
        }

        void ShowQRCode()
        {
            picQRCode.InitialImage = null;

            var server = setting.GetServer(servIndex);
            if (string.IsNullOrEmpty(server))
            {
                return;
            }

            string link = string.Empty;
            Tuple<Bitmap, Lib.QRCode.QRCode.WriteErrors> pair;

            if (linkType == 0)
            {
                string configString = Lib.Utils.Base64Decode(server);
                var vmess = Lib.Utils.ConfigString2Vmess(configString);
                link = Lib.Utils.Vmess2VmessLink(vmess);
                pair = Lib.QRCode.QRCode.GenQRCode(link, 180);
            }
            else
            {
                var prefix = Model.Data.Table.linkPrefix[(int)Model.Data.Enum.LinkTypes.v2ray];
                link = prefix + server;
                pair = Lib.QRCode.QRCode.GenQRCode(link, 320);
            }

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
            ShowQRCode();
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

        private void cboxServList_SelectedIndexChanged(object sender, EventArgs e)
        {
            servIndex = cboxServList.SelectedIndex;
            ShowQRCode();
        }
        #endregion
    }
}
