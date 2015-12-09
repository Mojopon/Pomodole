using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ChangeConfigurationMessage : IApplicationMessage
    {
        private IConfigManager configManager;
        public ChangeConfigurationMessage(IConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public void Execute(IViewModel target)
        {
            var configurable = target as IConfigurable;
            if (configurable != null)
            {
                configurable.Configure(configManager);
            }
        }
    }
}
