using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pomodole
{
    public class ConfigWindowViewModel : IConfigWindowViewModel, INotifyPropertyChanged, IDataErrorInfo
    {
        public event Action OpenConfigWindow;
        public event Action CloseConfigWindow;

        #region IApplicationMessageUser method group
        public IApplicationMessageEvent ApplicationMessageEvent { get; private set; }
        public Action<IApplicationMessage> Subject { get; private set; }
        #endregion

        public int TaskTime
        {
            get { return configManager.TaskTime; }
            set { configManager.TaskTime= value; }
        }
        public int BreakTime
        {
            get { return configManager.BreakTime; }
            set { configManager.BreakTime = value; }
        }
        public int RepeatTime
        {
            get { return configManager.RepeatTime; }
            set { configManager.RepeatTime = value; }
        }
        public int LongBreakTime
        {
            get { return configManager.LongBreakTime; }
            set { configManager.LongBreakTime = value; }
        }

        private IApplicationMessageEvent applicationMessageEvent;
        private IConfigManager configManager;
        public ConfigWindowViewModel(IApplicationMessageEvent applicationMessageEvent, IConfigManager configManager)
        {
            this.applicationMessageEvent = applicationMessageEvent;
            this.configManager = configManager;

            OkButtonCommand = new OkButtonCommandImpl(applicationMessageEvent, configManager, this);

            Subject += ((IApplicationMessage m) => m.Execute(this));
        }

        public void Open()
        {
            if (OpenConfigWindow != null) OpenConfigWindow();
        }

        public void Close()
        {
            if (CloseConfigWindow != null) CloseConfigWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public ICommand OkButtonCommand { get; private set; }

        class OkButtonCommandImpl : ICommand
        {
            private IApplicationMessageEvent applicationMessageEvent;
            private IConfigManager configManager;
            public OkButtonCommandImpl(IApplicationMessageEvent applicationMessageEvent, IConfigManager configManager, IConfigWindowViewModel viewModel)
            {
                this.applicationMessageEvent = applicationMessageEvent;
                this.configManager = configManager;
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                applicationMessageEvent.Trigger(new ChangeConfigurationMessage(configManager));
            }
        }

        public string Error { get { return null; } }

        public string this[string propertyName]
        {
            get
            {
                string result = null;

                switch (propertyName)
                {
                    case "TaskTime":
                        if (TaskTime <= 0)
                            result = "Task time cannot be 0 or minus";
                        break;
                }

                return result;
            }
        }
    }
}
