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
        private List<string[]> servSummarys;
        public event EventHandler OnSettingChange;
        public event EventHandler<Model.Data.DataEvent> OnLog;
        public event EventHandler OnRequireCoreRestart;

        private int _curEditingIndex;

        Setting()
        {
            _curEditingIndex = -1;

            LoadServers();
            SaveServers();

            aliases = new List<string>();
            servSummarys = new List<string[]>();
            OnServerListChanged();
            servers.ListChanged += OnServerListChanged;
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

        public int GetCurServIndex()
        {
            return _curServIndex;
        }

        int _curServIndex
        {
            get
            {
                // bug: return Lib.Utils.Clamp(n, 0, ServNum);
                return Math.Max(0, Properties.Settings.Default.CurServ);
            }
            set
            {
                int n = Lib.Utils.Clamp(value, 0, GetServerCount());
                Properties.Settings.Default.CurServ = n;
                Properties.Settings.Default.Save();
            }
        }

        public bool isConfigerShowToolsPanel
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

        public string GetCurAlias()
        {
            var aliases = GetAllAliases();
            if (aliases.Count < 1)
            {
                return string.Empty;
            }
            var index = GetCurServIndex();
            index = Lib.Utils.Clamp(index, 0, aliases.Count);
            return aliases[index];
        }

        public string GetInbountInfo()
        {
            var b64 = GetServer(GetCurServIndex());

            if (string.IsNullOrEmpty(b64))
            {
                return string.Empty;
            }

            var config = JObject.Parse(Lib.Utils.Base64Decode(b64));

            try
            {
                var proxy = string.Format(
                    "{0}://{1}:{2}",
                    config["inbound"]["protocol"],
                    config["inbound"]["listen"],
                    config["inbound"]["port"]);

                return proxy;
            }
            catch { }

            return string.Empty;
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
            // https://pac.txthinking.com/white/{SOCKS5}%20{127.0.0.1:1080}

            var http = (int)Model.Data.Enum.ProxyTypes.http;
            string mode = proxyType == http ? "PROXY" : "SOCKS5";
            return string.Format(resData("PacUrlTpl"), mode, proxyAddr);
        }

        public int proxyType
        {
            get
            {
                var type = Properties.Settings.Default.ProxyType;
                return Lib.Utils.Clamp(type, 0, 3);
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
            var n = Lib.Utils.Clamp(index, 0, GetServerCount());

            if (_curServIndex == n)
            {
                _curServIndex = 0;
            }
            else if (_curServIndex < n)
            {
                _curServIndex++;
            }

            var item = servers[n];
            servers.RemoveAtQuiet(n);
            servers.Insert(0, item);
            SaveServers();
            OnSettingChange?.Invoke(this, EventArgs.Empty);
        }

        public void MoveItemUp(int index)
        {
            if (index < 1 || index > GetServerCount() - 1)
            {
                return;
            }
            if (_curServIndex == index)
            {
                _curServIndex--;
            }
            else if (_curServIndex == index - 1)
            {
                _curServIndex++;
            }
            var item = servers[index];
            servers.RemoveAtQuiet(index);
            servers.Insert(index - 1, item);
            SaveServers();

            OnSettingChange?.Invoke(this, EventArgs.Empty);
        }

        public void MoveItemDown(int index)
        {
            if (index < 0 || index > GetServerCount() - 2)
            {
                return;
            }
            if (_curServIndex == index)
            {
                _curServIndex++;
            }
            else if (_curServIndex == index + 1)
            {
                _curServIndex--;
            }
            var item = servers[index];
            servers.RemoveAtQuiet(index);
            servers.Insert(index + 1, item);
            SaveServers();
            OnSettingChange?.Invoke(this, EventArgs.Empty);
        }

        public void MoveItemToButtom(int index)
        {
            var n = Lib.Utils.Clamp(index, 0, GetServerCount());

            if (_curServIndex == n)
            {
                _curServIndex = GetServerCount() - 1;
            }
            else if (_curServIndex > n)
            {
                _curServIndex--;
            }
            var item = servers[n];
            servers.RemoveAtQuiet(n);
            servers.Add(item);
            SaveServers();
            OnSettingChange?.Invoke(this, EventArgs.Empty);
        }

        public void DeleteAllServers()
        {
            servers.Clear();
            SaveServers();
            OnSettingChange?.Invoke(this, EventArgs.Empty);
            OnRequireCoreRestart?.Invoke(this, EventArgs.Empty);
        }

        public void DeleteServer(int index)
        {
            Debug.WriteLine("delete server: " + index);

            if (index < 0 || index >= GetServerCount())
            {
                Debug.WriteLine("index out of range");
                return;
            }

            servers.RemoveAt(index);
            SaveServers();

            if (_curServIndex >= GetServerCount())
            {
                // normal restart
                _curServIndex = GetServerCount() - 1;
                OnRequireCoreRestart?.Invoke(this, EventArgs.Empty);
            }
            else if (_curServIndex == index)
            {
                // force restart
                OnRequireCoreRestart?.Invoke(this, EventArgs.Empty);
            }
            else if (_curServIndex > index)
            {
                // dont need restart
                _curServIndex--;
            }

            // dont need restart
            OnSettingChange?.Invoke(this, EventArgs.Empty);
        }

        public bool ImportLinks(string links)
        {
            var vmess = ImportVmessLinks(links, true);
            var v2ray = ImportV2RayLinks(links, true);
            var ss = ImportSSLinks(links, true);

            if (vmess || v2ray || ss)
            {
                servers.Notify();
                OnSettingChange?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        bool ImportSSLinks(string links, bool quiet = false)
        {
            var isAddNewServer = false;

            string pattern = Lib.Utils.GenPattern(Model.Data.Enum.LinkTypes.ss);
            var matches = Regex.Matches("\n" + links, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string link = match.Value.Substring(1);
                string config = Lib.Utils.SSLink2ConfigString(link);
                if (string.IsNullOrEmpty(config))
                {
                    continue;
                }
                var msg = Lib.Utils.CutStr(link, 48) + " ...";
                SendLog(I18N("AddServer") + ": " + msg);
                if (AddServer(Lib.Utils.Base64Encode(config), quiet))
                {
                    isAddNewServer = true;
                }
            }

            if (!quiet && isAddNewServer)
            {
                servers.Notify();
            }

            return isAddNewServer;
        }

        bool ImportV2RayLinks(string links, bool quiet = false)
        {
            bool isAddNewServer = false;

            string pattern = Lib.Utils.GenPattern(Model.Data.Enum.LinkTypes.v2ray);
            var matches = Regex.Matches("\n" + links, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                try
                {
                    string link = match.Value.Substring(1);

                    string b64Config = Lib.Utils.GetLinkBody(link);
                    string config = Lib.Utils.Base64Decode(b64Config);
                    if (JObject.Parse(config) != null)
                    {
                        var msg = Lib.Utils.CutStr(link, 48) + " ...";
                        SendLog(I18N("AddServer") + ": " + msg);
                        if (AddServer(b64Config, quiet))
                        {
                            isAddNewServer = true;
                        }
                    }
                }
                catch
                {
                    // skip if error occured
                }
            }

            if (!quiet && isAddNewServer)
            {
                servers.Notify();
            }

            return isAddNewServer;
        }

        bool ImportVmessLinks(string links, bool quiet = false)
        {
            var isAddNewServer = false;

            string pattern = Lib.Utils.GenPattern(Model.Data.Enum.LinkTypes.vmess);
            var matches = Regex.Matches("\n" + links, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string link = match.Value.Substring(1);
                string config = Lib.Utils.VmessLink2ConfigString(link);
                if (string.IsNullOrEmpty(config))
                {
                    continue;
                }
                var msg = Lib.Utils.CutStr(link, 48) + " ...";
                SendLog(I18N("AddServer") + ": " + msg);

                if (AddServer(Lib.Utils.Base64Encode(config), quiet))
                {
                    isAddNewServer = true;
                }
            }

            if (!quiet && isAddNewServer)
            {
                servers.Notify();
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

        public ReadOnlyCollection<string[]> GetAllServSummarys()
        {
            return servSummarys.AsReadOnly();
        }

        void OnServerListChanged()
        {


            aliases.Clear();
            servSummarys.Clear();

            for (int i = 0; i < servers.Count; i++)
            {
                try
                {
                    var configString = Lib.Utils.Base64Decode(servers[i]);
                    var config = JObject.Parse(configString);

                    var GetStr = Lib.Utils.GetStringByKeyHelper(config);

                    var name = GetStr("v2raygcon.alias");

                    var alias = string.IsNullOrEmpty(name) ?
                        I18N("Empty") :
                        Lib.Utils.CutStr(name, 20);

                    aliases.Add(string.Join(".", i + 1, alias));

                    var protocol = GetStr("outbound.protocol");
                    var keys = Model.Data.Table.servInfoKeys["shadowsocks"];
                    if (protocol.Equals("vmess"))
                    {
                        keys = Model.Data.Table.servInfoKeys["vmess"];
                    }

                    var summary = new string[6];

                    for (int j = 0; j < 6; j++)
                    {
                        summary[j] = GetStr(keys[j]);
                    }
                    servSummarys.Add(new string[] {
                        (i+1).ToString(),
                        alias,
                        protocol,
                        summary[0],  // ip
                        summary[1],  // port
                        string.Empty, // selected
                        summary[3],  // path
                        summary[4],  // streamType
                        summary[2],  // tls/enc
                        summary[5],  // type /disguise
                    });
                }
                catch { }

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

        public bool AddServer(string b64ConfigString, bool quiet = false)
        {
            if (servers.IndexOf(b64ConfigString) >= 0)
            {
                // Debug.WriteLine("Duplicate server, skip!");
                SendLog(I18N("DuplicateServer") + "\r\n");
                return false;
            }
            if (quiet)
            {
                servers.AddQuiet(b64ConfigString);
                SaveServers();
            }
            else
            {
                servers.Add(b64ConfigString);
                SaveServers();
                OnSettingChange?.Invoke(this, EventArgs.Empty);
            }
            SendLog(I18N("AddServSuccess") + "\r\n");
            return true;
        }

        public void ActivateServer(int index = -1)
        {
            if (index >= 0)
            {
                _curServIndex = index;
            }
            OnRequireCoreRestart?.Invoke(this, EventArgs.Empty);
            OnSettingChange?.Invoke(this, EventArgs.Empty);
        }

        public bool ReplaceServer(string b64ConfigString, int replaceIndex)
        {
            if (replaceIndex < 0 || replaceIndex >= GetServerCount())
            {
                return AddServer(b64ConfigString);
            }

            servers[replaceIndex] = b64ConfigString;
            SaveServers();

            if (replaceIndex == _curServIndex)
            {
                OnRequireCoreRestart?.Invoke(this, EventArgs.Empty);
            }
            OnSettingChange?.Invoke(this, EventArgs.Empty);
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
