using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class Pomodoro : IPomodoro
    {
        public bool CountdownEnd { get; private set; }

        public event Action OnSwitchToBreak;
        public event Action OnSwitchToTask;

        private Countdown taskCountdown;
        private Countdown breakCountdown;
        private int repeatTime;
        private Countdown longBreakCountdown;

        public Pomodoro() { }

        public void Configure(IPomodoroConfig config)
        {
            taskCountdown = new Countdown(config.TaskTime);
            breakCountdown = new Countdown(config.BreakTime);
            repeatTime = config.RepeatTime;
            longBreakCountdown = new Countdown(config.LongBreakTime);

            Reset();
        }

        private Countdown currentCountdown;
        private int repeatTimeLeft;
        public void Reset()
        {
            taskCountdown.Reset();
            breakCountdown.Reset();
            repeatTimeLeft = repeatTime;

            currentCountdown = taskCountdown;
        }

        public void Tick()
        {
            currentCountdown.Tick();
            if (currentCountdown.CountdownEnd)
            {
                if (currentCountdown == taskCountdown)
                {
                    SwitchTaskCountdownToBreakCountdown();
                }
                else if (currentCountdown == breakCountdown)
                {
                    SwitchBreakCountdownToTaskCountdown();
                }
            }
        }

        void SwitchTaskCountdownToBreakCountdown()
        {
            if (OnSwitchToBreak != null) OnSwitchToBreak();

            taskCountdown.Reset();
            currentCountdown = breakCountdown;
        }

        void SwitchBreakCountdownToTaskCountdown()
        {
            if (OnSwitchToTask != null) OnSwitchToTask();

            repeatTimeLeft--;
            breakCountdown.Reset();

            currentCountdown = taskCountdown;
        }

        public int GetMinute()
        {
            return currentCountdown.GetMinute();
        }

        public int GetSecond()
        {
            return currentCountdown.GetSecond();
        }
    }
}
