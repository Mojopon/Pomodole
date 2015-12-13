using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pomodole
{
    public class ConfigWindowViewModel : IConfigWindowViewModel, INotifyPropertyChanged
    {
        public event Action OpenConfigWindow;

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

        private IApplicationController applicationController;
        private IConfigManager configManager;
        public ConfigWindowViewModel(IApplicationController applicationController, IConfigManager configManager)
        {
            this.applicationController = applicationController;
            this.configManager = configManager;

            OkButtonCommand = new OkButtonCommandImpl(applicationController, configManager);
        }

        public void Open()
        {
            if (OpenConfigWindow != null) OpenConfigWindow();
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
            private IApplicationController applicationController;
            private IConfigManager configManager;
            public OkButtonCommandImpl(IApplicationController applicationController, IConfigManager configManager)
            {
                this.applicationController = applicationController;
                this.configManager = configManager;
            }

            public event EventHandler CanExecuteChanged;
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                applicationController.SendMessage(new ChangeConfigurationMessage(configManager));
            }
        }
    }
}
