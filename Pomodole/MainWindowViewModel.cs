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
        public bool TimerRunning { get; private set; }

        private Color backgroundColorForTaskMode = Colors.White;
        private Color backgroundColorForBreakMode = Colors.PeachPuff;

        private IPomodoro pomodoro;
        private ITickTimer tickTimer;

        #region IApplicationMessageUser method group
        public IApplicationMessageEvent Messenger { get; private set; }
        public Action<IApplicationMessage> Subject { get; private set; }
        #endregion

        public MainWindowViewModel(IApplicationMessageEvent applicationMessageEvent, IPomodoro pomodoro)
        {
            // setup for ApplicationMessageEvent to communicate with other viewmodels and views
            Messenger = applicationMessageEvent;
            Subject += ((IApplicationMessage m) => m.Execute(this));

            this.pomodoro = pomodoro;
            pomodoro.OnSwitchToBreak += new Action(OnSwitchToBreakEvent);
            pomodoro.OnSwitchToTask += new Action(OnSwitchToTaskEvent);
            pomodoro.OnSwitchToLongBreak += new Action(OnSwitchToLongBreakEvent);
            pomodoro.OnCompletePomodoro += new Action(OnCompletePomodoroEvent);

            tickTimer = new TickTimer(50);
            tickTimer.OnTick += new Action(OnTick);
            StartCommand = new StartCommandImpl(this);
            ConfigButtonCommand = new ConfigButtonCommandImpl(applicationMessageEvent);

            InitializeBackgroundColor();
        }

        private void InitializeBackgroundColor()
        {
            _backgroundStartColor = backgroundColorForTaskMode;
            _backgroundEndColor = backgroundColorForBreakMode;
        }

        private void ActivateWindow()
        {
            Messenger.Trigger(new ActivateMainWindowMessage());
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
            ActivateWindow();
        }

        void OnSwitchToBreakEvent()
        {
            itWillSwitchColor = true;
            Stop();
            UpdatePropeties();
            ActivateWindow();
        }

        void OnSwitchToLongBreakEvent()
        {
            itWillSwitchColor = true;
            Stop();
            UpdatePropeties();
            ActivateWindow();
        }

        void OnCompletePomodoroEvent()
        {
            itWillSwitchColor = true;
            Stop();
            UpdatePropeties();
            ProgressState = TaskbarItemProgressState.Paused;
            ActivateWindow();
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
            ActivateWindow();
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
                switch(pomodoro.CurrentPhase)
                {
                    case PomodoroPhase.NotRunning:
                    case PomodoroPhase.WaitingSwitchToTask:
                        return MessageResource.GetMessageFor(Message.StartTaskMessage);
                    case PomodoroPhase.WaitingSwitchToBreak:
                        return MessageResource.GetMessageFor(Message.StartBreakMessage);
                    case PomodoroPhase.WaitingSwitchToLongBreak:
                        return MessageResource.GetMessageFor(Message.StartLongBreakMessage);
                    case PomodoroPhase.RunningLongBreak:
                        return MessageResource.GetMessageFor(Message.LongBreakMessage);
                    default:
                        return MessageResource.GetMessageFor(Message.DisplayPomodoroSetMessage, pomodoro.GetRepeatTimeLeft().ToString());
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

        public void Configure(IConfigManager configManager)
        {
            pomodoro.Configure(configManager);
            UpdatePropeties();
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

        public ICommand ConfigButtonCommand { get; private set; }
        class ConfigButtonCommandImpl : ICommand
        {
            private IApplicationMessageEvent applicationMessageEvent;
            public ConfigButtonCommandImpl(IApplicationMessageEvent applicationMessageEvent)
            {
                this.applicationMessageEvent = applicationMessageEvent;
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                var command = ToggleConfigWindowApplicationMessage.CommandType.Show;
                applicationMessageEvent.Trigger(new ToggleConfigWindowApplicationMessage(command));
            }
        }
    }
}
