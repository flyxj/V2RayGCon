using System;
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

        delegate void UpdateServerListDelegate(int index);


        FormQRCode()
        {
            InitializeComponent();
            setting = Service.Setting.Instance;

            servIndex = 0;
            linkType = 0;

            UpdateServerList();

            cboxLinkType.SelectedIndex = linkType;

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= SettingChange;
            };

            this.Show();

            setting.OnSettingChange += SettingChange;
        }

        void SettingChange(object sender, EventArgs args)
        {
            UpdateServerListDelegate updater =
                new UpdateServerListDelegate(UpdateServerList);
            try
            {
                cboxServList?.Invoke(updater, -1);
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
            if (linkType == 0)
            {
                string configString = Lib.Utils.Base64Decode(server);
                var vmess = Lib.Utils.ConfigString2Vmess(configString);
                link = Lib.Utils.Vmess2VmessLink(vmess);
                if (!string.IsNullOrEmpty(link))
                {
                    picQRCode.Image = Lib.QRCode.QRCode.GenQRCode(link, 180);
                }
            }
            else
            {
                var prefix = Model.Data.Table.linkPrefix[
                    (int)Model.Data.Enum.LinkTypes.v2ray];
                link = prefix + server;
                picQRCode.Image = Lib.QRCode.QRCode.GenQRCode(link, 320);
            }

        }

        private void cboxLinkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            linkType = cboxLinkType.SelectedIndex;
            ShowQRCode();
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
    }
}
