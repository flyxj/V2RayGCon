using System;

namespace V2RayGCon.Model.Data
{
    public class ServerList : EventList<ServerItem>
    {
        public event EventHandler<Model.Data.StrEvent> OnLog;

        public ServerList()
        {
            // do not bindevents in here 
            // list is empty
            // BindEvents();
        }

        #region private method
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

        public void BindEvent(Model.Data.ServerItem server)
        {
            server.OnLog += SendLog;
            server.OnPropertyChanged += SaveChange;
        }

        public void ReleaseEvent(Model.Data.ServerItem server)
        {
            server.OnLog -= SendLog;
            server.OnPropertyChanged -= SaveChange;
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

            BindEvent(server);

            if (quiet)
            {
                this.AddQuiet(server);
            }
            else
            {
                this.Add(server);
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
