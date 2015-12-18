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
            pomodoroConfig = new PomodoroConfig(25, 5, 3, 15);
        }

        public void ExecuteConfigurationFor(IPomodoroConfigUser target)
        {
            target.ConfigurePomodoroRelatives(pomodoroConfig);
        }

        public void SetupPomodoroConfig(int taskTime, int breakTime, int repeatTime, int longBreakTime)
        {
            pomodoroConfig = new PomodoroConfig(taskTime, breakTime, repeatTime, longBreakTime);
        }

        public void Save()
        {
            var configurationFileManager = ConfigurationFileManager.GetInstance();
            var configurations = new Configurations();
            configurations.pomodoroConfig = (PomodoroConfig)pomodoroConfig;

            configurationFileManager.Save(configurations, App.ConfigurationFileName);
        }

        public void Load()
        {
            var configurationFileManager = ConfigurationFileManager.GetInstance();
            var configurations = configurationFileManager.Load<Configurations>(App.ConfigurationFileName);
            pomodoroConfig = configurations.pomodoroConfig;
        }

        public class Configurations
        {
            public PomodoroConfig pomodoroConfig;
        }
    }
}
