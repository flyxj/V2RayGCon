using System;
using System.Collections.Generic;
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
            // do not bind events here 
            // list is empty
            // BindEvents();
        }

        #region private method

        void InvokeOnRequireMenuUpdate(object sender, EventArgs args)
        {
            OnRequireMenuUpdate?.Invoke(this, EventArgs.Empty);
        }

        void InvokeOnRequireFlyPanelUpdate(object sender, EventArgs args)
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
        public List<int> GetActiveServerList()
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
                try
                {
                    this[list[index]].RestartCoreThen(next);
                }
                catch
                {
                    next();
                }
            };

            ChainActionHelper(list.Count, worker, done)();
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

            ChainActionHelper(this.Count, worker, done)();
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

            ChainActionHelper(this.Count, worker, done)();
        }

        public void StopAllServersThen(Action lambda = null)
        {
            Action<int, Action> worker = (idx, next) =>
            {
                try
                {
                    this[idx].server.StopCoreThen(next);
                }
                catch
                {
                    // skip if something goes wrong
                    next();
                }
            };

            ChainActionHelper(this.Count, worker, lambda)();
        }

        /*
         * ChainActionHelper loop from count-1 to 0
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
         * Action finished = ()=>{
         *   // do something when all finished
         *   // or simply set to null
         * }
         * 
         * Finally call this function like this.
         * Do not forget the last pair of parentheses.
         * 
         * ChainActionHelper(10, worker, finished)();
         */
        Action ChainActionHelper(int countdown, Action<int, Action> worker, Action done)
        {
            int _index = countdown - 1;

            return () =>
            {
                if (_index < 0)
                {
                    done?.Invoke();
                    return;
                }

                worker(_index, ChainActionHelper(_index, worker, done));
            };
        }

        public void DeleteAllItemsThen(Action done = null)
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
                done?.Invoke();
                SaveChanges(this, EventArgs.Empty);
                InvokeOnRequireFlyPanelUpdate(this, EventArgs.Empty);
                InvokeOnRequireMenuUpdate(this, EventArgs.Empty);
            };

            ChainActionHelper(this.Count, worker, finish)();
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
                InvokeOnRequireFlyPanelUpdate(this, EventArgs.Empty);
                InvokeOnRequireMenuUpdate(this, EventArgs.Empty);
            };

            ChainActionHelper(this.Count, worker, done)();
        }

        public void BindEvents()
        {
            foreach (var server in this)
            {
                BindEvent(server);
            }
        }

        public void ReleaseEvents()
        {
            foreach (var server in this)
            {
                ReleaseEvent(server);
            }
        }

        private void DeleteItem(object sender, Model.Data.StrEvent args)
        {
            var index = GetItemIndexByConfig(args.Data);
            if (index < 0)
            {
                MessageBox.Show(I18N("CantFindOrgServDelFail"));
                return;
            }

            var item = this[index];

            lock (writeLock)
            {
                ReleaseEvent(this[index]);
                this.RemoveAt(index);
            }

            InvokeOnRequireMenuUpdate(this, EventArgs.Empty);
            InvokeOnRequireFlyPanelUpdate(this, EventArgs.Empty);
        }

        private int GetItemIndexByConfig(string config)
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

        public void BindEvent(Model.Data.ServerItem server)
        {
            server.OnLog += SendLog;
            server.OnPropertyChanged += SaveChanges;
            server.OnRequireMenuUpdate += InvokeOnRequireMenuUpdate;
            server.OnRequireDelete += DeleteItem;
        }

        public void ReleaseEvent(Model.Data.ServerItem server)
        {
            server.OnLog -= SendLog;
            server.OnPropertyChanged -= SaveChanges;
            server.OnRequireMenuUpdate -= InvokeOnRequireMenuUpdate;
            server.OnRequireDelete -= DeleteItem;
        }

        public bool AddConfig(string config, bool quiet = false)
        {
            foreach (var item in this)
            {
                if (item.config == config)
                {
                    // duplicate
                    return false;
                }
            }

            var server = new Model.Data.ServerItem()
            {
                config = config,
            };

            lock (writeLock)
            {
                BindEvent(server);
                this.Add(server);
            }

            if (!quiet)
            {
                server.UpdateSummaryThen(() =>
                {
                    InvokeOnRequireMenuUpdate(this, EventArgs.Empty);
                    InvokeOnRequireFlyPanelUpdate(this, EventArgs.Empty);
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
