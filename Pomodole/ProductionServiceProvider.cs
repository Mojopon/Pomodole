using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ProductionServiceProvider : ServiceProvider
    {
        public ProductionServiceProvider() { }

        public override IMainWindowViewModel GetMainWindowViewModel()
        {
            var newPomodoro = new Pomodoro();
            return new MainWindowViewModel(newPomodoro);
        }
    }
}
