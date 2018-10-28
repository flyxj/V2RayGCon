using Newtonsoft.Json;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Controller.OptionComponent
{
    class PacServer : OptionComponentController
    {
        Service.Setting setting;
        Service.PacServer pacServer;

        TextBox tboxPort, tboxCustomPacFilePath;
        CheckBox chkIsAutorun, chkCustomPacFile;
        RichTextBox rtboxCustomWhiteList, rtboxCustomBlackList;
        Button btnBrowseCustomPacFile;

        public PacServer(
            TextBox port,
            CheckBox isAutorun,
            RichTextBox customWhiteList,
            RichTextBox customBlackList,

            // custom pac file
            TextBox tboxCustomPacFilePath,
            CheckBox chkCustomPacFile,
            Button btnBrowseCustomPacFile)
        {
            setting = Service.Setting.Instance;
            pacServer = Service.PacServer.Instance;

            tboxPort = port;
            chkIsAutorun = isAutorun;
            rtboxCustomBlackList = customBlackList;
            rtboxCustomWhiteList = customWhiteList;

            this.chkCustomPacFile = chkCustomPacFile;
            this.tboxCustomPacFilePath = tboxCustomPacFilePath;
            this.btnBrowseCustomPacFile = btnBrowseCustomPacFile;

            InitControls();
            BindEvents();
        }


        #region public method
        public void Reload(string rawPacServSetting)
        {
            try
            {
                var pacServSetting = JsonConvert
                    .DeserializeObject<Model.Data.PacServerSettings>(
                    rawPacServSetting);
                setting.SavePacServerSettings(pacServSetting);
            }
            catch
            {
                setting.SavePacServerSettings(new Model.Data.PacServerSettings());
            }

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
                customBlackList = rtboxCustomBlackList.Text,
                customWhiteList = rtboxCustomWhiteList.Text,
                customPacFilePath = tboxCustomPacFilePath.Text,
                isUseCustomPac = chkCustomPacFile.Checked,
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

            // basic
            if (s.port != Lib.Utils.Str2Int(tboxPort.Text)
                || s.isAutorun != chkIsAutorun.Checked
                || s.customBlackList != rtboxCustomBlackList.Text
                || s.customWhiteList != rtboxCustomWhiteList.Text)
            {
                return true;
            }

            // custom pac file
            if (s.isUseCustomPac != this.chkCustomPacFile.Checked
                || s.customPacFilePath != this.tboxCustomPacFilePath.Text)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region private method
        private void BindEvents()
        {
            this.btnBrowseCustomPacFile.Click += (s, a) =>
            {
                var filename = Lib.UI.ShowSelectFileDialog(StrConst.ExtJs);
                if (!string.IsNullOrEmpty(filename))
                {
                    this.tboxCustomPacFilePath.Text = filename;
                }
            };
        }

        private void InitControls()
        {
            var pacSetting = setting.GetPacServerSettings();
            tboxPort.Text = pacSetting.port.ToString();
            chkIsAutorun.Checked = pacSetting.isAutorun;
            rtboxCustomBlackList.Text = pacSetting.customBlackList;
            rtboxCustomWhiteList.Text = pacSetting.customWhiteList;
            chkCustomPacFile.Checked = pacSetting.isUseCustomPac;
            tboxCustomPacFilePath.Text = pacSetting.customPacFilePath;
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
