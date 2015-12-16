using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{

    public class ApplicationController : IApplicationController, IApplicationMessageSubscriber
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

        public Action<IApplicationMessage> Subject { get; private set; }
        private ApplicationController()
        {
            applicationMessageEvent = new ApplicationMessageEvent();
            Subject += ((IApplicationMessage message) => message.Execute(this));
            Register(this);
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
            Show(ViewFor.MainWindow);
        }

        #region IApplicationController Method Group
        private IMainWindow mainWindow;
        private IConfigWindow configWindow;
        public void Show(ViewFor view)
        {
            switch(view)
            {
                case ViewFor.MainWindow:
                    {
                        mainWindow = (IMainWindow)serviceProvider.GetView(ViewFor.MainWindow);
                        Register(mainWindow);
                        mainWindow.Show();
                    }
                    break;
                case ViewFor.ConfigWindow:
                    {
                        configWindow = (IConfigWindow)serviceProvider.GetView(ViewFor.ConfigWindow);
                        Register(configWindow);
                        configWindow.Show();
                    }
                    break;
            }
        }

        public void Close(ViewFor view)
        {
            switch (view)
            {
                case ViewFor.MainWindow:
                    {
                        mainWindow.Close();
                    }
                    break;
                case ViewFor.ConfigWindow:
                    {
                        configWindow.Close();
                        configWindow = null;
                    }
                    break;
            }
        }
        #endregion

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
