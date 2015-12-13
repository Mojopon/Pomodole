using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ConfigManager : IConfigManager
    {
        public int TaskTime {
            get { return pomodoroConfig.TaskTime; }
            set { pomodoroConfig.TaskTime = value; }
        }
        public int BreakTime
        {
            get { return pomodoroConfig.BreakTime; }
            set { pomodoroConfig.BreakTime = value; }
        }
        public int RepeatTime
        {
            get { return pomodoroConfig.RepeatTime; }
            set { pomodoroConfig.RepeatTime = value; }
        }
        public int LongBreakTime
        {
            get { return pomodoroConfig.LongBreakTime; }
            set { pomodoroConfig.LongBreakTime = value; }
        }


        private IPomodoroConfig pomodoroConfig;
        public ConfigManager()
        {
            pomodoroConfig = new PomodoroConfig(20, 3, 2, 10);
        }

        public void ExecuteConfigurationFor(IPomodoroConfigUser target)
        {
            target.ConfigurePomodoroRelatives(pomodoroConfig);
        }

        public void SetupPomodoroConfig(int taskTime, int breakTime, int repeatTime, int longBreakTime)
        {
            pomodoroConfig = new PomodoroConfig(taskTime, breakTime, repeatTime, longBreakTime);
        }
    }
}
