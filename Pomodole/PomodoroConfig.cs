using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class PomodoroConfig : IPomodoroConfig
    {
        public int TaskTime { get; private set; }
        public int BreakTime { get; private set; }
        public int RepeatTime { get; private set; }
        public int LongBreakTime { get; private set; }

        public PomodoroConfig(int task, int shortBreak, int repeatTime, int longBreak)
        {
            TaskTime = task;
            BreakTime = shortBreak;
            RepeatTime = repeatTime;
            LongBreakTime = longBreak;
        }
    }
}
