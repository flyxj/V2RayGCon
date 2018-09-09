using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.Data
{
    public class ServerList : EventList<ServerItem>
    {
        public event EventHandler<Model.Data.StrEvent> OnLog;
        public event EventHandler OnRequireMenuUpdate, OnRequireFlyPanelUpdate;

        [JsonIgnore]
        object writeLock = new object();

        [JsonIgnore]
        object testingLock = new object();

        public ServerList()
        {
            // Do not bind events here, because list is empty.
            // BindEvents();
            isTesting = false;
        }

        #region property
        [JsonIgnore]
        bool _isTesting;

        [JsonIgnore]
        bool isTesting
        {
            get
            {
                lock (testingLock)
                {
                    return _isTesting;
                }
            }
            set
            {
                lock (testingLock)
                {
                    _isTesting = value;
                }
            }
        }
        #endregion

        #region private method

        void InvokeEventOnRequireMenuUpdate(object sender, EventArgs args)
        {
            OnRequireMenuUpdate?.Invoke(this, EventArgs.Empty);
        }

        void InvokeEventOnRequireFlyPanelUpdate(object sender, EventArgs args)
        {
            OnRequireFlyPanelUpdate?.Invoke(this, EventArgs.Empty);
        }

        private void OnSendLogHandler(object sender, Model.Data.StrEvent arg)
        {
            OnLog?.Invoke(this, arg);
        }

        private void SaveChanges(object sender, EventArgs arg)
        {
            Notify();
        }

        JObject Config2Package(string configString, string id, int portBase, int index, string tagPrefix)
        {
            var cache = Service.Cache.Instance;
            var pkg = cache.tpl.LoadPackage("package");
            var config = Lib.ImportParser.Parse(configString);

            var tagin = tagPrefix + "in" + index.ToString();
            var tagout = tagPrefix + "out" + index.ToString();
            var port = portBase + index;

            pkg["routing"]["settings"]["rules"][0]["inboundTag"][0] = tagin;
            pkg["routing"]["settings"]["rules"][0]["outboundTag"] = tagout;

            pkg["inboundDetour"][0]["port"] = port;
            pkg["inboundDetour"][0]["tag"] = tagin;
            pkg["inboundDetour"][0]["settings"]["port"] = port;
            pkg["inboundDetour"][0]["settings"]["clients"][0]["id"] = id;

            pkg["outboundDetour"][0]["protocol"] = config["outbound"]["protocol"];
            pkg["outboundDetour"][0]["tag"] = tagout;
            pkg["outboundDetour"][0]["settings"] = config["outbound"]["settings"];
            pkg["outboundDetour"][0]["streamSettings"] = config["outbound"]["streamSettings"];

            return pkg;
        }

        JObject CreateVnext(int index, int port, string id)
        {
            var vnext = Service.Cache.Instance.tpl.LoadPackage("vnext");
            vnext["outbound"]["settings"]["vnext"][0]["port"] = port + index;
            vnext["outbound"]["settings"]["vnext"][0]["users"][0]["id"] = id;
            return vnext;
        }
        #endregion

        #region public method
        public void PackSelectedServers()
        {
            var cache = Service.Cache.Instance;
            var list = this.Where(s => s.isSelected).OrderByDescending(s => s.index).ToList();
            var container = JObject.Parse(@"{}");
            var id = Guid.NewGuid().ToString();
            var port = Lib.Utils.Str2Int(StrConst("PacmanInitPort"));
            var tagPrefix = StrConst("PacmanTagPrefix");

            Action done = () =>
            {
                var config = cache.tpl.LoadPackage("main");
                Lib.Utils.UnionJson(ref config, container);
                OnSendLogHandler(this, new Model.Data.StrEvent(I18N("PackageDone")));
                AddServer(config.ToString(Formatting.None));
            };

            Action<int, Action> worker = (index, next) =>
            {
                var server = list[index];
                try
                {
                    var pack = Config2Package(server.config, id, port, index, tagPrefix);
                    Lib.Utils.UnionJson(ref container, pack);
                    var vnext = CreateVnext(index, port, id);
                    Lib.Utils.UnionJson(ref container, vnext);
                    OnSendLogHandler(this, new Model.Data.StrEvent(I18N("PackageSuccess") + ": " + server.name));
                }
                catch
                {
                    OnSendLogHandler(this, new Model.Data.StrEvent(I18N("PackageFail") + ": " + server.name));
                }
                next();
            };

            Lib.Utils.ChainActionHelperAsync(list.Count, worker, done);
        }

        public bool DoSpeedTestOnSelectedServers()
        {
            if (isTesting)
            {
                return false;
            }
            isTesting = true;

            var list = this.OrderBy(o => o.index)
                .Where(o => o.isSelected)
                .ToList();

            Action done = () =>
            {
                this.isTesting = false;
                OnSendLogHandler(this, new StrEvent(I18N("SpeedTestFinished")));
            };

            var count = list.Count;

            Action<int, Action> worker = (index, next) =>
            {
                // idx = 0 to count - 1
                var idx = count - 1 - index;
                list[idx].DoSpeedTestThen(next);

            };

            Lib.Utils.ChainActionHelperAsync(count, worker, done);
            return true;
        }

        public List<Model.Data.ServerItem> GetActiveServersList()
        {
            return this.Where(s => s.isServerOn).ToList();
        }

        public void RetartServersByList(List<Model.Data.ServerItem> servers, Action done = null)
        {
            var list = servers;
            Action<int, Action> worker = (index, next) =>
            {
                list[index].RestartCoreThen(next);
            };

            Lib.Utils.ChainActionHelperAsync(list.Count, worker, done);
        }

        public void WakeupAutorunServersThen(Action done = null)
        {
            Action<int, Action> worker = (index, next) =>
            {
                if (this[index].isAutoRun)
                {
                    this[index].RestartCoreThen(next);
                }
                else
                {
                    next();
                }
            };

            Lib.Utils.ChainActionHelperAsync(this.Count, worker, done);
        }

        public void RestartAllSelectedThen(Action done = null)
        {
            Action<int, Action> worker = (index, next) =>
            {
                if (this[index].isSelected)
                {
                    this[index].RestartCoreThen(next);
                }
                else
                {
                    next();
                }
            };

            Lib.Utils.ChainActionHelperAsync(this.Count, worker, done);
        }

        public void RestartAllServersThen(Action done = null)
        {
            Action<int, Action> worker = (index, next) =>
            {
                if (this[index].isServerOn)
                {
                    this[index].RestartCoreThen(next);
                }
                else
                {
                    next();
                }
            };

            Lib.Utils.ChainActionHelperAsync(this.Count, worker, done);
        }

        public void StopAllSelectedThen(Action lambda = null)
        {
            Action<int, Action> worker = (index, next) =>
            {
                if (this[index].isSelected)
                {
                    this[index].server.StopCoreThen(next);
                }
                else
                {
                    next();
                }
            };

            Lib.Utils.ChainActionHelperAsync(this.Count, worker, lambda);
        }

        public void StopAllServersThen(Action lambda = null)
        {
            Action<int, Action> worker = (index, next) =>
            {
                // Model.BaseClass.CoreServer take care of errors 
                // do not need try/catch
                this[index].server.StopCoreThen(next);
            };

            Lib.Utils.ChainActionHelperAsync(this.Count, worker, lambda);
        }

        public void DeleteSelectedServersThen(Action done = null)
        {
            if (isTesting)
            {
                MessageBox.Show(I18N("LastTestNoFinishYet"));
                return;
            }

            Action<int, Action> worker = (index, next) =>
            {
                if (!this[index].isSelected)
                {
                    next();
                    return;
                }

                this[index].CleanupThen(() =>
                {
                    this.RemoveAt(index);
                    next();
                });
            };

            Action finish = () =>
            {
                SaveChanges(this, EventArgs.Empty);
                InvokeEventOnRequireFlyPanelUpdate(this, EventArgs.Empty);
                InvokeEventOnRequireMenuUpdate(this, EventArgs.Empty);
                done?.Invoke();
            };

            Lib.Utils.ChainActionHelperAsync(this.Count, worker, finish);
        }



        public void DeleteAllServersThen(Action done = null)
        {
            if (isTesting)
            {
                MessageBox.Show(I18N("LastTestNoFinishYet"));
                return;
            }

            Action<int, Action> worker = (index, next) =>
            {
                this[index].CleanupThen(() =>
                {
                    this.RemoveAt(index);
                    next();
                });
            };

            Action finish = () =>
            {
                SaveChanges(this, EventArgs.Empty);
                InvokeEventOnRequireFlyPanelUpdate(this, EventArgs.Empty);
                InvokeEventOnRequireMenuUpdate(this, EventArgs.Empty);
                done?.Invoke();
            };

            Lib.Utils.ChainActionHelperAsync(this.Count, worker, finish);
        }

        public void UpdateAllServersSummary()
        {
            Action<int, Action> worker = (index, next) =>
            {
                try
                {
                    this[index].UpdateSummaryThen(next);
                }
                catch
                {
                    // skip if something goes wrong
                    next();
                }
            };

            Action done = () =>
            {
                InvokeEventOnRequireFlyPanelUpdate(this, EventArgs.Empty);
                InvokeEventOnRequireMenuUpdate(this, EventArgs.Empty);
            };

            Lib.Utils.ChainActionHelperAsync(this.Count, worker, done);
        }

        public void BindEventsToAllServers()
        {
            foreach (var server in this)
            {
                BindEventTo(server);
            }
        }

        public void ReleaseEventsFromAllServers()
        {
            foreach (var server in this)
            {
                ReleaseEventFrom(server);
            }
        }

        private void DeleteServerHandler(object sender, Model.Data.StrEvent args)
        {
            if (isTesting)
            {
                MessageBox.Show(I18N("LastTestNoFinishYet"));
                return;
            }

            var index = GetServerIndexByConfig(args.Data);
            if (index < 0)
            {
                MessageBox.Show(I18N("CantFindOrgServDelFail"));
                return;
            }

            var server = this[index];

            Action removeServer = () =>
            {
                lock (writeLock)
                {
                    ReleaseEventFrom(server);
                    this.RemoveAt(index);
                }
                InvokeEventOnRequireMenuUpdate(this, EventArgs.Empty);
                InvokeEventOnRequireFlyPanelUpdate(this, EventArgs.Empty);
            };

            server.CleanupThen(removeServer);
        }

        public void BindEventTo(Model.Data.ServerItem server)
        {
            server.OnLog += OnSendLogHandler;
            server.OnPropertyChanged += SaveChanges;
            server.OnRequireMenuUpdate += InvokeEventOnRequireMenuUpdate;
            server.OnRequireDeleteServer += DeleteServerHandler;
        }

        public void ReleaseEventFrom(Model.Data.ServerItem server)
        {
            server.OnLog -= OnSendLogHandler;
            server.OnPropertyChanged -= SaveChanges;
            server.OnRequireMenuUpdate -= InvokeEventOnRequireMenuUpdate;
            server.OnRequireDeleteServer -= DeleteServerHandler;
        }

        public bool AddServer(string config, bool quiet = false)
        {
            var newServer = new Model.Data.ServerItem()
            {
                config = config,
            };

            lock (writeLock)
            {
                foreach (var server in this)
                {
                    if (server.config == config)
                    {
                        // duplicate
                        return false;
                    }
                }

                this.Add(newServer);
            }

            BindEventTo(newServer);

            if (!quiet)
            {
                newServer.UpdateSummaryThen(() =>
                {
                    InvokeEventOnRequireMenuUpdate(this, EventArgs.Empty);
                    InvokeEventOnRequireFlyPanelUpdate(this, EventArgs.Empty);
                });
            }

            return true;
        }

        public int GetServerIndexByConfig(string config)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].config == config)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool ReplaceServerConfigByIndex(int index, string config)
        {
            if (index < 0 || index > this.Count)
            {
                return false;
            }

            this[index].ChangeConfig(config);
            return true;
        }

        #endregion
    }
}
