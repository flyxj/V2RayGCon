using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormConfigTester : Form
    {
        string formTitle;
        int preIndex, maxNumberLines;
        static AutoResetEvent sayGoodbye = new AutoResetEvent(false);

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

            setting.OnRequireMenuUpdate += SettingChange;

            this.FormClosed += (s, a) =>
            {
                tester.OnLog -= LogReceiver;
                setting.OnRequireMenuUpdate -= SettingChange;
                setting.LazyGC();
                tester.StopCoreThen(() => sayGoodbye.Set());
                sayGoodbye.WaitOne();
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

            var servers = setting.GetServerList();

            if (servers.Count <= 0)
            {
                cboxServList.SelectedIndex = -1;
                return;
            }

            for (int i = 0; i < servers.Count; i++)
            {
                cboxServList.Items.Add(servers[i].name);
            }

            cboxServList.SelectedIndex = Lib.Utils.Clamp(
                preIndex,
                0,
                servers.Count);
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
            var configString = setting.GetServerByIndex(index);

            if (string.IsNullOrEmpty(configString))
            {
                tester.StopCoreThen(null);
                return;
            }

            JObject config = null;
            try
            {
                config = Lib.ImportParser.ParseImport(
                    cboxGlobalImport.Checked ?
                    Lib.Utils.InjectGlobalImport(configString) :
                    configString);
            }
            catch
            {
                SendLog(I18N("DecodeFail"));
                tester.StopCoreThen(null);
                return;
            }

            var s = config.ToString();
            var env = Lib.Utils.GetEnvVarsFromConfig(config);

            config = null;
            setting.LazyGC();
            tester.RestartCoreThen(s, OnCoreStatusChanged, null, env);
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
