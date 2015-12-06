using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class MainWindowMessageManager : IMainWindowViewElements
    {
        public string Minute
        {
            get { return PomodoleHelper.ShapeTimeNumber(pomodoro.GetMinute()); }
        }
        public string Second
        {
            get { return PomodoleHelper.ShapeTimeNumber(pomodoro.GetSecond()); }
        }

        public string PomodoroSetMessage
        {
            get
            {
                // display pomodoro set time left
                if (pomodoro.GetRepeatTimeLeft() > 0)
                    return string.Format("{0} {1} {2}", MessageResource.GetMessageFor(Message.LeftPomodoroSetMessage),
                                                          pomodoro.GetRepeatTimeLeft().ToString(),
                                                          MessageResource.GetMessageFor(Message.RightPomodoroSetMessage));

                // return AlmostLongBreak message or LongBreakMessage when pomodoro set is 0
                switch(pomodoro.CurrentPhase)
                {
                    case PomodoroPhase.RunningTask:
                    case PomodoroPhase.WaitingSwitchToLongBreak:
                        return MessageResource.GetMessageFor(Message.AlmostLongBreakMessage);
                    case PomodoroPhase.RunningLongBreak:
                        return MessageResource.GetMessageFor(Message.LongBreakMessage);
                    default:
                        return "";
                }
            }
        }

        public string MainButtonMessage
        {
            get
            {
                switch(pomodoro.CurrentPhase)
                {
                    case PomodoroPhase.NotRunning:
                    default:
                        return MessageResource.GetMessageFor(Message.MainButtonStartmessage);
                    case PomodoroPhase.RunningTask:
                    case PomodoroPhase.RunningBreak:
                    case PomodoroPhase.RunningLongBreak:
                        return MessageResource.GetMessageFor(Message.MainButtonStopMessage);
                    case PomodoroPhase.WaitingSwitchToTask:
                    case PomodoroPhase.WaitingSwitchToBreak:
                    case PomodoroPhase.WaitingSwitchToLongBreak:
                        return MessageResource.GetMessageFor(Message.MainButtonResumeMessage);
                }
            }
        }

        private IPomodoro pomodoro;
        public MainWindowMessageManager(IPomodoro pomodoro)
        {
            this.pomodoro = pomodoro;
        }
    }
}
