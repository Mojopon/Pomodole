using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ProductionServiceProvider : ServiceProvider
    {
        private ProductionServiceProvider() { }
        private IApplicationController applicationController;
        public ProductionServiceProvider(IApplicationController applicationController) : base()
        {
            this.applicationController = applicationController;
            SetupViewModels();
        }

        private IMainWindowViewModel mainWindowViewModel;
        private IConfigWindowViewModel configWindowViewModel;
        private void SetupViewModels()
        {
            mainWindowViewModel = new MainWindowViewModel(applicationController, pomodoro);

            configWindowViewModel = new ConfigWindowViewModel(applicationController, configManager);
            configWindowViewModel.OpenConfigWindow += (() => OpenConfigWindow());
            configWindowViewModel.CloseConfigWindow += (() => CloseConfigWindow());

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
        public override object GetView(ViewFor view)
        {
            switch(view)
            {
                case ViewFor.MainWindow:
                    {
                        if (mainWindow == null)
                            mainWindow = new MainWindow();
                        var mainWindowService = new MainWindowService(mainWindow);
                        mainWindowViewModel.RegisterMainWindowService(mainWindowService);
                        mainWindow.DataContext = mainWindowViewModel;
                        return mainWindow;
                    }
                case ViewFor.ConfigWindow:
                    {
                        var configWindow = new ConfigWindow();
                        configWindow.DataContext = configWindowViewModel;
                        return configWindow;
                    }
                default:
                    return null;
            }
        }

        public override IMainWindowViewModel GetMainWindowViewModel()
        {
            return mainWindowViewModel;
        }

        public override IConfigWindowViewModel GetConfigWindowViewModel()
        {
            return configWindowViewModel;
        }
    }
}
