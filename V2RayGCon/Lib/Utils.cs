using Ionic.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Lib
{
    public class Utils
    {
        public static Func<string, string, string> FuncGetString(JObject json)
        {
            var c = json;
            return (prefix, key) =>
            {
                return Lib.Utils.GetString(c, prefix + key);
            };
        }

        public static Func<string, string, string, string> FuncGetAddr(JObject json)
        {
            var c = json;
            return (prefix, keyIP, keyPort) =>
            {
                var ip = Lib.Utils.GetString(c, prefix + keyIP);
                var port = Lib.Utils.GetString(c, prefix + keyPort);
                return string.Join(":", ip, port);
            };
        }

        public static int GetIndex(Dictionary<int, string> dict, string value)
        {
            foreach (var data in dict)
            {
                if (data.Value.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                {
                    return data.Key;
                }
            }
            return -1;
        }

        public static Model.Data.Shadowsocks SSLink2SS(string ssLink)
        {
            string b64 = GetLinkBody(ssLink);

            try
            {
                var ss = new Model.Data.Shadowsocks();
                var plainText = Base64Decode(b64);
                var parts = plainText.Split('@');
                var mp = parts[0].Split(':');
                if (parts[1].Length > 0 && mp[0].Length > 0 && mp[1].Length > 0)
                {
                    ss.method = mp[0];
                    ss.pass = mp[1];
                    ss.addr = parts[1];
                }
                return ss;
            }
            catch { }
            return null;
        }

        public static string SSLink2ConfigString(string ssLink)
        {
            Model.Data.Shadowsocks ss = SSLink2SS(ssLink);
            if (ss == null)
            {
                return string.Empty;
            }

            TryParseIPAddr(ss.addr, out string ip, out int port);
            var tpl = JObject.Parse(resData("config_tpl"));
            var config = tpl["tplImportSS"];

            var setting = config["outbound"]["settings"]["servers"][0];
            setting["address"] = ip;
            setting["port"] = port;
            setting["method"] = ss.method;
            setting["password"] = ss.pass;

            return config.ToString();
        }

        public static string CutStr(string s, int len)
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

        static JToken GetValue(JToken json, string path)
        {

            var curPos = json;
            var keys = path.Split('.');

            int depth;
            for (depth = 0; depth < keys.Length; depth++)
            {
                if (curPos == null || !curPos.HasValues)
                {
                    break;
                }

                if (int.TryParse(keys[depth], out int n))
                {
                    curPos = curPos[n];
                }
                else
                {
                    curPos = curPos[keys[depth]];
                }
            }

            if (depth < keys.Length)
            {
                return null;
            }

            return curPos;
        }

        public static bool GetBool(JToken json, string path)
        {
            string value = GetString(json, path);
            return value.Equals("True", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string GetString(JToken json, string path)
        {
            JToken value = GetValue(json, path);
            return value == null ? string.Empty : value.ToString();
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
            port = Clamp(Str2Int(parts[1]), 0, 65536);
            return true;
        }

        public static Model.Data.Vmess ConfigString2Vmess(string config)
        {
            Model.Data.Vmess vmess = new Model.Data.Vmess();
            JObject json;
            try
            {
                json = JObject.Parse(config);
            }
            catch
            {
                return null;
            }

            vmess.ps = GetString(json, "v2raygcon.alias");

            var prefix = "outbound.settings.vnext.0.";
            vmess.add = GetString(json, prefix + "address");
            vmess.port = GetString(json, prefix + "port");
            vmess.id = GetString(json, prefix + "users.0.id");
            vmess.aid = GetString(json, prefix + "users.0.alterId");

            prefix = "outbound.streamSettings.";
            vmess.net = GetString(json, prefix + "network");
            vmess.type = GetString(json, prefix + "kcpSettings.header.type");
            vmess.host = GetString(json, prefix + "wsSettings.path");
            vmess.tls = GetString(json, prefix + "security");
            return vmess;
        }

        public static string VmessLink2ConfigString(string vmessString)
        {
            var vmess = VmessLink2Vmess(vmessString);

            if (vmess == null)
            {
                return string.Empty;
            }

            // prepare template
            var tpl = JObject.Parse(resData("config_tpl"));
            var config = tpl["tplImortVmess"];
            config["v2raygcon"]["alias"] = vmess.ps;

            var cPos = config["outbound"]["settings"]["vnext"][0];
            cPos["address"] = vmess.add;
            cPos["port"] = Lib.Utils.Str2Int(vmess.port);
            cPos["users"][0]["id"] = vmess.id;
            cPos["users"][0]["alterId"] = Lib.Utils.Str2Int(vmess.aid);

            // insert stream type
            string[] streamTypes = { "ws", "tcp", "kcp" };
            string streamType = vmess.net.ToLower();

            if (!streamTypes.Contains(streamType))
            {
                return config.ToString();
            }

            config["outbound"]["streamSettings"] = tpl[streamType];

            config["outbound"]["streamSettings"]["security"] = vmess.tls;
            try
            {
                if (streamType.Equals("kcp"))
                {
                    config["outbound"]["streamSettings"]["kcpSettings"]["header"]["type"] = vmess.type;
                }

                if (streamType.Equals("ws"))
                {
                    config["outbound"]["streamSettings"]["wsSettings"]["path"] = vmess.host;
                }

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
                    proc.WaitForExit(1000);
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

        static string Base64PadRight(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }

        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return string.Empty;
            }
            var padded = Base64PadRight(base64EncodedData);
            var base64EncodedBytes = System.Convert.FromBase64String(padded);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        static string GetLinkPrefix(Model.Data.Enum.LinkTypes linkType)
        {
            return Model.Data.Table.linkPrefix[(int)linkType];
        }

        public static string GenPattern(Model.Data.Enum.LinkTypes linkType)
        {
            return string.Format(
               "{0}{1}{2}",
               resData("PatternNonAlphabet"), // vme[ss]
               GetLinkPrefix(linkType),
               resData("PatternBase64"));
        }


        public static string LinkAddPrefix(string b64Content, Model.Data.Enum.LinkTypes linkType)
        {

            return GetLinkPrefix(linkType) + b64Content;
        }

        public static string Vmess2VmessLink(Model.Data.Vmess vmess)
        {
            if (vmess == null)
            {
                return string.Empty;
            }

            string content = JsonConvert.SerializeObject(vmess);
            return LinkAddPrefix(
                Base64Encode(content),
                Model.Data.Enum.LinkTypes.vmess);
        }

        public static Model.Data.Vmess VmessLink2Vmess(string link)
        {
            try
            {
                string plainText = Base64Decode(GetLinkBody(link));
                var vmess = JsonConvert.DeserializeObject<Model.Data.Vmess>(plainText);
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

        public static string GetLinkBody(string link)
        {
            Regex re = new Regex("[a-zA-Z0-9]+://");
            return re.Replace(link, string.Empty);
        }

        public static string GetClipboardText()
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                return Clipboard.GetText(TextDataFormat.Text);

            }
            return string.Empty;
        }

        public static int Clamp(int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max - 1), min);
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
