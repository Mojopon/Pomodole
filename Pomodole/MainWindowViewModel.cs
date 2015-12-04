using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows;

namespace Pomodole
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand StartCommand { get; private set; }

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
        public bool TimerRunning { get; private set; }
        public MainWindowViewModel(IPomodoro pomodoro)
        {
            this.pomodoro = pomodoro;
            tickTimer = new TickTimer(50);
            tickTimer.OnTick += new Action(OnTick);
            StartCommand = new StartCommandImpl(this);
        }

        public void Start()
        {
            tickTimer.Start();
            TimerRunning = true;
        }

        public void Stop()
        {
            tickTimer.Stop();
            TimerRunning = false;
        }

        public void OnTick()
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

        class StartCommandImpl : ICommand
        {
            private MainWindowViewModel viewModel;
            public StartCommandImpl(MainWindowViewModel viewModel)
            {
                this.viewModel = viewModel;
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return !viewModel.TimerRunning;
            }

            public void Execute(object parameter)
            {
                viewModel.Start();
            }
        }
    }
}
