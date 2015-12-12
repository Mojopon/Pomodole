using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pomodole
{
    public class ConfigWindowViewModel : IConfigWindowViewModel, INotifyPropertyChanged
    {
        public event Action OpenConfigWindow;
        public event Action<IApplicationMessage> SendMessage;

        private IConfigManager configManager;

        public UIElement Child { get; private set; }

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

        public ConfigWindowViewModel(IConfigManager configManager)
        {
            this.configManager = configManager;

            Child = new PomodoroConfigControl();
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
    }
}
