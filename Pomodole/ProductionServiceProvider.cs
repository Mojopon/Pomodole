using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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
                        mainWindow.DataContext = mainWindowViewModel;
                        return mainWindow;
                    }
                case ViewFor.ConfigWindow:
                    {
                        if (ConfigWindow.IsOpened) return null;

                        var configWindow = new ConfigWindow();
                        configWindow.DataContext = configWindowViewModel;
                        return configWindow;
                    }
                default:
                    return null;
            }
        }

        public override IConfigManager GetConfigManager()
        {
            return configManager;
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
