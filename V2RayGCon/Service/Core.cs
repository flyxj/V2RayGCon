using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{

    public class Core : Model.BaseClass.SingletonService<Core>
    {

        Model.BaseClass.CoreServer coreServer;

        Setting setting;
        Cache cache;

        public event EventHandler OnCoreStatChange;

        Core()
        {
            cache = Cache.Instance;

            setting = Setting.Instance;
            setting.OnRequireCoreRestart += (s, a) =>
                Task.Factory.StartNew(() => CoreRestartHandler());

            coreServer = new Model.BaseClass.CoreServer();
            coreServer.OnLog += (s, a) => setting.SendLog(a.Data);
        }

        #region properties
        public bool isRunning
        {
            get => coreServer.isRunning;
        }
        #endregion

        #region private method
        void OverwriteInboundSettings(JObject config)
        {
            var type = setting.proxyType;

            if (!(type == (int)Model.Data.Enum.ProxyTypes.http
                || type == (int)Model.Data.Enum.ProxyTypes.socks))
            {
                return;
            }

            var protocol = Model.Data.Table.proxyTypesString[type];

            Lib.Utils.TryParseIPAddr(setting.proxyAddr, out string ip, out int port);
            var part = protocol + "In";
            try
            {
                config["inbound"]["protocol"] = protocol;
                config["inbound"]["listen"] = ip;
                config["inbound"]["port"] = port;
                config["inbound"]["settings"] =
                    Cache.Instance.tpl.LoadTemplate(part);
                if (type == (int)Model.Data.Enum.ProxyTypes.socks)
                {
                    config["inbound"]["settings"]["ip"] = ip;
                }
            }
            catch
            {
                Debug.WriteLine("Core: Can not set local proxy address");
                MessageBox.Show(I18N("CoreCantSetLocalAddr"));
            }

        }

        void CoreRestartHandler()
        {
            var index = setting.GetCurServIndex();
            var b64Config = setting.GetServer(index);

            if (string.IsNullOrEmpty(b64Config))
            {
                return;
            }

            string plainText = Lib.Utils.Base64Decode(b64Config);
            JObject config = JObject.Parse(plainText);

            try
            {
                config = Lib.ImportParser.ParseImport(config);
                cache.core[b64Config] = config.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch
            {
                setting.SendLog(I18N("DecodeImportFail"));
                var cacheConfig = cache.core[b64Config];
                if (string.IsNullOrEmpty(cacheConfig))
                {
                    StopCoreThen(null);
                    return;
                }
                setting.SendLog(I18N("UsingDecodeCache"));
                config = JObject.Parse(cacheConfig);
            }

            OverwriteInboundSettings(config);

            coreServer.RestartCore(config.ToString(), NotifyStateChange);

        }

        void NotifyStateChange()
        {
            try
            {
                OnCoreStatChange?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }
        #endregion

        #region public method

        public bool IsCoreExist()
        {
            return coreServer.IsExecutableExist();
        }

        public void StopCoreThen(Action lambda)
        {
            coreServer.StopCoreThen(lambda);
        }

        public string GetCoreVersion()
        {
            return coreServer.GetCoreVersion();
        }
        #endregion
    }
}
