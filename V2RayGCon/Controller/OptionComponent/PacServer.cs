using System;
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

            InitControls(port, isAutorun, customWhiteList, customBlackList);
        }

        private void InitControls(TextBox port, CheckBox isAutorun, RichTextBox customWhiteList, RichTextBox customBlackList)
        {

            tboxPort = port;
            chkIsAutorun = isAutorun;
            rtboxCustomBlackList = customBlackList;
            rtboxCustomWhiteList = customWhiteList;

            var pacSetting = setting.GetPacServerSettings();

            port.Text = pacSetting.port.ToString();
            isAutorun.Checked = pacSetting.isAutorun;
            customBlackList.Text = pacSetting.customBlackList;
            customWhiteList.Text = pacSetting.customWhiteList;
        }

        #region public method
        public void UpdateSystemProxy()
        {
            var curProxyUrl = setting.curSysProxyUrl;
            if (string.IsNullOrEmpty(curProxyUrl)
                || !curProxyUrl.Contains("?"))
            {
                // do not need to update
                return;
            }

            // https://stackoverflow.com/questions/2884551/get-individual-query-parameters-from-uri
            Uri uri = new Uri(curProxyUrl);
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
                return;
            }

            bool isWhiteList = type == "whitelist";
            var newProxyUrl = pacServer.GetPacUrl(
                type == "whitelist",
                proto == "socks",
                ip,
                Lib.Utils.Str2Int(port));

            setting.SetSystemProxy(newProxyUrl);
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
        bool IsIndexValide(int index)
        {
            if (index < 0 || index > 2)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
