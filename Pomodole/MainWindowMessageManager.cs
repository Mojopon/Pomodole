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
            get { return string.Format("{0} {1} {2}", MessageResource.GetMessageFor(Message.LeftPomodoroSetMessage),
                                                      pomodoro.GetRepeatTimeLeft().ToString(),
                                                      MessageResource.GetMessageFor(Message.RightPomodoroSetMessage)); }
        }

        public string StartButtonMessage
        {
            get { return MessageResource.GetMessageFor(Message.StartButton); }
        }

        private IPomodoro pomodoro;
        public MainWindowMessageManager(IPomodoro pomodoro)
        {
            this.pomodoro = pomodoro;
        }
    }
}
