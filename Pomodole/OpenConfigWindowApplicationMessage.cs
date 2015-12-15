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
            var configWindowViewModel = target as IConfigWindowViewModel;
            if(configWindowViewModel != null)
            {
                configWindowViewModel.Open();
            }
        }
    }
}
