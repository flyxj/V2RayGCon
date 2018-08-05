using System;
using System.Windows.Forms;

namespace V2RayGCon.Model.BaseClass
{
    class CancelableTimeout
    {
        Timer timer;
        EventHandler OnTick;

        public CancelableTimeout(Action OnTick, int timeout)
        {
            if (timeout <= 0 || OnTick == null)
            {
                throw new ArgumentException();
            }

            timer = new Timer();
            timer.Interval = timeout;

            this.OnTick = (s, a) =>
            {
                OnTick();
                Cancel();
            };

            timer.Tick += this.OnTick;
        }

        public void Start()
        {
            if (timer.Enabled)
            {
                return;
            }

            timer.Start();
        }

        public void Cancel()
        {
            if (!timer.Enabled)
            {
                return;
            }

            timer.Stop();
        }

        public void Dispose()
        {
            Cancel();

            if (OnTick == null)
            {
                return;
            }

            timer.Tick -= OnTick;
            OnTick = null;
        }
    }
}
