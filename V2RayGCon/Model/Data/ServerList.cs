using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.Data
{
    public class ServerList : EventList<ServerItem>
    {
        public event EventHandler<Model.Data.StrEvent> OnLog;
        public event EventHandler OnRequireMenuUpdate;
        public event EventHandler OnRequireFlyPanelUpdate;

        private object writeLock = new object();

        public ServerList()
        {
            // Do not bind events here, because list is empty.
            // BindEvents();
        }

        #region private method

        void InvokeEventOnRequireMenuUpdate(object sender, EventArgs args)
        {
            OnRequireMenuUpdate?.Invoke(this, EventArgs.Empty);
        }

        void InvokeEventOnRequireFlyPanelUpdate(object sender, EventArgs args)
        {
            OnRequireFlyPanelUpdate?.Invoke(this, EventArgs.Empty);
        }

        private void SendLog(object sender, Model.Data.StrEvent arg)
        {
            OnLog?.Invoke(this, arg);
        }

        private void SaveChanges(object sender, EventArgs arg)
        {
            Notify();
        }
        #endregion

        #region public method
        public List<int> GetActiveServersList()
        {
            var list = new List<int>();
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].isServerOn)
                {
                    list.Add(i);
                }
            }
            return list;
        }

        public void StartServersByList(List<int> servers, Action done = null)
        {
            var list = servers;
            Action<int, Action> worker = (index, next) =>
            {
                this[list[index]].RestartCoreThen(next);
            };

            ChainActionHelper(list.Count, worker, done);
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

            ChainActionHelper(this.Count, worker, done);
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


            ChainActionHelper(this.Count, worker, done);
        }

        public void StopAllServersThen(Action lambda = null)
        {
            Action<int, Action> worker = (index, next) =>
            {
                // Model.BaseClass.CoreServer take care of errors 
                // do not need try/catch
                this[index].server.StopCoreThen(next);
            };

            ChainActionHelper(this.Count, worker, lambda);
        }

        /*
         * ChainActionWorker loop from count-1 to 0
         * These values will pass into worker through the first parameter,
         * which is index in this example.
         * 
         * Action<int,Action> worker = (index, next)=>{
         * 
         *   // do something accroding to index
         *   console.log(index); 
         *   
         *   // call next when done
         *   next(); 
         * }
         * 
         * Action done = ()=>{
         *   // do something when all done
         *   // or simply set to null
         * }
         * 
         * Finally call this function like this.
         * ChainActionWorker(10, worker, done)();
         * 
         */

        void ChainActionHelper(int countdown, Action<int, Action> worker, Action done = null)
        {
            Task.Factory.StartNew(() =>
            {
                ChainActionWorker(countdown, worker, done)();
            });
        }

        Action ChainActionWorker(int countdown, Action<int, Action> worker, Action done = null)
        {
            int _index = countdown - 1;

            return () =>
            {
                if (_index < 0)
                {
                    done?.Invoke();
                    return;
                }

                worker(_index, ChainActionWorker(_index, worker, done));
            };
        }

        public void DeleteAllServersThen(Action done = null)
        {
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

            ChainActionHelper(this.Count, worker, finish);
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

            ChainActionHelper(this.Count, worker, done);
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
            var index = GetServerIndexByConfig(args.Data);
            if (index < 0)
            {
                MessageBox.Show(I18N("CantFindOrgServDelFail"));
                return;
            }

            lock (writeLock)
            {
                ReleaseEventFrom(this[index]);
                this.RemoveAt(index);
            }

            InvokeEventOnRequireMenuUpdate(this, EventArgs.Empty);
            InvokeEventOnRequireFlyPanelUpdate(this, EventArgs.Empty);
        }

        public void BindEventTo(Model.Data.ServerItem server)
        {
            server.OnLog += SendLog;
            server.OnPropertyChanged += SaveChanges;
            server.OnRequireMenuUpdate += InvokeEventOnRequireMenuUpdate;
            server.OnRequireDeleteServer += DeleteServerHandler;
        }

        public void ReleaseEventFrom(Model.Data.ServerItem server)
        {
            server.OnLog -= SendLog;
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
