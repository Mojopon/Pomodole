using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class OpenConfigWindowApplicationMessage : IApplicationMessage
    {
        public OpenConfigWindowApplicationMessage() { }

        public void Execute(object target)
        {
            var applicationController = target as IApplicationController;
            if(applicationController != null)
            {
                applicationController.Show(ViewFor.ConfigWindow);
            }
        }
    }
}
