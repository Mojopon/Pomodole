using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IConfigManager : IPomodoroConfig, IApplicationMessageSubscriber
    {
        void ExecuteConfigurationFor(IPomodoroConfigUser target);
        void SaveConfigurationToFile();
        void LoadConfigurationFromFile();
    }
}
