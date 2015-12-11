using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class ProductionServiceProvider : ServiceProvider
    {
        public ProductionServiceProvider() : base() { }

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
                case ViewFor.ConfigWindow:
                    return new ConfigWindow();
                default:
                    return null;
            }
        }

        public override IMainWindowViewModel GetMainWindowViewModel()
        {
            return new MainWindowViewModel(pomodoro);
        }

        public override IConfigWindowViewModel GetConfigWindowViewModel()
        {
            return new ConfigWindowViewModel(configManager);
        }
    }
}
