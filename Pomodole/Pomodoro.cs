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

        public PomodoroPhase CurrentPhase { get; private set; }

        public event Action OnSwitchToBreak;
        public event Action OnSwitchToTask;
        public event Action OnSwitchToLongBreak;
        public event Action OnCompletePomodoro;

        private Countdown taskCountdown;
        private Countdown breakCountdown;
        private int repeatTime;
        private Countdown longBreakCountdown;

        public Pomodoro() { }

        public Pomodoro(bool flag)
        {
            taskCountdown = new Countdown(0,2);
            breakCountdown = new Countdown(0,1);
            repeatTime = 2;
            longBreakCountdown = new Countdown(0,3);

            Reset();
        }

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
            CurrentPhase = PomodoroPhase.Task;
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
                else if(currentCountdown == longBreakCountdown)
                {
                    CompletePomodoro();
                }
            }
        }

        void SwitchTaskCountdownToBreakCountdown()
        {
            if (repeatTimeLeft <= 0)
            {
                if (OnSwitchToLongBreak != null) OnSwitchToLongBreak();
                currentCountdown = longBreakCountdown;
                CurrentPhase = PomodoroPhase.LongBreak;
            }
            else
            {
                if (OnSwitchToBreak != null) OnSwitchToBreak();
                currentCountdown = breakCountdown;
                CurrentPhase = PomodoroPhase.Break;
            }
            taskCountdown.Reset();
        }

        void SwitchBreakCountdownToTaskCountdown()
        {
            repeatTimeLeft--;
            if (OnSwitchToTask != null) OnSwitchToTask();
            currentCountdown = taskCountdown;
            CurrentPhase = PomodoroPhase.Task;

            breakCountdown.Reset();
        }

        void CompletePomodoro()
        {
            if (OnCompletePomodoro != null) OnCompletePomodoro();
            Reset();
        }

        public int GetMinute()
        {
            return currentCountdown.GetMinute();
        }

        public int GetSecond()
        {
            return currentCountdown.GetSecond();
        }

        public int GetRepeatTimeLeft()
        {
            return repeatTimeLeft;
        }
    }
}
