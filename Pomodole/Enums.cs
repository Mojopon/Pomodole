using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public enum ViewModelFor
    {
        MainWindow,
    }

    public enum Language
    {
        Japanese,
        English,
    }

    public enum PomodoroPhase
    {
        NotRunning,
        RunningTask,
        WaitingSwitchToBreak,
        RunningBreak,
        WaitingSwitchToTask,
        WaitingSwitchToLongBreak,
        RunningLongBreak,
    }

    public enum Message
    {
        MainButtonStartmessage,
        MainButtonStopMessage,
        MainButtonResumeMessage,
        LeftPomodoroSetMessage,
        RightPomodoroSetMessage,
        AlmostLongBreakMessage,
        LongBreakMessage,
    }
}
