using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ConfigurationDataManagingMessage : IApplicationMessage
    {
        private IApplicationMessageEvent messenger;
        private ActionType actionType;
        public ConfigurationDataManagingMessage(IApplicationMessageEvent messenger, ActionType actionType)
        {
            this.messenger = messenger;
            this.actionType = actionType;
        }

        public void Execute(object target)
        {
            var configManager = target as IConfigManager;
            if(configManager != null)
            {
                switch(actionType)
                {
                    case ActionType.Save:
                        configManager.SaveConfigurationToFile();
                        break;
                    case ActionType.Load:
                        configManager.LoadConfigurationFromFile();
                        messenger.Trigger(new ChangeConfigurationMessage(configManager));
                        return;
                }
            }
        }

        public enum ActionType
        {
            Save,
            Load,
        }
    }
}
