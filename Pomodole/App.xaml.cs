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
        private IApplicationController applicationController;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            applicationController = ApplicationController.GetInstance();
            var mainWindowViewModel = applicationController.GetViewModel(ViewModelFor.MainWindow);
            var mainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
            mainWindow.Show();
        }
    }
}
