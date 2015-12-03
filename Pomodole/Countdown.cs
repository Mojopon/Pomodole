using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class Countdown : ICountdownTimer
    {
        private int startMinute;
        private int currentMinute;
        private int startSecond;
        private int currentSecond;

        public bool CountdownEnd { get; private set; }

        public Countdown(int startMinute, int startSecond)
        {
            this.startMinute = startMinute;
            this.startSecond = startSecond;
            Reset();
        }

        public Countdown(int startMinute) : this(startMinute, 0) { }

        public void Tick()
        {
            ProgressSecond();
        }

        public void Reset()
        {
            currentMinute = startMinute;
            currentSecond = startSecond;
            CountdownEnd = false;
        }

        private void ProgressSecond()
        {
            currentSecond--;

            if (currentSecond <= 0)
            {
                ProgressMinute();
                if (CountdownEnd)
                {
                    return;
                }
                currentSecond = 60 + currentSecond;
            }
        }

        private bool ProgressMinute()
        {
            if (currentMinute <= 0)
            {
                CountdownEnd = true;
                return false;
            }
            currentMinute--;

            return true;
        }

        public int GetMinute()
        {
            return currentMinute;
        }

        public int GetSecond()
        {
            return currentSecond;
        }

        public int GetHour()
        {
            return 0;
        }
    }
}
