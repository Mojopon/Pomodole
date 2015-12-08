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
                applicationController = new ApplicationController(ServiceProvider.GetInstance());
                applicationController.Initialize();
            }
            return applicationController;
        }

        private IServiceProvider serviceProvider;

        private List<IViewModel> viewModels;
        private ConfigManager configManager;
        private ApplicationController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            viewModels = new List<IViewModel>();
        }

        private void Initialize()
        {
            configManager = new ConfigManager();
            configManager.SetupPomodoroConfig(25, 5, 3, 15);

            SetupViewModelForMainWindow();
        }


        private IMainWindowViewModel mainWindowViewModel;
        void SetupViewModelForMainWindow()
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
    }
}
