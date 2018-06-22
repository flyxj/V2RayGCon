using System;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller
{
    class FormMainCtrl
    {
        Service.Setting setting;

        public FormMainCtrl()
        {
            setting = Service.Setting.Instance;
        }

        public void CopyV2RayLink(int index)
        {
            var server = setting.GetServer(index);
            string v2rayLink = string.Empty;

            if (!string.IsNullOrEmpty(server))
            {
                v2rayLink = Lib.Utils.AddLinkPrefix(server, Model.Data.Enum.LinkTypes.v2ray);
            }

            MessageBox.Show(
                Lib.Utils.CopyToClipboard(v2rayLink) ?
                I18N("LinksCopied") :
                I18N("CopyFail"));
        }

        public void CopyVmessLink(int index)
        {
            var server = setting.GetServer(index);
            string vmessLink = string.Empty;

            if (!string.IsNullOrEmpty(server))
            {
                var config = Lib.Utils.Base64Decode(server);
                var vmess = Lib.Utils.ConfigString2Vmess(config);
                vmessLink = Lib.Utils.Vmess2VmessLink(vmess);
            }

            MessageBox.Show(
                Lib.Utils.CopyToClipboard(vmessLink) ?
                I18N("LinksCopied") :
                I18N("CopyFail"));
        }

        public void ImportLinks()
        {
            string links = Lib.Utils.GetClipboardText();

            MessageBox.Show(
                setting.ImportLinks(links) ?
                I18N("ImportLinkSuccess") :
                I18N("ImportLinkFail"));
        }

        public void CopyAllV2RayLink()
        {
            var servers = setting.GetAllServers();
            string s = string.Empty;

            foreach (var server in servers)
            {
                s += "v2ray://" + server + "\r\n";
            }

            MessageBox.Show(
                Lib.Utils.CopyToClipboard(s) ?
                I18N("LinksCopied") :
                I18N("CopyFail"));
        }

        public void CheckUpdate()
        {
            var version = Lib.Utils.GetLatestVGCVersion();
            if (string.IsNullOrEmpty(version))
            {
                MessageBox.Show(I18N("GetVGCVerFail"));
                return;
            }

            var verNew = new Version(version);
            var verCur = new Version(Properties.Resources.Version);

            var result = verCur.CompareTo(verNew);
            if (result >= 0)
            {
                MessageBox.Show(I18N("NoNewVGC"));
                return;
            }

            var confirmTpl = I18N("ConfirmDownloadNewVGC");
            var msg = string.Format(confirmTpl, version);
            if (Lib.UI.Confirm(msg))
            {
                var tpl = resData("TplUrlVGCRelease");
                var url = string.Format(tpl, version);
                System.Diagnostics.Process.Start(url);
            }
        }

        public void CopyAllVmessLink()
        {
            var servers = setting.GetAllServers();
            string s = string.Empty;

            foreach (var server in servers)
            {
                var config = Lib.Utils.Base64Decode(server);
                var vmess = Lib.Utils.ConfigString2Vmess(config);
                var vmessLink = Lib.Utils.Vmess2VmessLink(vmess);

                if (!string.IsNullOrEmpty(vmessLink))
                {
                    s += vmessLink + "\r\n";
                }
            }

            MessageBox.Show(
                Lib.Utils.CopyToClipboard(s) ?
                I18N("LinksCopied") :
                I18N("CopyFail"));
        }
    }
}
