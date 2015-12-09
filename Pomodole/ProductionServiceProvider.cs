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

        private MainWindow mainWindow;
        public override object GetView(ViewFor view)
        {
            switch(view)
            {
                case ViewFor.MainWindow:
                    {
                        if (mainWindow == null)
                            mainWindow = new MainWindow();
                        return mainWindow;
                    }
                default:
                    return null;
            }
        }

        public override IMainWindowViewModel GetMainWindowViewModel()
        {
            var newPomodoro = new Pomodoro();
            return new MainWindowViewModel(newPomodoro);
        }
    }
}
