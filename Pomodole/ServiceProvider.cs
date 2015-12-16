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

        public static IPomodoleServiceProvider Create(IApplicationController applicationController, ServiceProviderType providerType)
        {
            switch (providerType)
            {
                case ServiceProviderType.Production:
                default:
                    var productionServiceProvider = new ProductionServiceProvider(applicationController);
                    return productionServiceProvider;
                case ServiceProviderType.Test:
                    return new TestServiceProvider(applicationController);
            }
        }

        public abstract object GetView(ViewFor view);
        public abstract IMainWindowViewModel GetMainWindowViewModel();
        public abstract IConfigWindowViewModel GetConfigWindowViewModel();
    }
}
