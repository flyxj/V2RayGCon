using System;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.Data
{
    public class ServerList : EventList<ServerItem>
    {
        public event EventHandler<Model.Data.StrEvent> OnLog;
        public event EventHandler RequireMenuUpdate;

        private object writeLock = new object();

        public ServerList()
        {
            // do not bind events here 
            // list is empty
            // BindEvents();
        }

        #region private method

        void InvokeRequireMenuUpdate(object sender, EventArgs args)
        {
            try
            {
                RequireMenuUpdate?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        private void SendLog(object sender, Model.Data.StrEvent arg)
        {
            try
            {
                OnLog?.Invoke(this, arg);
            }
            catch { }
        }

        private void SaveChange(object sender, EventArgs arg)
        {
            Notify();
        }
        #endregion

        #region public method
        public void StopAllCoreThen(Action lambda = null)
        {
            Action helper(int cur, int max)
            {
                int _cur = cur;
                int _max = max;

                return () =>
                {
                    if (_cur < _max)
                    {
                        this[cur].server.StopCoreThen(helper(cur + 1, max));
                        return;
                    }

                    lambda?.Invoke();
                };
            }

            helper(0, this.Count)();
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
            item.CleanupThen(() =>
            {
                lock (writeLock)
                {
                    ReleaseEvent(this[index]);
                    this.RemoveAt(index);
                }

                InvokeRequireMenuUpdate(this, EventArgs.Empty);
            });
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
            server.OnPropertyChanged += SaveChange;
            server.RequireMenuUpdate += InvokeRequireMenuUpdate;
            server.RequireDelete += DeleteItem;
        }

        public void ReleaseEvent(Model.Data.ServerItem server)
        {
            server.OnLog -= SendLog;
            server.OnPropertyChanged -= SaveChange;
            server.RequireMenuUpdate -= InvokeRequireMenuUpdate;
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

            lock (writeLock)
            {
                var server = new Model.Data.ServerItem()
                {
                    config = config,
                };

                BindEvent(server);
                this.AddQuiet(server);
            }

            if (!quiet)
            {
                Notify();
            }

            return true;
        }

        public int SearchConfig(string config)
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

        public bool Replace(int index, string config)
        {
            if (index < 0 || index > this.Count)
            {
                return false;
            }

            this[index].ChangeConfig(config);
            this.Notify();
            return true;
        }

        #endregion
    }
}
