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
            return GetInstance(ServiceProvider.GetInstance());
        }
        public static IApplicationController GetInstance(IPomodoleServiceProvider serviceProvider)
        {
            if(applicationController == null)
            {
                applicationController = new ApplicationController(serviceProvider);
                applicationController.Initialize();
            }
            return applicationController;
        }
        public static void ResetInstance()
        {
            applicationController = null;
        }

        private IPomodoleServiceProvider serviceProvider;

        private List<IViewModel> viewModels;
        private ConfigManager configManager;
        private ApplicationController(IPomodoleServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            viewModels = new List<IViewModel>();
        }

        private void Initialize()
        {
            SetupConfigManager();
            SetupViewModelForMainWindow();
        }

        private void SetupConfigManager()
        {
            configManager = new ConfigManager();
            configManager.SetupPomodoroConfig(25, 5, 3, 15);
        }


        private IMainWindowViewModel mainWindowViewModel;
        private void SetupViewModelForMainWindow()
        {
            mainWindowViewModel = serviceProvider.GetMainWindowViewModel();
            mainWindowViewModel.Configure(configManager);
            viewModels.Add(mainWindowViewModel);
        }

        public object GetViewModel(ViewModelFor viewModel)
        {
            switch(viewModel)
            {
                case ViewModelFor.MainWindow:
                    return mainWindowViewModel;
                default:
                    return null;
            }
        }

        public void SendMessage(IApplicationMessage message)
        {
            foreach(IViewModel viewModel in viewModels)
            {
                message.Execute(viewModel);
            }
        }
    }
}
