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
        public double Progress { get { return currentCountdown.Progress; } }

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
            taskCountdown = new Countdown(0,4);
            breakCountdown = new Countdown(0,3);
            repeatTime = 2;
            longBreakCountdown = new Countdown(0,5);

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
            CurrentPhase = PomodoroPhase.NotRunning;
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
            else
            {
                if (currentCountdown == taskCountdown)
                {
                    CurrentPhase = PomodoroPhase.RunningTask;
                }
                else if (currentCountdown == breakCountdown)
                {
                    CurrentPhase = PomodoroPhase.RunningBreak;
                }
                else if (currentCountdown == longBreakCountdown)
                {
                    CurrentPhase = PomodoroPhase.RunningLongBreak;
                }
            }
        }

        void SwitchTaskCountdownToBreakCountdown()
        {
            if (repeatTimeLeft <= 0)
            {
                CurrentPhase = PomodoroPhase.WaitingSwitchToLongBreak;
                if (OnSwitchToLongBreak != null) OnSwitchToLongBreak();
                currentCountdown = longBreakCountdown;
            }
            else
            {
                CurrentPhase = PomodoroPhase.WaitingSwitchToBreak;
                if (OnSwitchToBreak != null) OnSwitchToBreak();
                currentCountdown = breakCountdown;
            }
            taskCountdown.Reset();
        }

        void SwitchBreakCountdownToTaskCountdown()
        {
            repeatTimeLeft--;
            CurrentPhase = PomodoroPhase.WaitingSwitchToTask;
            if (OnSwitchToTask != null) OnSwitchToTask();
            currentCountdown = taskCountdown;

            breakCountdown.Reset();
        }

        void CompletePomodoro()
        {
            CurrentPhase = PomodoroPhase.NotRunning;
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
