using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ToggleConfigWindowApplicationMessage : IApplicationMessage
    {
        public enum CommandType
        {
            Show,
            Close,
        }

        private CommandType type;
        public ToggleConfigWindowApplicationMessage(CommandType type)
        {
            this.type = type;
        }

        public void Execute(object target)
        {
            var applicationController = target as IApplicationController;
            if (applicationController != null)
            {
                switch (type)
                {
                    case CommandType.Show:
                        {
                            applicationController.Show(ViewFor.ConfigWindow);
                        }
                        break;
                    case CommandType.Close:
                        {
                            applicationController.Close(ViewFor.ConfigWindow);
                        }
                        break;
                }
            }
        }
    }
}
