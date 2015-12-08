using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ConfigManager : IConfigManager
    {
        public ConfigManager() { }

        public void ExecuteConfigurationFor(IPomodoroConfigUser target)
        {
            target.ConfigurePomodoroRelatives(pomodoroConfig);
        }

        private IPomodoroConfig pomodoroConfig;
        public void SetupPomodoroConfig(int taskTime, int breakTime, int repeatTime, int longBreakTime)
        {
            pomodoroConfig = new PomodoroConfig(taskTime, breakTime, repeatTime, longBreakTime);
        }
    }
}
