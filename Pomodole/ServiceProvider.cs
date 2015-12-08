using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public abstract class ServiceProvider : IServiceProvider
    {
        public static IServiceProvider GetInstance()
        {
            return GetInstance(ServiceProviderType.Production);
        }

        public static IServiceProvider GetInstance(ServiceProviderType providerType)
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

        public abstract IMainWindowViewModel GetMainWindowViewModel();
    }
}
