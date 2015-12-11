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
        private ApplicationController(IPomodoleServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            viewModels = new List<IViewModel>();
        }

        private void Initialize()
        {
            SetupViewModelForMainWindow();
            SetupViewModelForConfigWindow();
        }

        private IMainWindowViewModel mainWindowViewModel;
        private void SetupViewModelForMainWindow()
        {
            mainWindowViewModel = serviceProvider.GetMainWindowViewModel();
            RegisterViewModel(mainWindowViewModel);
        }

        private IConfigWindowViewModel configWindowViewModel;
        private void SetupViewModelForConfigWindow()
        {
            configWindowViewModel = serviceProvider.GetConfigWindowViewModel();
            configWindowViewModel.OpenConfigWindow += (() => 
            {
                var configWindow = GetView(ViewFor.ConfigWindow) as ConfigWindow;
                configWindow.Show();
            });
            RegisterViewModel(configWindowViewModel);
        }

        private void RegisterViewModel(IViewModel viewModel)
        {
            viewModels.Add(viewModel);
            viewModel.SendMessage += ((IApplicationMessage message) => SendMessage(message));
        }

        private MainWindow mainWindow;
        public object GetView(ViewFor view)
        {
            switch (view)
            {
                case ViewFor.MainWindow:
                    {
                        if (mainWindow == null)
                        {
                            mainWindow = (MainWindow)serviceProvider.GetView(ViewFor.MainWindow);
                            var mainWindowService = new MainWindowService(mainWindow);
                            mainWindowViewModel.RegisterMainWindowService(mainWindowService);
                            mainWindow.DataContext = mainWindowViewModel;
                        }
                        return mainWindow;
                    }
                case ViewFor.ConfigWindow:
                    {
                        var configWindow = serviceProvider.GetView(ViewFor.ConfigWindow);
                        return configWindow;
                    }
                default:
                    return null;
            }
        }

        public object GetViewModel(ViewModelFor viewModel)
        {
            switch(viewModel)
            {
                case ViewModelFor.MainWindow:
                    return mainWindowViewModel;
                case ViewModelFor.ConfigWindow:
                    return configWindowViewModel;
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
