using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IPomodoro : ICountdownTimer, IConfigurable, IPomodoroConfigUser
    {
        event Action OnSwitchToBreak;
        event Action OnSwitchToTask;
        event Action OnSwitchToLongBreak;
        event Action OnCompletePomodoro;

        int GetRepeatTimeLeft();
        PomodoroPhase CurrentPhase { get; }
    }
}
