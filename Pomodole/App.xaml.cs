using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Pomodole
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IPomodoleServiceProvider serviceProvider;
        private IApplicationController applicationController;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            serviceProvider = ServiceProvider.GetInstance(ServiceProviderType.Production);
            applicationController = ApplicationController.GetInstance(serviceProvider);
            var mainWindow = (MainWindow)applicationController.GetView(ViewFor.MainWindow);
            mainWindow.Show();
        }
    }
}
