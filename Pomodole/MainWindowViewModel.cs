using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shell;

namespace Pomodole
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        public ICommand StartCommand { get; private set; }

        public string Minute { get { return MessageManager.Minute; } }
        public string Second { get { return MessageManager.Second; } }
        public string PomodoroSetMessage { get { return MessageManager.PomodoroSetMessage; } }
        public string MainButtonMessage { get { return MessageManager.MainButtonMessage; } }

        public double Progress { get { return pomodoro.Progress; } }
        public TaskbarItemProgressState ProgressState { get; private set; }

        public MainWindowMessageManager MessageManager { get; private set; }
        private IPomodoro pomodoro;
        private ITickTimer tickTimer;
        public bool TimerRunning { get; private set; }

        public MainWindowViewModel(IPomodoro pomodoro)
        {
            this.pomodoro = pomodoro;
            pomodoro.OnSwitchToBreak += new Action(OnSwitchToBreakEvent);
            pomodoro.OnSwitchToTask += new Action(OnSwitchToTaskEvent);
            pomodoro.OnSwitchToLongBreak += new Action(OnSwitchToLongBreakEvent);
            pomodoro.OnCompletePomodoro += new Action(OnCompletePomodoroEvent);

            MessageManager = new MainWindowMessageManager(pomodoro);

            tickTimer = new TickTimer(50);
            tickTimer.OnTick += new Action(OnTick);
            StartCommand = new StartCommandImpl(this);
        }

        public void Start()
        {
            tickTimer.Start();
            TimerRunning = true;
            ProgressState = TaskbarItemProgressState.Normal;
        }

        public void Stop()
        {
            tickTimer.Stop();
            TimerRunning = false;
            ProgressState = TaskbarItemProgressState.Paused;
        }

        void OnSwitchToTaskEvent()
        {
            Stop();
            UpdatePropeties();
            MessageBox.Show("Switch to task");
            Start();
        }

        void OnSwitchToBreakEvent()
        {
            Stop();
            UpdatePropeties();
            MessageBox.Show("Switch to break");
            Start();
        }

        void OnSwitchToLongBreakEvent()
        {
            Stop();
            UpdatePropeties();
            MessageBox.Show("Switch to Long break");
            NotifyPropertyChanged("PomodoroSet");
            Start();
        }

        void OnCompletePomodoroEvent()
        {
            Stop();
            UpdatePropeties();
            MessageBox.Show("Pomodoro Completed");
            ProgressState = TaskbarItemProgressState.None;
        }

        public void OnTick()
        {
            pomodoro.Tick();
            UpdatePropeties();
        }

        private void UpdatePropeties()
        {
            NotifyPropertyChanged("Minute");
            NotifyPropertyChanged("Second");
            NotifyPropertyChanged("MainButtonMessage");
            NotifyPropertyChanged("PomodoroSetMessage");
            NotifyPropertyChanged("Progress");
            NotifyPropertyChanged("ProgressState");
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
