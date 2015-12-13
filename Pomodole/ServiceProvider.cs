using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public abstract class ServiceProvider : IPomodoleServiceProvider
    {
        protected IPomodoro pomodoro;
        protected IConfigManager configManager;
        public ServiceProvider()
        {
            var newConfigManager = new ConfigManager();
            newConfigManager.SetupPomodoroConfig(25, 5, 3, 15);
            configManager = newConfigManager;
            pomodoro = new Pomodoro();
            pomodoro.Configure(configManager);
        }

        public static IPomodoleServiceProvider GetInstance()
        {
            return GetInstance(ServiceProviderType.Production);
        }

        public static IPomodoleServiceProvider GetInstance(ServiceProviderType providerType)
        {
            switch (providerType)
            {
                case ServiceProviderType.Production:
                default:
                    return new ProductionServiceProvider();
                case ServiceProviderType.Test:
                    return new TestServiceProvider();
            }
        }

        public abstract object GetView(ViewFor view);
        public abstract IMainWindowViewModel GetMainWindowViewModel(IApplicationController applicationController);
        public abstract IConfigWindowViewModel GetConfigWindowViewModel(IApplicationController applicationController);
    }
}
