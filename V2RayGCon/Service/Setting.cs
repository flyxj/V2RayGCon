using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Setting : Model.BaseClass.SingletonService<Setting>
    {

        private Model.Data.EventList<string> servers;
        private List<string> aliases;
        public event EventHandler OnSettingChange;
        public event EventHandler<Model.Data.DataEvent> OnLog;
        public event EventHandler OnRequireCoreRestart;

        private int _curEditingIndex;

        Setting()
        {
            _curEditingIndex = -1;

            LoadServers();
            aliases = new List<string>();
            UpdateAliases();
            servers.ListChanged += UpdateAliases;
            SaveServers();
        }

        public void SendLog(string log)
        {
            var arg = new Model.Data.DataEvent(log);
            OnLog?.Invoke(this, arg);
        }

        public int curEditingIndex
        {
            get
            {
                return _curEditingIndex;
            }
            set
            {
                if (value >= 0 && value < GetServerCount())
                {
                    _curEditingIndex = value;
                }
                else
                {
                    _curEditingIndex = -1;
                }
            }
        }

        int _selectedServerIndex
        {
            get
            {
                // bug: return Lib.Utils.Clamp(n, 0, ServNum - 1);
                return Math.Max(0, Properties.Settings.Default.CurServ);
            }
            set
            {
                int n = Lib.Utils.Clamp(value, 0, GetServerCount());
                Properties.Settings.Default.CurServ = n;
                Properties.Settings.Default.Save();
            }
        }

        public bool isShowConfigerLeftPanel
        {
            get
            {
                return Properties.Settings.Default.CfgShowToolPanel == true;
            }
            set
            {
                Properties.Settings.Default.CfgShowToolPanel = value;
                Properties.Settings.Default.Save();
            }
        }

        public int GetSelectedServerIndex()
        {
            return _selectedServerIndex;
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
                Lib.Utils.TryParseIPAddr(addr, out string ip, out int port);
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

            string mode =
                proxyType == (int)Model.Data.Enum.ProxyTypes.http ?
                "PROXY" : "SOCKS5";
            return string.Format(resData("PacUrlTpl"), mode, proxyAddr);
        }

        public int proxyType
        {
            get
            {
                return Lib.Utils.Clamp(Properties.Settings.Default.ProxyType, 0, 2);
            }
            set
            {
                Properties.Settings.Default.ProxyType = value;
                Properties.Settings.Default.Save();
            }
        }

        public int GetServerCount()
        {
            return servers.Count;
        }

        public void MoveItemToTop(int index)
        {
            var n = Lib.Utils.Clamp(index, 0, GetServerCount() - 1);

            if (_selectedServerIndex == n)
            {
                _selectedServerIndex = 0;
            }
            else if (_selectedServerIndex < n)
            {
                _selectedServerIndex++;
            }

            var item = servers[n];
            servers.RemoveAt(n);
            servers.Insert(0, item);
            SaveServers();
            OnSettingChange?.Invoke(this, null);
        }

        public void MoveItemUp(int index)
        {
            if (index < 1 || index > GetServerCount() - 1)
            {
                return;
            }
            if (_selectedServerIndex == index)
            {
                _selectedServerIndex--;
            }
            else if (_selectedServerIndex == index - 1)
            {
                _selectedServerIndex++;
            }
            var item = servers[index];
            servers.RemoveAt(index);
            servers.Insert(index - 1, item);
            SaveServers();

            OnSettingChange?.Invoke(this, null);
        }

        public void MoveItemDown(int index)
        {
            if (index < 0 || index > GetServerCount() - 2)
            {
                return;
            }
            if (_selectedServerIndex == index)
            {
                _selectedServerIndex++;
            }
            else if (_selectedServerIndex == index + 1)
            {
                _selectedServerIndex--;
            }
            var item = servers[index];
            servers.RemoveAt(index);
            servers.Insert(index + 1, item);
            SaveServers();
            OnSettingChange?.Invoke(this, null);
        }

        public void MoveItemToButtom(int index)
        {
            var n = Lib.Utils.Clamp(index, 0, GetServerCount() - 1);

            if (_selectedServerIndex == n)
            {
                _selectedServerIndex = GetServerCount() - 1;
            }
            else if (_selectedServerIndex > n)
            {
                _selectedServerIndex--;
            }
            var item = servers[n];
            servers.RemoveAt(n);
            servers.Add(item);
            SaveServers();
            OnSettingChange?.Invoke(this, null);
        }

        public void DeleteAllServers()
        {
            servers.Clear();
            SaveServers();
            OnSettingChange?.Invoke(this, null);
            OnRequireCoreRestart?.Invoke(this, null);
        }

        public void DeleteServer(int index)
        {
            Debug.WriteLine("delete server: " + index);

            if (index < 0 || index >= GetServerCount())
            {
                Debug.WriteLine("delete server index out of range");
                return;
            }

            servers.RemoveAt(index);
            SaveServers();


            if (_selectedServerIndex >= GetServerCount())
            {
                // normal restart
                _selectedServerIndex = GetServerCount() - 1;
                OnRequireCoreRestart?.Invoke(this, null);
            }
            else if (_selectedServerIndex == index)
            {
                // force restart
                OnRequireCoreRestart?.Invoke(this, null);
            }
            else if (_selectedServerIndex > index)
            {
                // dont need restart
                _selectedServerIndex--;
            }

            // dont need restart
            OnSettingChange?.Invoke(this, null);
        }

        public bool ImportLinks(string links)
        {
            // dirty hack for matching the first link
            var patchLinks = "\r\n" + links;

            var vmess = ImportVmessLinks(patchLinks);
            var v2ray = ImportV2RayLinks(patchLinks);
            var ss = ImportSSLinks(patchLinks);
            return vmess || v2ray || ss;
        }

        bool ImportSSLinks(string links)
        {
            var isAddNewServer = false;

            string pattern = string.Format(
                "{0}{1}{2}",
                // distinguish between ss:// and vme(ss://)
                resData("PatternNonAlphabet"),
                resData("SSLinkPrefix"),
                resData("PatternBase64"));

            foreach (Match m in Regex.Matches(links, pattern, RegexOptions.IgnoreCase))
            {
                string link = m.Value.Substring(1);
                string config = Lib.Utils.SSLink2ConfigString(link);
                if (!string.IsNullOrEmpty(config) && AddServer(Lib.Utils.Base64Encode(config)))
                {
                    isAddNewServer = true;
                    Debug.WriteLine("New server: " + Lib.Utils.CutString(link, 32) + " ...");
                }
            }

            return isAddNewServer;
        }

        bool ImportV2RayLinks(string links)
        {
            bool isAddNewServer = false;

            // @"[^0-9a-zA-Z]v2ray://(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})";
            string pattern = string.Format(
                "{0}{1}{2}",
                resData("PatternNonAlphabet"),
                resData("V2RayLinkPrefix"),
                resData("PatternBase64"));

            foreach (Match m in Regex.Matches(links, pattern, RegexOptions.IgnoreCase))
            {
                try
                {
                    string link = m.Value.Substring(1);

                    string b64Config = Lib.Utils.LinkBody(link, Model.Data.Enum.LinkTypes.v2ray);
                    string config = Lib.Utils.Base64Decode(b64Config);
                    if (JObject.Parse(config) != null && AddServer(b64Config))
                    {
                        isAddNewServer = true;
                        Debug.WriteLine("New server: " + Lib.Utils.CutString(link, 32) + " ...");
                    }
                }
                catch
                {
                    // skip if error occured
                }
            }

            return isAddNewServer;
        }

        bool ImportVmessLinks(string links)
        {
            var isAddNewServer = false;

            //string pattern = resData("VmessLinkPrefix") + resData("PatternBase64");

            string pattern = string.Format(
                "{0}{1}{2}",
                resData("PatternNonAlphabet"),
                resData("VmessLinkPrefix"),
                resData("PatternBase64"));

            foreach (Match m in Regex.Matches(links, pattern, RegexOptions.IgnoreCase))
            {
                string link = m.Value.Substring(1);
                string config = Lib.Utils.VmessLink2ConfigString(link);
                if (!string.IsNullOrEmpty(config) && AddServer(Lib.Utils.Base64Encode(config)))
                {
                    isAddNewServer = true;
                    Debug.WriteLine("New server: " + Lib.Utils.CutString(link, 32) + " ...");
                }
            }

            return isAddNewServer;
        }

        public ReadOnlyCollection<string> GetAllAliases()
        {
            return aliases.AsReadOnly();
        }

        public void LoadServers()
        {

            servers = new Model.Data.EventList<string>();

            string rawData = Properties.Settings.Default.Servers;

            List<string> serverList = null;
            try
            {
                serverList = JsonConvert.DeserializeObject<List<string>>(rawData);
                if (serverList == null)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Read server list in settings fail: " + e);
                return;
            }

            // make sure every server config can be parsed
            for (var i = serverList.Count - 1; i >= 0; i--)
            {
                try
                {
                    var config = Lib.Utils.Base64Decode(serverList[i]);
                    var obj = JObject.Parse(config);
                    if (obj == null)
                    {
                        serverList.RemoveAt(i);
                    }
                }
                catch
                {
                    serverList.RemoveAt(i);
                }
            }

            servers = new Model.Data.EventList<string>(serverList);
        }

        void UpdateAliases()
        {
            aliases.Clear();

            for (int i = 0; i < servers.Count; i++)
            {
                try
                {
                    var configString = Lib.Utils.Base64Decode(servers[i]);
                    var config = JObject.Parse(configString);
                    var name = Lib.Utils.GetStrFromJToken(config, "v2raygcon.alias");
                    if (!string.IsNullOrEmpty(name))
                    {
                        aliases.Add(string.Join(".", i + 1,
                            Lib.Utils.CutString(name, 20)));
                        continue;
                    }

                }
                catch { }
                aliases.Add(string.Join(".", i + 1, I18N("Empty")));
            }
        }

        public ReadOnlyCollection<string> GetAllServers()
        {
            return servers.AsReadOnly();
        }

        public string GetServer(int index)
        {
            if (GetServerCount() == 0
                || index < 0
                || index >= GetServerCount())
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

        public void ActivateServer(int index = -1)
        {
            if (index >= 0)
            {
                _selectedServerIndex = index;
            }
            OnRequireCoreRestart?.Invoke(this, null);
            OnSettingChange?.Invoke(this, null);
        }

        public bool ReplaceServer(string b64ConfigString, int replaceIndex)
        {
            if (replaceIndex < 0 || replaceIndex >= GetServerCount())
            {
                return AddServer(b64ConfigString);
            }

            servers[replaceIndex] = b64ConfigString;
            SaveServers();

            if (replaceIndex == _selectedServerIndex)
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
