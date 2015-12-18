using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IPomodoleServiceProvider
    {
        object GetView(ViewFor viewFor);
        IConfigManager GetConfigManager();
        IMainWindowViewModel GetMainWindowViewModel();
        IConfigWindowViewModel GetConfigWindowViewModel();
    }
}
