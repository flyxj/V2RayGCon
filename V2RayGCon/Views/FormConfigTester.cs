using System;
using System.Windows.Forms;

namespace V2RayGCon.Views
{
    public partial class FormConfigTester : Form
    {
        Service.Setting setting;
        int preIndex;

        public FormConfigTester()
        {
            InitializeComponent();
            setting = Service.Setting.Instance;
            preIndex = -1;
            this.Show();
        }

        private void FormConfigTester_Shown(object sender, System.EventArgs e)
        {
            UpdateServerList();

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= SettingChange;
            };

            setting.OnSettingChange += SettingChange;
        }

        #region public method
        #endregion

        #region private method

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

        void UpdateServerList()
        {
            cboxServList.Items.Clear();

            var aliases = setting.GetAllAliases();

            if (aliases.Count <= 0)
            {
                cboxServList.SelectedIndex = -1;
                return;
            }

            foreach (var alias in aliases)
            {
                cboxServList.Items.Add(alias);
            }

            cboxServList.SelectedIndex = Math.Min(preIndex, aliases.Count - 1);
        }

        #endregion

        #region UI event
        private void cboxServList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var index = cboxServList.SelectedIndex;
            if (preIndex == index)
            {
                return;
            }
            preIndex = index;
            if (index == -1)
            {
                return;
            }
        }
        #endregion
    }
}
