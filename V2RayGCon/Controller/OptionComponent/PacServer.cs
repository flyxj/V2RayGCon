using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace V2RayGCon.Controller.OptionComponent
{
    class PacServer : OptionComponentController
    {
        Service.Setting setting;
        Service.PACServer pacServer;

        TextBox tboxPort;
        CheckBox chkIsAutorun;
        RichTextBox rtboxCustomWhiteList, rtboxCustomBlackList;

        public PacServer(
            TextBox port,
            CheckBox isAutorun,
            RichTextBox customWhiteList,
            RichTextBox customBlackList)
        {
            setting = Service.Setting.Instance;
            pacServer = Service.PACServer.Instance;

            tboxPort = port;
            chkIsAutorun = isAutorun;
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
            rtboxCustomBlackList.Text = pacSetting.customBlackList;
            rtboxCustomWhiteList.Text = pacSetting.customWhiteList;
        }

        void UpdateSystemProxy()
        {
            var proxy = setting.GetSysProxySetting();
            if (string.IsNullOrEmpty(proxy.autoConfigUrl))
            {
                // do not need to update
                return;
            }

            // https://stackoverflow.com/questions/2884551/get-individual-query-parameters-from-uri
            Uri uri = new Uri(proxy.autoConfigUrl);
            var arguments = uri.Query
                .Substring(1) // Remove '?'
                .Split('&')
                .Select(q => q.Split('='))
                .ToDictionary(q => q.FirstOrDefault(), q => q.Skip(1).FirstOrDefault());

            if (arguments == null)
            {
                return;
            }

            // e.g. http://localhost:3000/pac/?&port=5678&ip=1.2.3.4&proto=socks&type=whitelist&key=rnd

            arguments.TryGetValue("ip", out string ip);
            arguments.TryGetValue("port", out string port);
            arguments.TryGetValue("type", out string type);
            arguments.TryGetValue("proto", out string proto);

            if (string.IsNullOrEmpty(ip)
                || string.IsNullOrEmpty(port)
                || string.IsNullOrEmpty(type)
                || string.IsNullOrEmpty(proto))
            {
                Debug.WriteLine("Update pac url fail!");
                return;
            }

            pacServer.SetPACProx(
                ip,
                Lib.Utils.Str2Int(port),
                proto == "socks",
                type == "whitelist");
        }
        #endregion
    }
}
