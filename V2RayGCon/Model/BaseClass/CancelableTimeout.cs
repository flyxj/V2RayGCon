using System;
using System.Timers;

namespace V2RayGCon.Model.BaseClass
{
    class CancelableTimeout
    {
        Timer timer;
        int TIMEOUT;
        Action worker;
        object timerLock;

        public CancelableTimeout(Action worker, int timeout)
        {
            if (timeout <= 0 || worker == null)
            {
                throw new ArgumentException();
            }

            this.TIMEOUT = timeout;
            this.worker = worker;
            this.timerLock = new object();

            timer = new Timer();
            timer.Enabled = false;
            timer.AutoReset = false;
            timer.Elapsed += OnTimeout;
        }

        private void OnTimeout(object sender, EventArgs e)
        {
            lock (timerLock)
            {
                this.worker();
            }
        }

        public void Timeout()
        {
            Cancel();
            this.worker();
        }

        public void Start()
        {
            lock (timerLock)
            {
                timer.Interval = this.TIMEOUT;
                timer.Enabled = true;
            }
        }

        public void Cancel()
        {
            lock (timerLock)
            {
                timer.Enabled = false;
            }
        }

        public void Release()
        {
            Cancel();
            timer.Elapsed -= OnTimeout;
            this.worker = null;
            timer.Close();
        }
    }
}
