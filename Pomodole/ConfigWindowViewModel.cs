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
        #region IApplicationMessageUser method group
        public IApplicationMessageEvent Messenger { get; private set; }
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

        private IConfigManager configManager;
        public ConfigWindowViewModel(IApplicationMessageEvent applicationMessageEvent, IConfigManager configManager)
        {
            Messenger = applicationMessageEvent;
            this.configManager = configManager;

            OkButtonCommand = new OkButtonCommandImpl(applicationMessageEvent, configManager, this);

            Subject += ((IApplicationMessage m) => m.Execute(this));
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
            private IApplicationMessageEvent messenger;
            private IConfigManager configManager;
            private IConfigWindowViewModel viewModel;
            public OkButtonCommandImpl(IApplicationMessageEvent messenger, IConfigManager configManager, IConfigWindowViewModel viewModel)
            {
                this.messenger = messenger;
                this.configManager = configManager;
                this.viewModel = viewModel;
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                messenger.Trigger(new ChangeConfigurationMessage(configManager));
                messenger.Trigger(new ConfigurationDataManagingMessage(messenger, ConfigurationDataManagingMessage.ActionType.Save));

                var command = ToggleConfigWindowApplicationMessage.CommandType.Close;
                messenger.Trigger(new ToggleConfigWindowApplicationMessage(command));
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
                        {
                            
                            Console.WriteLine("error occured");
                            result = "Task time cannot be 0 or minus";
                            
                        }
                        break;
                }

                return result;
            }
        }
    }
}
