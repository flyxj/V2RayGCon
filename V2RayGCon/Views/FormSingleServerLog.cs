using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormSingleServerLog : Form
    {
        Service.Setting setting;
        int maxNumberLines;
        Model.Data.ServerItem serverItem;

        public FormSingleServerLog(Model.Data.ServerItem serverItem)
        {
            setting = Service.Setting.Instance;
            maxNumberLines = setting.maxLogLines;

            this.serverItem = serverItem;

            InitializeComponent();

            this.FormClosed += (s, e) =>
            {
                serverItem.OnLog -= OnLogHandler;
            };

#if DEBUG
            this.Icon = Properties.Resources.icon_light;
#endif

            this.Show();

            this.Text = I18N("Log") + " - " + serverItem.summary;
            rtBoxLogger.Text = serverItem.logCache;
            serverItem.OnLog += OnLogHandler;
        }

        private void OnLogHandler(object sender, Model.Data.StrEvent args)
        {
            Task.Factory.StartNew(() =>
            {
                rtBoxLogger.Invoke((MethodInvoker)delegate
                {
                    rtBoxLogger.Text = serverItem.logCache;
                });
            });
        }

        private void rtBoxLogger_TextChanged(object sender, System.EventArgs e)
        {
            // set the current caret position to the end
            rtBoxLogger.SelectionStart = rtBoxLogger.Text.Length;
            // scroll it automatically
            rtBoxLogger.ScrollToCaret();
        }
    }
}
