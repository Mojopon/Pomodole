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

        private ApplicationController(IPomodoleServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            applicationMessageEvent = new ApplicationMessageEvent();
        }

        #region IApplicationMessageEvent Method Group
        private ApplicationMessageEvent applicationMessageEvent;
        public void Trigger(IApplicationMessage message)
        {
            applicationMessageEvent.Trigger(message);
        }

        public void Register(IApplicationMessageSubscriber subscriber)
        {
            applicationMessageEvent.Register(subscriber);
        }
        #endregion

        private void Initialize()
        {
            SetupViewModelForMainWindow();
            SetupViewModelForConfigWindow();
        }

        private IMainWindowViewModel mainWindowViewModel;
        private void SetupViewModelForMainWindow()
        {
            mainWindowViewModel = serviceProvider.GetMainWindowViewModel(this);
            Register(mainWindowViewModel);
        }

        private IConfigWindowViewModel configWindowViewModel;
        private void SetupViewModelForConfigWindow()
        {
            configWindowViewModel = serviceProvider.GetConfigWindowViewModel(this);
            configWindowViewModel.OpenConfigWindow += (() => OpenConfigWindow());
            configWindowViewModel.CloseConfigWindow += (() => CloseConfigWindow());
            Register(configWindowViewModel);
        }

        private ConfigWindow configWindow;
        private void OpenConfigWindow()
        {
            configWindow = GetView(ViewFor.ConfigWindow) as ConfigWindow;
            configWindow.Show();
        }

        private void CloseConfigWindow()
        {
            configWindow.Close();
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
                        var configWindow = (ConfigWindow)serviceProvider.GetView(ViewFor.ConfigWindow);
                        configWindow.DataContext = configWindowViewModel;
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
    }
}
