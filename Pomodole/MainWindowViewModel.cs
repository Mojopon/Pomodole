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
            
            tickTimer = new TickTimer(50);
            tickTimer.OnTick += new Action(OnTick);
            StartCommand = new StartCommandImpl(this);
        }

        public void Start()
        {
            tickTimer.Start();
            TimerRunning = true;
            ProgressState = TaskbarItemProgressState.Normal;
            UpdatePropeties();
        }

        public void Stop()
        {
            tickTimer.Stop();
            TimerRunning = false;
            ProgressState = TaskbarItemProgressState.Paused;
            UpdatePropeties();
        }

        void OnSwitchToTaskEvent()
        {
            Stop();
            UpdatePropeties();
        }

        void OnSwitchToBreakEvent()
        {
            Stop();
            UpdatePropeties();
        }

        void OnSwitchToLongBreakEvent()
        {
            Stop();
            UpdatePropeties();
        }

        void OnCompletePomodoroEvent()
        {
            Stop();
            UpdatePropeties();
            ProgressState = TaskbarItemProgressState.Paused;
        }

        public void OnTick()
        {
            pomodoro.Tick();
            UpdatePropeties();
        }

        // Properties for Data binding
        public double Progress { get { return pomodoro.Progress; } }
        public TaskbarItemProgressState ProgressState { get; private set; }
        public string Minute { get { return PomodoleHelper.ShapeTimeNumber(pomodoro.GetMinute()); } }
        public string Second { get { return PomodoleHelper.ShapeTimeNumber(pomodoro.GetSecond()); } }
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
                switch (pomodoro.CurrentPhase)
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
                if (!TimerRunning) return MessageResource.GetMessageFor(Message.MainButtonStartmessage);
                else return MessageResource.GetMessageFor(Message.MainButtonStopMessage);
            }
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

        public void Configure(IConfigManager configManager)
        {
            pomodoro.Configure(configManager);
        }

        public ICommand StartCommand { get; private set; }
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
