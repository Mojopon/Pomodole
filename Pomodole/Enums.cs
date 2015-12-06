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
        Task,
        Break,
        LongBreak,
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
