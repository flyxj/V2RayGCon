using System.Windows.Forms;

namespace V2RayGCon.Controller.OptionComponent
{
    class PacServer : OptionComponentController
    {
        Service.Setting setting;
        Service.PACServer pacServer;

        TextBox tboxPort;
        CheckBox chkIsAutorun, chkIsAutoTrack;
        RichTextBox rtboxCustomWhiteList, rtboxCustomBlackList;

        public PacServer(
            TextBox port,
            CheckBox isAutorun,
            CheckBox isAutoTrack,
            RichTextBox customWhiteList,
            RichTextBox customBlackList)
        {
            setting = Service.Setting.Instance;
            pacServer = Service.PACServer.Instance;

            tboxPort = port;
            chkIsAutorun = isAutorun;
            chkIsAutoTrack = isAutoTrack;
            rtboxCustomBlackList = customBlackList;
            rtboxCustomWhiteList = customWhiteList;

            InitControls();
        }

        #region public method
        public void Reload(string rawPacServSetting)
        {
            Properties.Settings.Default.PacServerSettings = rawPacServSetting;
            Properties.Settings.Default.Save();

            InitControls();
            if (pacServer.isWebServRunning)
            {
                pacServer.RestartPacServer();
            }
        }

        public override bool SaveOptions()
        {
            if (!IsOptionsChanged())
            {
                return false;
            }

            var pacSetting = new Model.Data.PacServerSettings
            {
                port = Lib.Utils.Str2Int(tboxPort.Text),
                isAutorun = chkIsAutorun.Checked,
                isAutoTrack = chkIsAutoTrack.Checked,
                customBlackList = rtboxCustomBlackList.Text,
                customWhiteList = rtboxCustomWhiteList.Text,
            };

            setting.SavePacServerSettings(pacSetting);
            if (pacServer.isWebServRunning)
            {
                pacServer.RestartPacServer();
            }
            UpdateSystemProxy();
            return true;
        }

        public override bool IsOptionsChanged()
        {
            var s = setting.GetPacServerSettings();

            if (s.port != Lib.Utils.Str2Int(tboxPort.Text)
                || s.isAutorun != chkIsAutorun.Checked
                || s.isAutoTrack != chkIsAutoTrack.Checked
                || s.customBlackList != rtboxCustomBlackList.Text
                || s.customWhiteList != rtboxCustomWhiteList.Text)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region private method
        private void InitControls()
        {
            var pacSetting = setting.GetPacServerSettings();
            tboxPort.Text = pacSetting.port.ToString();
            chkIsAutorun.Checked = pacSetting.isAutorun;
            chkIsAutoTrack.Checked = pacSetting.isAutoTrack;
            rtboxCustomBlackList.Text = pacSetting.customBlackList;
            rtboxCustomWhiteList.Text = pacSetting.customWhiteList;
        }

        void UpdateSystemProxy()
        {
            var proxySetting = setting.GetSysProxySetting();
            var proxyParams = Lib.Utils.GetProxyParamsFromUrl(proxySetting.autoConfigUrl);

            if (proxyParams == null)
            {
                return;
            }

            pacServer.SetPACProx(proxyParams);
        }
        #endregion
    }
}
