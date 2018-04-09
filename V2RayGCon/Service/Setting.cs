using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Setting
    {
        #region Singleton
        static readonly Setting instSetting = new Setting();
        public static Setting Instance
        {
            get
            {
                return instSetting;
            }
        }
        #endregion

        // Begin
        private List<string> servers;
        public event EventHandler OnSettingChange;
        public event EventHandler OnRequireCoreRestart;
        //Func<string, string> resData;

        private int _curEditingIndex;

        Setting()
        {
            //resData = Lib.Utils.resData;
            _curEditingIndex = -1;
            InitServers();
            SaveServers();
        }

        public int curEditingIndex
        {
            get
            {
                return _curEditingIndex;
            }
            set
            {
                if (value >= 0 && value < GetServeNum())
                {
                    _curEditingIndex = value;
                }
                else
                {
                    _curEditingIndex = -1;
                }
            }
        }

        int _SelectedServerIndex
        {
            get
            {
                // bug: return Lib.Utils.Clamp(n, 0, ServNum - 1);
                return Math.Max(0, Properties.Settings.Default.CurServ);
            }
            set
            {
                int n = Lib.Utils.Clamp(value, 0, GetServeNum());
                Properties.Settings.Default.CurServ = n;
                Properties.Settings.Default.Save();
            }
        }

        public int GetSelectedServerIndex()
        {
            return _SelectedServerIndex;
        }

        public int GetServerNum()
        {
            return servers.Count;
        }

        public int maxLogLines
        {
            get
            {
                int n = Properties.Settings.Default.MaxLogLine;
                return Lib.Utils.Clamp(n, 10, 1000);
            }
            set
            {
                int n = Lib.Utils.Clamp(value, 10, 1000);
                Properties.Settings.Default.MaxLogLine = n;
                Properties.Settings.Default.Save();
            }
        }


        public string proxyAddr
        {
            get
            {
                string addr = Properties.Settings.Default.ProxyAddr;
                string ip;
                int port;
                Lib.Utils.TryParseIPAddr(addr, out ip, out port);
                return string.Join(":", ip, port);
            }
            set
            {
                Properties.Settings.Default.ProxyAddr = value;
                Properties.Settings.Default.Save();
            }
        }

        public string GetPacUrl()
        {
            // https://pac.txthinking.com/white/SOCKS5%20127.0.0.1:1080
            // https://pac.txthinking.com/white/{0}%20{1}
            // ProxyAddr ProxyType
            string mode = proxyType.Equals("http") ? "PROXY" : "SOCKS5";
            return string.Format(resData("PacUrlTpl"), mode, proxyAddr);
        }

        public string proxyType
        {
            get
            {
                string type = Properties.Settings.Default.ProxyType.ToLower();
                string[] types = { "socks", "http" };
                return types.Contains(type) ? type : types[0];
            }
            set
            {
                Properties.Settings.Default.ProxyType = value;
                Properties.Settings.Default.Save();
            }
        }

        public int GetServeNum()
        {
            return servers.Count;
        }

        public void MoveItemToTop(int index)
        {
            var n = Lib.Utils.Clamp(index, 0, GetServeNum() - 1);

            if (_SelectedServerIndex == n)
            {
                _SelectedServerIndex = 0;
            }
            else if (_SelectedServerIndex < n)
            {
                _SelectedServerIndex++;
            }

            var item = servers[n];
            servers.RemoveAt(n);
            servers.Insert(0, item);
            SaveServers();
            OnSettingChange?.Invoke(this, null);
        }

        public void MoveItemUp(int index)
        {
            if (index < 1 || index > GetServeNum() - 1)
            {
                return;
            }
            if (_SelectedServerIndex == index)
            {
                _SelectedServerIndex--;
            }
            else if (_SelectedServerIndex == index - 1)
            {
                _SelectedServerIndex++;
            }
            var item = servers[index];
            servers.RemoveAt(index);
            servers.Insert(index - 1, item);
            SaveServers();

            OnSettingChange?.Invoke(this, null);
        }

        public void MoveItemDown(int index)
        {
            if (index < 0 || index > GetServeNum() - 2)
            {
                return;
            }
            if (_SelectedServerIndex == index)
            {
                _SelectedServerIndex++;
            }
            else if (_SelectedServerIndex == index + 1)
            {
                _SelectedServerIndex--;
            }
            var item = servers[index];
            servers.RemoveAt(index);
            servers.Insert(index + 1, item);
            SaveServers();
            OnSettingChange?.Invoke(this, null);
        }

        public void MoveItemToButtom(int index)
        {
            var n = Lib.Utils.Clamp(index, 0, GetServeNum() - 1);

            if (_SelectedServerIndex == n)
            {
                _SelectedServerIndex = GetServeNum() - 1;
            }
            else if (_SelectedServerIndex > n)
            {
                _SelectedServerIndex--;
            }
            var item = servers[n];
            servers.RemoveAt(n);
            servers.Add(item);
            SaveServers();
            OnSettingChange?.Invoke(this, null);
        }

        public void DeleteAllServers()
        {
            servers = new List<string>();
            SaveServers();
            OnSettingChange?.Invoke(this, null);
            OnRequireCoreRestart?.Invoke(this, null);
        }

        public void DeleteServer(int index)
        {
            Debug.WriteLine("delete server: " + index);

            if (index < 0 || index >= GetServeNum())
            {
                Debug.WriteLine("delete server index out of range");
                return;
            }

            servers.RemoveAt(index);
            SaveServers();


            if (_SelectedServerIndex >= GetServeNum())
            {
                // normal restart
                _SelectedServerIndex = GetServeNum() - 1;
                OnRequireCoreRestart?.Invoke(this, null);
            }
            else if (_SelectedServerIndex == index)
            {
                // force restart
                OnRequireCoreRestart?.Invoke(this, null);
            }
            else if (_SelectedServerIndex > index)
            {
                // dont need restart
                _SelectedServerIndex--;
            }

            // dont need restart
            OnSettingChange?.Invoke(this, null);
        }

        public bool ImportLinks(string links)
        {
            var vmess = ImportVmessLinks(links);
            var v2ray = ImportV2RayLinks(links);
            return vmess || v2ray;
        }

        bool ImportV2RayLinks(string v2rayLink)
        {
            // string pat = @"v2ray://(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})";
            string pat = resData("V2RayLinkPerfix") + resData("PatternBase64");
            Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = regex.Match(v2rayLink);
            int count = 0;
            string link = String.Empty;
            string config = String.Empty;
            JObject obj = null;
            string content;

            while (match.Success)
            {
                link = v2rayLink.Substring(match.Index, match.Length);
                try
                {
                    content = link.Substring(8);
                    config = Lib.Utils.Base64Decode(content);
                    obj = JObject.Parse(config);
                    if (obj != null)
                    {
                        if (AddServer(content))
                        {
                            count++;
                        }
                    }
                    Debug.WriteLine("Add server: " + link.Substring(0, 32) + " ...");
                }
                catch { }
                match = match.NextMatch();
            }

            return count > 0;
        }

        bool ImportVmessLinks(string vmessLink)
        {
            string pat = resData("VmessLinkPerfix") + resData("PatternBase64");
            Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
            Match match = regex.Match(vmessLink);
            int count = 0;
            string link = String.Empty;
            string config = String.Empty;

            while (match.Success)
            {
                link = vmessLink.Substring(match.Index, match.Length);
                config = Lib.Utils.VmessLink2ConfigString(link);
                if (!string.IsNullOrEmpty(config))
                {
                    if (AddServer(Lib.Utils.Base64Encode(config)))
                    {
                        count++;
                    }
                    Debug.WriteLine("Add server: " + link.Substring(0, 32) + " ...");
                }
                match = match.NextMatch();
            }

            return count > 0;
        }

        void InitServers()
        {
            string raw_data = Properties.Settings.Default.Servers;

            servers = new List<string>();
            List<string> serv = new List<string>();

            try
            {
                serv = JsonConvert.DeserializeObject<List<string>>(raw_data);
                if (serv == null)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("AddServer fail: " + e);
                return;
            }

            List<string> validServers = new List<string>();
            foreach (var s in serv)
            {
                if (IsValidServer(s))
                {
                    validServers.Add(s);
                }
            }
            servers = validServers;
        }

        bool IsValidServer(string b64ConfigString)
        {
            try
            {
                var config = Lib.Utils.Base64Decode(b64ConfigString);
                var obj = JObject.Parse(config);
                return true;
            }
            catch { }
            return false;
        }

        public List<string> GetAllServers()
        {
            return servers;
        }

        public string GetServer(int index)
        {
            if (GetServeNum() == 0
                || index < 0
                || index >= GetServeNum())
            {
                return string.Empty;
            }

            return servers[index];
        }

        public bool AddServer(string b64ConfigString)
        {
            if (servers.IndexOf(b64ConfigString) >= 0)
            {
                Debug.WriteLine("Duplicate server, skip!");
                return false;
            }
            servers.Add(b64ConfigString);
            SaveServers();
            OnSettingChange?.Invoke(this, null);
            return true;
        }

        public void ActivateServer(int index)
        {
            _SelectedServerIndex = index;
            OnRequireCoreRestart?.Invoke(this, null);
            OnSettingChange?.Invoke(this, null);
        }

        public bool ReplaceServer(string b64ConfigString, int replaceIndex)
        {
            if (replaceIndex < 0 || replaceIndex >= GetServeNum())
            {
                return AddServer(b64ConfigString);
            }

            servers[replaceIndex] = b64ConfigString;
            SaveServers();

            if (replaceIndex == _SelectedServerIndex)
            {
                OnRequireCoreRestart?.Invoke(this, null);
            }
            OnSettingChange?.Invoke(this, null);
            return true;
        }

        void SaveServers()
        {
            string json = JsonConvert.SerializeObject(servers);
            Properties.Settings.Default.Servers = json;
            Properties.Settings.Default.Save();
        }
    }
}
