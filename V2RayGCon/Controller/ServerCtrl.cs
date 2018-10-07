using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller
{
    public class ServerCtrl
    {
        public event EventHandler<Model.Data.StrEvent> OnLog;
        public event EventHandler
            OnPropertyChanged,
            OnRequireStatusBarUpdate,
            OnRequireMenuUpdate;

        public string config; // plain text of config.json
        public bool isAutoRun, isInjectImport, isSelected, isCollapse, isInjectSkipCNSite;
        public string name, summary, inboundIP, mark;
        public int overwriteInboundType, inboundPort;
        public double index;



        public ServerCtrl()
        {
            // new ServerItem will display at the bottom
            index = int.MaxValue;

            isSelected = false;
            isServerOn = false;
            isAutoRun = false;
            isInjectImport = false;
            isCollapse = false;

            mark = string.Empty;
            status = string.Empty;
            name = string.Empty;
            summary = string.Empty;
            config = string.Empty;
            speedTestResult = -1;

            overwriteInboundType = 0;
            inboundIP = "127.0.0.1";
            inboundPort = 1080;

            server = new Service.Core();
            server.OnLog += OnLogHandler;
            server.OnCoreStatusChanged += OnCoreStateChangedHandler;
        }

        #region non-serialize properties
        [JsonIgnore]
        public long speedTestResult;

        [JsonIgnore]
        public Service.Servers parent = null;

        [JsonIgnore]
        Views.WinForms.FormSingleServerLog logForm = null;

        [JsonIgnore]
        public string status;

        [JsonIgnore]
        public bool isServerOn;

        [JsonIgnore]
        public Service.Core server;

        [JsonIgnore]
        ConcurrentQueue<string> _logCache = new ConcurrentQueue<string>();

        [JsonIgnore]
        public string logCache
        {
            get
            {
                return string.Join(Environment.NewLine, _logCache)
                    + System.Environment.NewLine;
            }
            private set
            {
                // keep 200 lines of log
                if (_logCache.Count > 300)
                {
                    var blackHole = "";
                    for (var i = 0; i < 100; i++)
                    {
                        _logCache.TryDequeue(out blackHole);
                    }
                }
                _logCache.Enqueue(value);
            }
        }
        #endregion

        #region public method
        public void SetIsSelected(bool selected)
        {
            if (selected == isSelected)
            {
                return;
            }
            this.isSelected = selected;
            InvokeEventOnRequireStatusBarUpdate();
            InvokeEventOnPropertyChange();
        }

        public bool ShowLogForm()
        {
            if (logForm != null)
            {
                return false;
            }
            logForm = new Views.WinForms.FormSingleServerLog(this);

            logForm.FormClosed += (s, a) =>
            {
                logForm.Dispose();
                logForm = null;
            };
            return true;
        }

        string PrepareSpeedTestConfig(int port)
        {
            var empty = string.Empty;
            if (port <= 0)
            {
                return empty;
            }

            var config = GetDecodedConfig(true, true, false);

            if (config == null)
            {
                return empty;
            }

            if (!OverwriteInboundSettings(
                ref config,
                (int)Model.Data.Enum.ProxyTypes.HTTP,
                "127.0.0.1",
                port))
            {
                return empty;
            }

            return config.ToString(Formatting.None);
        }

        public void RunSpeedTest()
        {
            void log(string msg)
            {
                SendLog(msg);
                SetPropertyOnDemand(ref status, msg);
            }

            var port = Lib.Utils.GetFreeTcpPort();
            var config = PrepareSpeedTestConfig(port);

            if (string.IsNullOrEmpty(config))
            {
                log(I18N("DecodeImportFail"));
                return;
            }

            var url = StrConst("SpeedTestUrl");
            var text = I18N("Testing") + " ...";
            log(text);
            SendLog(url);

            var speedTester = new Service.Core();
            speedTester.OnLog += OnLogHandler;
            speedTester.RestartCore(config);

            this.speedTestResult = Lib.Utils.VisitWebPageSpeedTest(url, port);
            text = string.Format("{0}:{1}",
                I18N("VisitWebPageTest"),
                speedTestResult < long.MaxValue ? speedTestResult.ToString() + "ms" : I18N("Timeout"));
            log(text);
            speedTester.StopCore();
            speedTester.OnLog -= OnLogHandler;
        }

        public void SetPropertyOnDemand(ref string property, string value, bool isNeedCoreStopped = false)
        {
            SetPropertyOnDemand<string>(ref property, value, isNeedCoreStopped);
        }

        public void SetPropertyOnDemand(ref int property, int value, bool isNeedCoreStopped = false)
        {
            SetPropertyOnDemand<int>(ref property, value, isNeedCoreStopped);
        }

        public void SetPropertyOnDemand(ref double property, double value, bool isNeedCoreStopped = false)
        {
            SetPropertyOnDemand<double>(ref property, value, isNeedCoreStopped);
        }

        public void SetPropertyOnDemand(ref bool property, bool value, bool isNeedCoreStopped = false)
        {
            SetPropertyOnDemand<bool>(ref property, value, isNeedCoreStopped);
        }

        public void ToggleIsInjectSkipCNSite()
        {
            this.isInjectSkipCNSite = !this.isInjectSkipCNSite;

            // refresh UI immediately
            InvokeEventOnPropertyChange();

            // time consuming things
            if (isServerOn)
            {
                RestartCoreThen();
            }
        }

        public void ToggleIsCollapse()
        {
            this.isCollapse = !this.isCollapse;
            InvokeEventOnPropertyChange();
        }

        public void ToggleIsAutorun()
        {
            this.isAutoRun = !this.isAutoRun;
            InvokeEventOnPropertyChange();
        }

        public void ToggleIsInjectImport()
        {
            this.isInjectImport = !this.isInjectImport;

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
                    Lib.Utils.InjectGlobalImport(this.config, false, true) :
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

                this.status = string.Empty;
                InvokeEventOnPropertyChange();
                lambda?.Invoke();
            });
        }

        public void CleanupThen(Action next)
        {
            this.server.StopCoreThen(() =>
            {
                this.server.OnLog -= OnLogHandler;
                this.server.OnCoreStatusChanged -= OnCoreStateChangedHandler;
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

        public void GetProxyAddrThen(Action<string> next)
        {
            if (overwriteInboundType == (int)Model.Data.Enum.ProxyTypes.HTTP
                || overwriteInboundType == (int)Model.Data.Enum.ProxyTypes.SOCKS)
            {
                next(string.Format(
                    "[{0}] {1}://{2}:{3}",
                    this.name,
                    Model.Data.Table.inboundOverwriteTypesName[overwriteInboundType],
                    this.inboundIP,
                    this.inboundPort));
                return;
            }

            var serverName = this.name;
            Task.Factory.StartNew(() =>
            {
                var cfg = GetDecodedConfig(true, false, true);
                var protocol = Lib.Utils.GetValue<string>(cfg, "inbound.protocol");
                var listen = Lib.Utils.GetValue<string>(cfg, "inbound.listen");
                var port = Lib.Utils.GetValue<string>(cfg, "inbound.port");

                if (string.IsNullOrEmpty(listen))
                {
                    next(string.Format("[{0}] {1}", serverName, protocol));
                    return;
                }

                next(string.Format("[{0}] {1}://{2}:{3}", serverName, protocol, listen, port));
            });
        }

        public void SetMark(string mark)
        {
            if (this.mark == mark)
            {
                return;
            }

            this.mark = mark;
            if (!string.IsNullOrEmpty(mark)
                && !(this.parent.GetMarkList().Contains(mark)))
            {
                this.parent.UpdateMarkList();
            }
            InvokeEventOnPropertyChange();
        }

        public void InvokeEventOnPropertyChange()
        {
            // things happen while invoking
            try
            {
                OnPropertyChanged?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        public void ChangeIndex(double index)
        {
            if (this.index == index)
            {
                return;
            }

            this.index = index;
            InvokeEventOnPropertyChange();
        }

        public List<string> GetSearchTextList()
        {
            return new List<string> {
                // index 0
                name+summary,

                // index 1
                Model.Data.Table.inboundOverwriteTypesName[overwriteInboundType]
                +inboundIP
                +inboundPort.ToString(),

                // index 2
                this.mark,
            };
        }

        public string GetTitle()
        {
            var result = string.Format("{0}.[{1}] {2}",
                (int)this.index,
                this.name,
                this.summary);
            return Lib.Utils.CutStr(result, 60);
        }
        #endregion

        #region private method
        void RestartCoreWorker(Action next)
        {
            JObject cfg = GetDecodedConfig(true, false, true);
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

            InjectSkipCNSite(ref cfg);

            server.RestartCoreThen(
                cfg.ToString(),
                next,
                Lib.Utils.GetEnvVarsFromConfig(cfg));
        }

        void InjectSkipCNSite(ref JObject config)
        {
            if (!this.isInjectSkipCNSite)
            {
                return;
            }

            // copy from Controller.ConfigerComponent.Quick.cs
            var c = JObject.Parse(@"{}");

            var dict = new Dictionary<string, string> {
                { "dns","dnsCFnGoogle" },
                { "routing","routeCNIP" },
                { "outboundDetour","outDtrFreedom" },
            };

            var cache = Service.Cache.Instance;
            foreach (var item in dict)
            {
                var tpl = Lib.Utils.CreateJObject(item.Key);
                var value = cache.tpl.LoadExample(item.Value);
                tpl[item.Key] = value;

                if (!Lib.Utils.Contains(config, tpl))
                {
                    c[item.Key] = value;
                }
            }

            Lib.Utils.CombineConfig(ref config, c);
        }

        void SetPropertyOnDemand<T>(
            ref T property,
            T value,
            bool isNeedCoreStopped)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
            {
                return;
            }

            if (isNeedCoreStopped)
            {
                if (NeedStopCoreFirst())
                {
                    InvokeEventOnPropertyChange(); // refresh ui
                    return;
                }
            }

            property = value;
            InvokeEventOnPropertyChange();
        }

        JObject GetDecodedConfig(bool isUseCache, bool isIncludeSpeedTest, bool isIncludeActivate)
        {
            var cache = Service.Cache.Instance.core;

            JObject cfg = null;
            try
            {
                cfg = Lib.ImportParser.Parse(
                    isInjectImport ?
                    Lib.Utils.InjectGlobalImport(config, isIncludeSpeedTest, isIncludeActivate) :
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
            OnLogHandler(this, new Model.Data.StrEvent(message));
        }

        void OnLogHandler(object sender, Model.Data.StrEvent arg)
        {
            var msg = string.Format("[{0}] {1}", this.name, arg.Data);

            logCache = msg;
            try
            {
                OnLog?.Invoke(this, new Model.Data.StrEvent(msg));
            }
            catch { }
        }

        void InvokeEventOnRequireStatusBarUpdate()
        {
            OnRequireStatusBarUpdate?.Invoke(this, EventArgs.Empty);
        }

        void InvokeEventOnRequireMenuUpdate()
        {
            OnRequireMenuUpdate?.Invoke(this, EventArgs.Empty);
        }

        void UpdateSummary(JObject config)
        {
            this.name = Lib.Utils.GetAliasFromConfig(config);
            this.summary = Lib.Utils.GetSummaryFromConfig(config);
        }

        void OnCoreStateChangedHandler(object sender, EventArgs args)
        {
            isServerOn = server.isRunning;
            InvokeEventOnPropertyChange();
        }

        #endregion
    }
}
