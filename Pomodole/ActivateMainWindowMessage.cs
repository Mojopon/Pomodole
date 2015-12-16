using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ActivateMainWindowMessage : IApplicationMessage
    {
        public void Execute(object target)
        {
            var mainWindowServiceController = target as IMainWindowService;
            if (mainWindowServiceController != null)
            {
                mainWindowServiceController.ActivateWindow();
            }
        }
    }
}
