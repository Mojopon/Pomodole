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
            if(target.GetType() == typeof(IConfigurable))
            {
                (target as IConfigurable).Configure(configManager);
            }
        }
    }
}
