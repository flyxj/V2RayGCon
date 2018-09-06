using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.Data
{
    public class ServerItem
    {
        public event EventHandler<Model.Data.StrEvent> OnLog, OnRequireDeleteServer;
        public event EventHandler OnPropertyChanged, OnRequireMenuUpdate;

        public string config; // plain text of config.json
        public bool isAutoRun, isInjectImport, isSelected;
        public string name, summary, inboundIP;
        public int overwriteInboundType, inboundPort, index;

        [JsonIgnore]
        public string status;

        [JsonIgnore]
        public bool isServerOn;

        [JsonIgnore]
        public Model.BaseClass.CoreServer server;

        public ServerItem()
        {
            // new ServerItem will display at the bottom
            index = int.MaxValue;

            isSelected = false;
            isServerOn = false;
            isAutoRun = false;
            isInjectImport = false;

            status = string.Empty;
            name = string.Empty;
            summary = string.Empty;
            config = string.Empty;
            overwriteInboundType = 0;
            inboundIP = "127.0.0.1";
            inboundPort = 1080;

            server = new BaseClass.CoreServer();
            server.OnLog += SendLogHandler;
        }

        #region public method
        public void DoSpeedTestThen(Action next = null)
        {
            void error(string msg)
            {
                SendLog(msg);
                SetStatus(msg);
                next?.Invoke();
            }

            var cfg = GetDecodedConfig();

            if (cfg == null)
            {
                error(I18N("DecodeImportFail"));
                return;
            }

            var port = Lib.Utils.GetFreeTcpPort();
            if (port <= 0)
            {
                error(I18N("GetFreePortFail"));
                return;
            }

            if (!OverwriteInboundSettings(
                ref cfg,
                (int)Model.Data.Enum.ProxyTypes.HTTP,
                "127.0.0.1",
                port))
            {
                error(I18N("CoreCantSetLocalAddr"));
                return;
            }

            SetStatus(I18N("Testing"));

            var tester = new Model.BaseClass.CoreServer();

            tester.RestartCoreThen(cfg.ToString(), null, () =>
            {
                var time = Lib.Utils.VisitWebPageSpeedTest(
                    StrConst("SpeedTestUrl"),
                    string.Format("127.0.0.1:{0}", port));

                var msg = string.Format("{0}:{1}",
                    I18N("VisitWebPageTest"),
                    time > 0 ? time.ToString() + "ms" : I18N("Timeout"));

                SetStatus(msg);
                SendLog(msg);
                tester.StopCoreThen(next);
            });
        }

        public void DeleteSelf()
        {
            OnRequireDeleteServer?.Invoke(this, new StrEvent(config));
        }

        public void SetInboundIP(string ip)
        {
            // prevent infinite prompt
            if (this.inboundIP == ip)
            {
                return;
            }

            if (NeedStopCoreFirst())
            {
                InvokeEventOnPropertyChange(); // refresh ui
                return;
            }

            inboundIP = ip;
            InvokeEventOnPropertyChange();
        }

        public void SetInboundPort(string port)
        {
            var p = Lib.Utils.Str2Int(port);
            if (inboundPort == p)
            {
                return;
            }

            if (NeedStopCoreFirst())
            {
                InvokeEventOnPropertyChange();
                return;
            }

            inboundPort = p;
            InvokeEventOnPropertyChange();
        }

        public void SetStatus(string status)
        {
            if (this.status == status)
            {
                return;
            }
            this.status = status;
            InvokeEventOnPropertyChange();
        }

        public void SetIndex(int index)
        {
            if (this.index == index)
            {
                return;
            }
            this.index = index;
            InvokeEventOnPropertyChange();
        }

        public void SetSelected(bool selected)
        {
            if (this.isSelected == selected)
            {
                return;
            }

            this.isSelected = selected;
            InvokeEventOnPropertyChange();
        }

        public void SetAutoRun(bool autorun)
        {
            if (this.isAutoRun == autorun)
            {
                return;
            }

            this.isAutoRun = autorun;
            InvokeEventOnPropertyChange();
        }

        public void SetInjectImport(bool import)
        {
            if (this.isInjectImport == import)
            {
                return;
            }

            this.isInjectImport = import;

            // refresh UI immediately
            InvokeEventOnPropertyChange();

            // time consuming things
            if (isServerOn)
            {
                RestartCoreThen();
            }

            UpdateSummaryThen(() => InvokeEventOnRequireMenuUpdate());
        }

        public void SetOverwriteInboundType(int type)
        {
            if (this.overwriteInboundType == type)
            {
                return;
            }

            this.overwriteInboundType = Lib.Utils.Clamp(
                type, 0, Model.Data.Table.inboundOverwriteTypesName.Length);

            InvokeEventOnPropertyChange();
            if (isServerOn)
            {
                // time consuming things
                RestartCoreThen();
            }
        }

        public void UpdateSummaryThen(Action lambda = null)
        {
            Task.Factory.StartNew(() =>
            {
                var configString = isInjectImport ?
                    Lib.Utils.InjectGlobalImport(this.config) :
                    this.config;
                try
                {
                    UpdateSummary(
                        Lib.ImportParser.Parse(configString));
                }
                catch
                {
                    UpdateSummary(JObject.Parse(configString));
                }

                InvokeEventOnPropertyChange();
                lambda?.Invoke();
            });
        }

        public void CleanupThen(Action next)
        {
            this.server.StopCoreThen(() =>
            {
                this.server.OnLog -= SendLogHandler;
                Task.Factory.StartNew(() =>
                {
                    next?.Invoke();
                });
            });
        }

        public void ChangeConfig(string config)
        {
            if (this.config == config)
            {
                return;
            }

            this.config = config;
            InvokeEventOnPropertyChange();
            UpdateSummaryThen(() =>
            {
                InvokeEventOnRequireMenuUpdate();
            });

            if (server.isRunning)
            {
                RestartCoreThen();
            }
        }

        public void StopCoreThen(Action next = null)
        {
            Task.Factory.StartNew(() => server.StopCoreThen(next));
        }

        public void RestartCoreThen(Action next = null)
        {
            Task.Factory.StartNew(() => RestartCoreWorker(next));
        }

        public void RestartCoreWorker(Action next)
        {
            JObject cfg = GetDecodedConfig(true);
            if (cfg == null)
            {
                StopCoreThen(next);
                return;
            }

            if (!OverwriteInboundSettings(
                ref cfg,
                overwriteInboundType,
                this.inboundIP,
                this.inboundPort))
            {
                StopCoreThen(next);
                return;
            }

            server.RestartCoreThen(
                cfg.ToString(),
                OnCoreStateChanged,
                next,
                Lib.Utils.GetEnvVarsFromConfig(cfg));
        }

        public void OnCoreStateChanged()
        {
            isServerOn = server.isRunning;
            InvokeEventOnPropertyChange();
        }
        #endregion

        #region private method
        JObject GetDecodedConfig(bool isUseCache = false)
        {
            var cache = Service.Cache.Instance.core;

            JObject cfg = null;
            try
            {
                cfg = Lib.ImportParser.Parse(
                    isInjectImport ?
                    Lib.Utils.InjectGlobalImport(config) :
                    config);

                cache[config] = cfg.ToString(Formatting.None);

            }
            catch { }

            if (cfg == null)
            {
                SendLog(I18N("DecodeImportFail"));
                if (isUseCache)
                {
                    try
                    {
                        cfg = JObject.Parse(cache[config]);
                    }
                    catch (KeyNotFoundException) { }
                    SendLog(I18N("UsingDecodeCache"));
                }
            }

            return cfg;
        }

        bool NeedStopCoreFirst()
        {
            if (!isServerOn)
            {
                return false;
            }

            if (overwriteInboundType != (int)Model.Data.Enum.ProxyTypes.HTTP
                && overwriteInboundType != (int)Model.Data.Enum.ProxyTypes.SOCKS)
            {
                return false;

            }

            Task.Factory.StartNew(() => MessageBox.Show(I18N("StopServerFirst")));
            return true;
        }

        bool OverwriteInboundSettings(
            ref JObject config,
            int inboundType,
            string ip,
            int port)
        {
            var type = (Model.Data.Enum.ProxyTypes)inboundType;

            if (type != Model.Data.Enum.ProxyTypes.HTTP
                && type != Model.Data.Enum.ProxyTypes.SOCKS)
            {
                return true;
            }

            var protocol = Model.Data.Table.inboundOverwriteTypesName[(int)type];

            var part = protocol + "In";
            try
            {
                var o = Lib.Utils.CreateJObject("inbound");
                o["inbound"]["protocol"] = protocol;
                o["inbound"]["listen"] = ip;
                o["inbound"]["port"] = port;
                o["inbound"]["settings"] =
                    Service.Cache.Instance.tpl.LoadTemplate(part);

                if (type == Model.Data.Enum.ProxyTypes.SOCKS)
                {
                    o["inbound"]["settings"]["ip"] = ip;
                }

                Lib.Utils.MergeJson(ref config, o);

                return true;
            }
            catch
            {
                SendLog(I18N("CoreCantSetLocalAddr"));
            }
            return false;
        }

        void SendLog(string message)
        {
            var log = new Model.Data.StrEvent(
                string.Format("[{0}] {1}", this.name, message));

            try
            {
                OnLog?.Invoke(this, log);
            }
            catch { }
        }

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

        void InvokeEventOnRequireMenuUpdate()
        {
            OnRequireMenuUpdate?.Invoke(this, EventArgs.Empty);
        }

        void InvokeEventOnPropertyChange()
        {
            // things happen while invoking
            try
            {
                OnPropertyChanged?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        void UpdateSummary(JObject config)
        {
            var name = Lib.Utils.GetAliasFromConfig(config);
            var summary = string.Format("[{0}] {1}", name, Lib.Utils.GetSummaryFromConfig(config));

            this.name = name;
            this.summary = Lib.Utils.CutStr(summary, 39);
        }


        #endregion
    }
}
