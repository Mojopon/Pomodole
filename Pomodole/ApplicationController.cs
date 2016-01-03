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
#if DEBUG
            applicationController = null;
#endif
        }

        public Action<IApplicationMessage> Subject { get; private set; }
        private ApplicationController()
        {
            applicationMessageEvent = new ApplicationMessageEvent();
            Subject += ((IApplicationMessage message) => message.Execute(this));
            Subscribe(this);
        }

        private IPomodoleServiceProvider serviceProvider;
        // it needs to be initialized first 
        public void Initialize(IPomodoleServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            Subscribe(serviceProvider.GetConfigManager());
            SetupViewModelForMainWindow();
            SetupViewModelForConfigWindow();

            Trigger(new ConfigurationDataManagingMessage(this, ConfigurationDataManagingMessage.ActionType.Load));
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
                        Subscribe(mainWindow);
                        mainWindow.Show();
                    }
                    break;
                case ViewFor.ConfigWindow:
                    {
                        var newConfigWindow = (IConfigWindow)serviceProvider.GetView(ViewFor.ConfigWindow);
                        // service provider returns null when configwindow is already opened so it returns if the given object is null
                        if (newConfigWindow == null) return;
                        Subscribe(newConfigWindow);
                        newConfigWindow.Show();
                        configWindow = newConfigWindow;
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
                        UnSubscribe(configWindow);
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

        public void Subscribe(IApplicationMessageSubscriber subscriber)
        {
            applicationMessageEvent.Subscribe(subscriber);
        }

        public void UnSubscribe(IApplicationMessageSubscriber subscriber)
        {
            applicationMessageEvent.UnSubscribe(subscriber);
        }
        #endregion

        private IMainWindowViewModel mainWindowViewModel;
        private void SetupViewModelForMainWindow()
        {
            mainWindowViewModel = serviceProvider.GetMainWindowViewModel();
            Subscribe(mainWindowViewModel);
        }

        private IConfigWindowViewModel configWindowViewModel;

        private void SetupViewModelForConfigWindow()
        {
            configWindowViewModel = serviceProvider.GetConfigWindowViewModel();
            Subscribe(configWindowViewModel);
        }
    }
}
