using Ionic.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Management;
using System.Linq;
using static V2RayGCon.Lib.StringResource;
using System.Text.RegularExpressions;

namespace V2RayGCon.Lib
{
    class Utils
    {
        public static MatchCollection MatchAll(string content, string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return Regex.Matches(content, pattern, RegexOptions.IgnoreCase);
        }

        public static string CutString(string s, int len)
        {
            return s.Substring(0, Math.Min(s.Length, len));
        }

        public static void SupportCtrlA(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A && sender != null)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        public static int Str2Int(string s)
        {
            return Int32.TryParse(s, out int number) ? number : 0;
        }

        static JToken WalkToJToken(JToken start, string path)
        {
            int i;
            var t = start;
            var step = path.Split('.');

            for (i = 0; i < step.Length; i++)
            {
                if (t == null || !t.HasValues)
                {
                    break;
                }

                if (int.TryParse(step[i], out int n))
                {
                    t = t[n];
                }
                else
                {
                    t = t[step[i]];
                }
            }

            if (i < step.Length)
            {
                return null;
            }

            return t;
        }

        public static int GetIntFromJToken(JToken start, string path)
        {
            string v = GetStrFromJToken(start, path);
            return Lib.Utils.Str2Int(v);
        }

        public static bool GetBoolFromJToken(JToken start, string path)
        {
            string v = GetStrFromJToken(start, path);
            return v.Equals("True", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string GetStrFromJToken(JToken start, string path)
        {
            JToken end = WalkToJToken(start, path);
            return end == null ? string.Empty : end.ToString();
        }

        public static bool TryParseIPAddr(string address, out string ip, out int port)
        {
            ip = "127.0.0.1";
            port = 1080;

            string[] parts = address.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            ip = parts[0];
            port = Clamp(Str2Int(parts[1]), 0, 65535);
            return true;
        }

        public static Model.Vmess ConfigString2Vmess(string config)
        {
            Model.Vmess v = new Model.Vmess();
            JObject o;
            try
            {
                o = JObject.Parse(config);
            }
            catch
            {
                return null;
            }

            v.ps = GetStrFromJToken(o, "v2raygcon.alias");
            v.add = GetStrFromJToken(o, "outbound.settings.vnext.0.address");
            v.port = GetStrFromJToken(o, "outbound.settings.vnext.0.port");
            v.id = GetStrFromJToken(o, "outbound.settings.vnext.0.users.0.id");
            v.aid = GetStrFromJToken(o, "outbound.settings.vnext.0.users.0.alterId");
            v.net = GetStrFromJToken(o, "outbound.streamSettings.network");
            v.type = GetStrFromJToken(o, "outbound.streamSettings.kcpSettings.header.type");
            v.host = GetStrFromJToken(o, "outbound.streamSettings.wsSettings.path");
            v.tls = GetStrFromJToken(o, "outbound.streamSettings.security");

            return v;
        }

        public static string VmessLink2ConfigString(string vmessLink)
        {
            Model.Vmess v = VmessLink2Vmess(vmessLink);

            if (v == null)
            {
                return string.Empty;
            }

            // prepare template
            JObject config = JObject.Parse(resData("config_vmess_tpl"));
            config["outbound"]["protocol"] = "vmess";
            config["outbound"]["tag"] = "agentout";
            config["outbound"]["mux"] = new JObject { { "enabled", true } };
            config["v2raygcon"]["alias"] = v.ps;

            // insert vmess info
            config["outbound"]["settings"] = new JObject{
                {"servers",null },
                {"vnext", new JArray{
                    new JObject{
                        {"address",v.add },
                        { "port",Lib.Utils.Str2Int(v.port) },
                        { "users",new JArray{
                            new JObject
                            {
                                {"id",v.id },
                                {"alterId", Lib.Utils.Str2Int(v.aid) },
                                {"security","auto" }
                            }
                        } }
                    }
                } }
            };

            // insert stream type
            string[] streamTypes = { "WS", "TCP", "KCP" };
            string streamType = v.net.ToUpper();

            if (!streamTypes.Contains(streamType))
            {
                return config.ToString();
            }

            var templateName = resData("Tpl" + streamType);
            config["outbound"]["streamSettings"] = JObject.Parse(templateName);

            config["outbound"]["streamSettings"]["security"] = v.tls;
            try
            {
                config["outbound"]["streamSettings"]["kcpSettings"]["header"]["type"] = v.type;
            }
            catch { }
            try
            {
                config["outbound"]["streamSettings"]["wsSettings"]["path"] = v.host;
            }
            catch { }

            return config.ToString();
        }

        public static bool CopyToClipboard(string content)
        {
            try
            {
                Clipboard.SetText(content);
                return true;
            }
            catch { }
            return false;
        }

        public static void ShowMsgboxSuccFail(bool success, string msgSuccess, string msgFail)
        {
            if (success)
            {
                MessageBox.Show(msgSuccess);
            }
            else
            {
                MessageBox.Show(msgFail);
            };
        }

        public static void KillProcessAndChildrens(int pid)
        {
            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection processCollection = processSearcher.Get();

            // We must kill child processes first!
            if (processCollection != null)
            {
                foreach (ManagementObject mo in processCollection)
                {
                    KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
                }
            }

            // Then kill parents.
            try
            {
                Process proc = Process.GetProcessById(pid);
                if (!proc.HasExited)
                {
                    proc.Kill();
                    proc.WaitForExit(2000);
                }
            }
            catch
            {
                // Process already exited.
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool Confirm(string content)
        {
            var confirm = MessageBox.Show(
                content,
                I18N("Confirm"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            return confirm == DialogResult.Yes;
        }

        public static string LinkAddPerfix(string b64Content, Model.Enum.LinkTypes type)
        {
            string perfix = string.Empty;

            switch (type)
            {
                case Model.Enum.LinkTypes.vmess:
                    perfix = resData("VmessLinkPerfix");
                    break;

                case Model.Enum.LinkTypes.v2ray:
                    perfix = resData("V2RayLinkPerfix");
                    break;
            }

            return perfix + b64Content;
        }

        public static string Vmess2VmessLink(Model.Vmess vmess)
        {
            if (vmess == null)
            {
                return string.Empty;
            }

            string content = JsonConvert.SerializeObject(vmess);
            return LinkAddPerfix(
                Base64Encode(content),
                Model.Enum.LinkTypes.vmess);
        }

        public static Model.Vmess VmessLink2Vmess(string link)
        {
            string base64_text = LinkBody(link, Model.Enum.LinkTypes.vmess);
            try
            {
                string plain_text = Base64Decode(base64_text);
                var vmess=JsonConvert.DeserializeObject<Model.Vmess>(plain_text);
                if (!string.IsNullOrEmpty(vmess.add)
                    && !string.IsNullOrEmpty(vmess.port)
                    && !string.IsNullOrEmpty(vmess.aid)) 
                {

                    return vmess;
                }
            }
            catch { }
            return null;
        }

        public static string LinkBody(string link, Model.Enum.LinkTypes type)
        {
            int len = 0;
            switch (type)
            {
                case Model.Enum.LinkTypes.vmess:
                    len = resData("VmessLinkPerfix").Length;
                    break;

                case Model.Enum.LinkTypes.v2ray:
                    len = resData("V2RayLinkPerfix").Length;
                    break;
            }
            return link.Substring(len);
        }

        public static string GetClipboardText()
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                return Clipboard.GetText(TextDataFormat.Text);

            }
            return string.Empty;
        }

#if DEBUG
        public static MenuItem FindSubMenuItemByText(MenuItem parent, string text)
        {
            for (int a = 0; a < parent.MenuItems.Count; a++)
            {

                MenuItem item = parent.MenuItems[a];
                if (item != null)
                {
                    // Debug.WriteLine("FSM: " + a + " name:" +item.Text);
                    if (item.Text == text)
                    {
                        return item;
                    }
                    else
                    {
                        // running reursively
                        if (item.MenuItems.Count > 0)
                        {
                            item = FindSubMenuItemByText(item, text);
                            if (item != null)
                            {
                                return item;
                            }
                        }
                    }
                }
            }
            // nothing found
            return null;
        }

        public static MenuItem FindMenuItemByText(ContextMenu parent, string text)
        {
            for (int a = 0; a < parent.MenuItems.Count; a++)
            {
                MenuItem item = parent.MenuItems[a];
                // Debug.WriteLine("FM: " + a + " name:" + item.Text);
                if (item != null)
                {
                    if (item.Text == text)
                    {
                        return item;
                    }
                    else
                    {
                        // running reursively
                        if (item.MenuItems.Count > 0)
                        {
                            item = FindSubMenuItemByText(item, text);
                            if (item != null)
                            {
                                return item;
                            }
                        }
                    }
                }
            }
            // nothing found
            return null;
        }
#endif

        public static int Clamp(int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static string GetAppDir()
        {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        public static void SupportProtocolTLS12()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        }

        public static void ZipFileDecompress(string fileName)
        {
            // let downloader handle exception
            using (ZipFile zip = ZipFile.Read(fileName))
            {
                var flattenFoldersOnExtract = zip.FlattenFoldersOnExtract;
                zip.FlattenFoldersOnExtract = true;
                zip.ExtractAll(GetAppDir(), ExtractExistingFileAction.OverwriteSilently);
                zip.FlattenFoldersOnExtract = flattenFoldersOnExtract;
            }
        }
    }
}
