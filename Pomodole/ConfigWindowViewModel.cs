using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ConfigWindowViewModel : IConfigWindowViewModel
    {
        public event Action OpenConfigWindow;
        public event Action<IApplicationMessage> SendMessage;

        private IConfigManager configManager;
        public ConfigWindowViewModel(IConfigManager configManager)
        {
            this.configManager = configManager;
        }

        public void Open()
        {
            if (OpenConfigWindow != null) OpenConfigWindow();
        }
    }
}
