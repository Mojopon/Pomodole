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
using System.Windows.Media;

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

            _backgroundStartColor = Colors.White;
            _backgroundEndColor = Colors.PeachPuff;
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
            itWillSwitchColor = true;
            Stop();
            UpdatePropeties();
        }

        void OnSwitchToBreakEvent()
        {
            itWillSwitchColor = true;
            Stop();
            UpdatePropeties();
        }

        void OnSwitchToLongBreakEvent()
        {
            itWillSwitchColor = true;
            Stop();
            UpdatePropeties();
        }

        void OnCompletePomodoroEvent()
        {
            itWillSwitchColor = true;
            Stop();
            UpdatePropeties();
            ProgressState = TaskbarItemProgressState.Paused;
        }

        private bool itWillSwitchColor = false;
        public void OnTick()
        {
            pomodoro.Tick();
            if (itWillSwitchColor)
            {
                SwitchBackgroundColor();
                _backgroundGradiationEndPoint = new Point(0, 0);
                itWillSwitchColor = false;
            }
            else
            {
                _backgroundGradiationEndPoint = new Point(pomodoro.Progress, 0);
            }
            UpdatePropeties();
        }

        // Properties for Data binding
        public double Progress { get { return pomodoro.Progress; } }
        public TaskbarItemProgressState ProgressState { get; private set; }
        private Point _backgroundGradiationEndPoint;
        public Point BackgroundGradiationEndpoint {
            get { return _backgroundGradiationEndPoint; }
            set
            {
                _backgroundGradiationEndPoint = value;
                NotifyPropertyChanged("BackgroundGradiationEndpoint");
            }
        }
        private Color _backgroundStartColor;
        public Color BackgroundStartColor
        {
            get { return _backgroundStartColor; }
            set
            {
                _backgroundStartColor = value;
                NotifyPropertyChanged("BackgroundStartColor");
            }
        }
        private Color _backgroundEndColor;
        public Color BackgroundEndColor
        {
            get { return _backgroundEndColor; }
            set
            {
                _backgroundEndColor = value;
                NotifyPropertyChanged("BackgroundEndColor");
            }
        }

        private void SwitchBackgroundColor()
        {
            var temp = BackgroundStartColor;
            BackgroundStartColor = BackgroundEndColor;
            BackgroundEndColor = temp;
        }

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
            NotifyPropertyChanged("BackgroundGradiationEndpoint");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
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
