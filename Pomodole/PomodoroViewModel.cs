using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Threading;

namespace Pomodole
{
    public class PomodoroViewModel : INotifyPropertyChanged
    {
        public string Minute
        {
            get { return ShapeTimeNumber(pomodoro.GetMinute()); }
        }

        public string Second
        {
            get { return ShapeTimeNumber(pomodoro.GetSecond()); }
        }

        private IPomodoro pomodoro;
        private ITickTimer tickTimer;
        public PomodoroViewModel(IPomodoro pomodoro)
        {
            this.pomodoro = pomodoro;
            tickTimer = new TickTimer(50);
            tickTimer.OnTick += new Action(OnProgressTime);
        }

        public void OnProgressTime()
        {
            pomodoro.Tick();
            NotifyPropertyChanged("Minute");
            NotifyPropertyChanged("Second");
        }

        private string ShapeTimeNumber(int time)
        {
            if (time < 10)
            {
                return "0" + time.ToString();
            }

            return time.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
