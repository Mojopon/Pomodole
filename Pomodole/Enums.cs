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
        ConfigWindow,
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
        Completed,
    }

    public enum Message
    {
        MainButtonStartmessage,
        MainButtonStopMessage,
        MainButtonResumeMessage,
        DisplayPomodoroSetMessage,
        AlmostLongBreakMessage,
        LongBreakMessage,
        StartTaskMessage,
        StartBreakMessage,
        StartLongBreakMessage,
    }

    public enum ServiceType
    {
        Production,
        Test,
    }

    public enum ViewFor
    {
        MainWindow,
        ConfigWindow,
    }
}
