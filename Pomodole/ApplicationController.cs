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
        public static ApplicationController Create()
        {
            if(applicationController == null)
            {
                applicationController = new ApplicationController();
                return applicationController;
            }
            Console.WriteLine("Application controller has been already created!");
            return null;
        }

        public static IApplicationController GetInstance()
        {
            return applicationController;
        }

        public static void ResetInstance()
        {
            applicationController = null;
        }

        private ApplicationController()
        {
            applicationMessageEvent = new ApplicationMessageEvent();
        }

        private IPomodoleServiceProvider serviceProvider;
        // it needs to be initialized first 
        public void Initialize(IPomodoleServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            SetupViewModelForMainWindow();
            SetupViewModelForConfigWindow();
        }

        public void Start()
        {
            var mainWindow = (MainWindow)serviceProvider.GetView(ViewFor.MainWindow);
            mainWindow.Show();
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

        private IMainWindowViewModel mainWindowViewModel;
        private void SetupViewModelForMainWindow()
        {
            mainWindowViewModel = serviceProvider.GetMainWindowViewModel();
            Register(mainWindowViewModel);
        }

        private IConfigWindowViewModel configWindowViewModel;
        private void SetupViewModelForConfigWindow()
        {
            configWindowViewModel = serviceProvider.GetConfigWindowViewModel();
            Register(configWindowViewModel);
        }
    }
}
