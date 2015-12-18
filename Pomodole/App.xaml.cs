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
        public static string ConfigurationFileName { get { return "configurations.xml"; } }
        public static ServiceType ServiceType { get; set; }

        private IPomodoleServiceProvider serviceProvider;
        private ApplicationController applicationController;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ServiceType = ServiceType.Production;
            applicationController = ApplicationController.Create();
            serviceProvider = ServiceProvider.Create(applicationController, ServiceType.Production);
            applicationController.Initialize(serviceProvider);
            applicationController.Start();
        }
    }
}
