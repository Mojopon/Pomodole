using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ApplicationController : IApplicationController
    {
        private static ApplicationController applicationController;
        public static IApplicationController GetInstance()
        {
            if(applicationController == null)
            {
                applicationController = new ApplicationController();
            }
            return applicationController;
        }

        private ConfigManager configManager;
        private ApplicationController()
        {
            configManager = new ConfigManager();
            configManager.SetupPomodoroConfig(25, 5, 3, 15);
        }

        public object GetViewModel(ViewModelFor viewModel)
        {
            switch(viewModel)
            {
                case ViewModelFor.MainWindow:
                    return SetupViewModelForMainWindow();
                default:
                    return null;
            }
        }

        object SetupViewModelForMainWindow()
        {
            var pomodoroConfig = new PomodoroConfig(5, 3, 2, 10);
            var newPomodoro = new Pomodoro();
            newPomodoro.Configure(configManager);

            var pomodoroViewModel = new MainWindowViewModel(newPomodoro);

            return pomodoroViewModel;
        }
    }
}
