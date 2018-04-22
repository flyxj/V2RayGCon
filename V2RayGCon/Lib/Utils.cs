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
    class Utils
    {
        public static Func<string, string, string> ClosureGetStringFromJToken(JObject config)
        {
            var c = config;
            return (perfix, key) =>
            {
                return Lib.Utils.GetStrFromJToken(c, perfix + key);
            };
        }

        public static Func<string, string, string, string> ClosureGetAddrFromJToken(JObject config)
        {
            var c = config;
            return (perfix, keyIP, keyPort) =>
            {
                var ip = Lib.Utils.GetStrFromJToken(c, perfix + keyIP);
                var port = Lib.Utils.GetStrFromJToken(c, perfix + keyPort);
                return string.Join(":", ip, port);
            };
        }


        public static int LookupDict(Dictionary<int, string> dict, string value)
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
            string b64Link = LinkBody(ssLink, Model.Data.Enum.LinkTypes.ss);

            try
            {
                var ss = new Model.Data.Shadowsocks();
                var plainText = Base64Decode(b64Link);
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
            JObject config = JObject.Parse(resData("config_ss_tpl"));

            var setting = config["outbound"]["settings"]["servers"][0];
            setting["address"] = ip;
            setting["port"] = port;
            setting["method"] = ss.method;
            setting["password"] = ss.pass;

            return config.ToString();
        }

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

        public static Model.Data.Vmess ConfigString2Vmess(string config)
        {
            Model.Data.Vmess v = new Model.Data.Vmess();
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
            Model.Data.Vmess v = VmessLink2Vmess(vmessLink);

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
            string[] streamTypes = { "ws", "tcp", "kcp" };
            string streamType = v.net.ToLower();

            if (!streamTypes.Contains(streamType))
            {
                return config.ToString();
            }

            var template = JObject.Parse(resData("config_tpl"));

            config["outbound"]["streamSettings"] = template[streamType];

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



        public static string LinkAddPerfix(string b64Content, Model.Data.Enum.LinkTypes type)
        {
            string perfix = string.Empty;

            switch (type)
            {
                case Model.Data.Enum.LinkTypes.vmess:
                    perfix = resData("VmessLinkPerfix");
                    break;

                case Model.Data.Enum.LinkTypes.v2ray:
                    perfix = resData("V2RayLinkPerfix");
                    break;
            }

            return perfix + b64Content;
        }

        public static string Vmess2VmessLink(Model.Data.Vmess vmess)
        {
            if (vmess == null)
            {
                return string.Empty;
            }

            string content = JsonConvert.SerializeObject(vmess);
            return LinkAddPerfix(
                Base64Encode(content),
                Model.Data.Enum.LinkTypes.vmess);
        }

        public static Model.Data.Vmess VmessLink2Vmess(string link)
        {
            string base64_text = LinkBody(link, Model.Data.Enum.LinkTypes.vmess);
            try
            {
                string plain_text = Base64Decode(base64_text);
                var vmess = JsonConvert.DeserializeObject<Model.Data.Vmess>(plain_text);
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

        public static string LinkBody(string link, Model.Data.Enum.LinkTypes type)
        {
            int len = 0;
            switch (type)
            {
                case Model.Data.Enum.LinkTypes.vmess:
                    len = resData("VmessLinkPerfix").Length;
                    break;

                case Model.Data.Enum.LinkTypes.v2ray:
                    len = resData("V2RayLinkPerfix").Length;
                    break;

                case Model.Data.Enum.LinkTypes.ss:
                    len = resData("SSLinkPerfix").Length;
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
