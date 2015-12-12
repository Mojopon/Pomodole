using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IPomodoroConfig
    {
        int TaskTime { get; set; }
        int BreakTime { get; set; }
        int RepeatTime { get; set; }
        int LongBreakTime { get; set; }
    }
}
