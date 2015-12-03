using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Pomodole
{
    public class TickTimer : ITickTimer
    {
        public event Action OnTick;

        private DispatcherTimer dispatcherTimer;
        public TickTimer(int updateFrequensyMS)
        {
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(updateFrequensyMS);
            dispatcherTimer.Tick += new EventHandler(Update);
        }

        public void Start()
        {
            lastUpdate = DateTime.Now;
            dispatcherTimer.Start();
        }

        public void Stop()
        {
            dispatcherTimer.Stop();
        }

        private DateTime lastUpdate;
        private long progress;
        void Update(object sender, EventArgs e)
        {
            progress += (long)(DateTime.Now - lastUpdate).TotalMilliseconds;
            lastUpdate = DateTime.Now;
            Console.WriteLine("Progress: {0}", progress);
            if (progress > 1000)
            {
                progress -= 1000;
                if(OnTick != null)
                {
                    OnTick();
                }
            }
        }
    }
}
