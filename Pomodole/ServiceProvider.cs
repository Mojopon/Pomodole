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
            configManager = newConfigManager;
            pomodoro = new Pomodoro();
            pomodoro.Configure(configManager);
        }

        public static IPomodoleServiceProvider Create(IApplicationController applicationController, ServiceType providerType)
        {
            switch (providerType)
            {
                case ServiceType.Production:
                default:
                    var productionServiceProvider = new ProductionServiceProvider(applicationController);
                    return productionServiceProvider;
                case ServiceType.Test:
                    return new TestServiceProvider(applicationController);
            }
        }

        public abstract object GetView(ViewFor view);
        public abstract IConfigManager GetConfigManager();
        public abstract IMainWindowViewModel GetMainWindowViewModel();
        public abstract IConfigWindowViewModel GetConfigWindowViewModel();
    }
}
