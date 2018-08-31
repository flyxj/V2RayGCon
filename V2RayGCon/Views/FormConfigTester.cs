using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormConfigTester : Form
    {
        string formTitle;
        int preIndex, maxNumberLines;

        Service.Setting setting;
        Model.BaseClass.CoreServer tester;

        public FormConfigTester()
        {
            InitializeComponent();

            setting = Service.Setting.Instance;
            preIndex = -1;
            maxNumberLines = setting.maxLogLines;

#if DEBUG
            this.Icon = Properties.Resources.icon_light;
#endif

            this.Show();
        }

        private void FormConfigTester_Shown(object sender, System.EventArgs e)
        {
            formTitle = this.Text;

            UpdateServerList();

            tester = new Model.BaseClass.CoreServer();
            tester.OnLog += LogReceiver;

            setting.OnSettingChange += SettingChange;

            this.FormClosed += (s, a) =>
            {
                tester.OnLog -= LogReceiver;
                setting.OnSettingChange -= SettingChange;
                tester.StopCoreThen(null);
            };

            Lib.UI.SetFormLocation<FormConfigTester>(this, Model.Data.Enum.FormLocations.BottomLeft);
        }

        #region public method
        #endregion

        #region private method

        void LogReceiver(object sender, Model.Data.StrEvent e)
        {
            var content = e.Data;
            try
            {
                rtboxLog.Invoke((MethodInvoker)delegate
                {
                    if (rtboxLog.Lines.Length >= maxNumberLines - 1)
                    {
                        rtboxLog.Lines = rtboxLog.Lines.Skip(rtboxLog.Lines.Length - maxNumberLines).ToArray();
                    }
                    rtboxLog.AppendText(content + "\r\n");
                });
            }
            catch { }

        }

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

            cboxServList.SelectedIndex = Lib.Utils.Clamp(
                preIndex,
                0,
                aliases.Count);
        }

        void SetTitle(bool running)
        {
            if (running)
            {
                this.Text = string.Format("{0} - {1}", formTitle, cboxServList.Text);
            }
            else
            {
                this.Text = formTitle;
            }
        }

        void OnCoreStatusChanged()
        {
            var isRunning = tester.isRunning;
            btnRestart.Invoke((MethodInvoker)delegate
            {
                SetTitle(isRunning);
                btnStop.Enabled = isRunning;
            });
        }

        void SendLog(string log)
        {
            var arg = new Model.Data.StrEvent(log);
            LogReceiver(this, arg);
        }

        void RestartCore(int index)
        {
            var b64Config = setting.GetServer(index);
            if (string.IsNullOrEmpty(b64Config))
            {
                tester.StopCoreThen(null);
                return;
            }

            JObject config = null;
            try
            {
                string plainText = Lib.Utils.Base64Decode(b64Config);

                config = Lib.ImportParser.ParseImport(
                    cboxGlobalImport.Checked ?
                    Lib.Utils.InjectGlobalImport(plainText) :
                    plainText);
            }
            catch
            {
                SendLog(I18N("DecodeFail"));
                tester.StopCoreThen(null);
                return;
            }

            var s = config.ToString();
            config = null;
            System.GC.Collect();
            tester.RestartCore(s, OnCoreStatusChanged);
        }
        #endregion

        #region UI event
        private void cboxServList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            preIndex = cboxServList.SelectedIndex;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            RestartCore(cboxServList.SelectedIndex);
        }

        private void rtboxLog_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            rtboxLog.SelectionStart = rtboxLog.Text.Length;
            // scroll it automatically
            rtboxLog.ScrollToCaret();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            tester.StopCoreThen(null);
            this.Text = formTitle;
        }
        #endregion


    }
}
