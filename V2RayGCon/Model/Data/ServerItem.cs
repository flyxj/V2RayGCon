using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.Data
{
    public class ServerItem
    {
        public event EventHandler<Model.Data.StrEvent> OnLog;
        public event EventHandler OnPropertyChanged;
        public event EventHandler RequireMenuUpdate;
        public event EventHandler<Model.Data.StrEvent> RequireDelete;

        public string config; // plain text of config.json
        public bool isOn, isInjectEnv, isInjectImport;
        public string name, summary, inboundIP;
        public int inboundOverwriteType, inboundPort, index;

        [JsonIgnore]
        public Model.BaseClass.CoreServer server;

        public ServerItem()
        {
            isOn = false;
            isInjectEnv = false;
            isInjectImport = false;

            name = string.Empty;
            summary = string.Empty;
            config = string.Empty;
            inboundOverwriteType = 0;
            inboundIP = "127.0.0.1";
            inboundPort = 1080;

            server = new BaseClass.CoreServer();
            server.OnLog += SendLogHandler;
            UpdateProperties();
        }

        #region public method
        public void Delete()
        {
            try
            {
                RequireDelete?.Invoke(
                    this,
                    new Model.Data.StrEvent(config));
            }
            catch { }
        }

        public void SetInboundIP(string ip)
        {
            inboundIP = ip;
            InvokePropertyChange();
        }

        public void SetInboundPort(string port)
        {
            inboundPort = Lib.Utils.Str2Int(port);
            InvokePropertyChange();
        }

        public void SetIndex(int index)
        {
            this.index = index;
            InvokePropertyChange();
        }

        public void SetInjectEnv(bool env)
        {
            this.isInjectEnv = env;
            InvokePropertyChange();
        }

        public void SetInjectImport(bool import)
        {
            this.isInjectImport = import;
            InvokePropertyChange();
        }

        public void SetInboundType(int type)
        {
            this.inboundOverwriteType = Lib.Utils.Clamp(type, 0, GetInboundTypeCount());
            InvokePropertyChange();
        }

        public void UpdateProperties()
        {
            Task.Factory.StartNew(() =>
            {
                var configString = isInjectImport ?
                    Lib.Utils.InjectGlobalImport(this.config) :
                    config;
                try
                {
                    SetPropertiesByConfig(
                        Lib.ImportParser.ParseImport(configString));
                }
                catch
                {
                    SetPropertiesByConfig(JObject.Parse(configString));
                }

                InvokePropertyChange();
                InvokeRequireMenuUpdate();
            });
        }

        public void CleanupThen(Action lambda)
        {
            this.server.StopCoreThen(() =>
            {
                this.server.OnLog -= SendLogHandler;
                lambda();
            });
        }

        public void ChangeConfig(string config)
        {
            this.config = config;

            if (server.isRunning)
            {
                server.RestartCore(config, InvokePropertyChange);
            }
            else
            {
                UpdateProperties();
            }
        }

        public string GetInboundOverwriteTypeNameByIndex(int index)
        {
            var types = Model.Data.Table.inboundOverwriteTypes;
            if (index > 0 && index < 3)
            {
                return types[index];
            }
            return types[0];
        }
        #endregion

        #region private method
        void SendLogHandler(object sender, Model.Data.StrEvent arg)
        {
            var log = new Model.Data.StrEvent(
                string.Format("[{0}] {1}", this.name, arg.Data));

            try
            {
                OnLog?.Invoke(this, log);
            }
            catch { }
        }

        void InvokeRequireMenuUpdate()
        {
            try
            {
                RequireMenuUpdate?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        void InvokePropertyChange()
        {
            // things happen while invoking
            try
            {
                OnPropertyChanged?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        void SetPropertiesByConfig(JObject config)
        {
            var name = Lib.Utils.GetValue<string>(config, "v2raygcon.alias");
            this.name = string.IsNullOrEmpty(name) ?
                I18N("Empty") :
                Lib.Utils.CutStr(name, 15);

            var GetStr = Lib.Utils.GetStringByKeyHelper(config);

            var protocol = GetStr("outbound.protocol");
            var ip = string.Empty;
            if (protocol == "vmess" || protocol == "shadowsocks")
            {
                var keys = Table.servInfoKeys[protocol];
                ip = GetStr(keys[0]); // ip
            }

            var summary = string.Format("[{0}] {1}", this.name, protocol);
            if (!string.IsNullOrEmpty(ip))
            {
                summary += "@" + ip;
            }
            this.summary = Lib.Utils.CutStr(summary, 40);
        }

        private int GetInboundTypeCount()
        {
            return Model.Data.Table.inboundOverwriteTypes.Count;
        }
        #endregion
    }
}
