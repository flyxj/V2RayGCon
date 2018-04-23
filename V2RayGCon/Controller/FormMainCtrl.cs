using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
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
                v2rayLink = Lib.Utils.LinkAddPrefix(server, Model.Data.Enum.LinkTypes.v2ray);
            }

            Lib.UI.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(v2rayLink),
                I18N("LinksCopied"),
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

            Lib.UI.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(vmessLink),
                I18N("LinksCopied"),
                I18N("CopyFail"));
        }

        public void ImportLinks()
        {
            string links = Lib.Utils.GetClipboardText();

            Lib.UI.ShowMsgboxSuccFail(
                setting.ImportLinks(links),
                I18N("ImportLinkSuccess"),
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

            Lib.UI.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(s),
                I18N("LinksCopied"),
                I18N("CopyFail"));
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

            Lib.UI.ShowMsgboxSuccFail(
                Lib.Utils.CopyToClipboard(s),
                I18N("LinksCopied"),
                I18N("CopyFail"));
        }

        public List<string[]> GetServerInfo()
        {
            var info = new List<string[]> { };

            var servers = setting.GetAllServers();

            if (servers.Count <= 0)
            {
                Debug.WriteLine("FormMain: servers is empty!");
                return info;
            }

            int count = 0;
            int selectedServer = setting.GetSelectedServerIndex();
            string s, proxy;
            JObject o;
            foreach (var server in servers)
            {
                try
                {
                    s = Lib.Utils.Base64Decode(server);
                    o = JObject.Parse(s);

                }
                catch { continue; }

                string ip, port, type, tls, path, streamType, alias;
                proxy = Lib.Utils.GetStrFromJToken(o, "outbound.protocol");

                if (proxy.Equals("shadowsocks"))
                {
                    ip = Lib.Utils.GetStrFromJToken(o, "outbound.settings.servers.0.address");
                    port = Lib.Utils.GetStrFromJToken(o, "outbound.settings.servers.0.port");
                    tls = Lib.Utils.GetStrFromJToken(o, "outbound.settings.servers.0.method");
                }
                else
                {
                    ip = Lib.Utils.GetStrFromJToken(o, "outbound.settings.vnext.0.address");
                    port = Lib.Utils.GetStrFromJToken(o, "outbound.settings.vnext.0.port");
                    tls = Lib.Utils.GetStrFromJToken(o, "outbound.streamSettings.security");
                }

                streamType = Lib.Utils.GetStrFromJToken(o, "outbound.streamSettings.network");
                type = Lib.Utils.GetStrFromJToken(o, "outbound.streamSettings.kcpSettings.header.type");
                path = Lib.Utils.GetStrFromJToken(o, "outbound.streamSettings.wsSettings.path");
                alias = Lib.Utils.GetStrFromJToken(o, "v2raygcon.alias");


                info.Add(new string[] {
                    (count+1).ToString(), //no.
                    string.IsNullOrEmpty(alias)?I18N("Empty"):Lib.Utils.CutString(alias,15),
                    proxy,
                    ip,
                    port,
                    count == selectedServer?"√":"",  //active
                    path, // Url
                    streamType, //protocol
                    tls, //encryption
                    type // disguise
                });
                count++;
            }

            return info;
        }

    }
}
