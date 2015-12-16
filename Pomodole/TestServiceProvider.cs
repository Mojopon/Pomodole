using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public class TestServiceProvider : ServiceProvider
    {
        private IMainWindowViewModel mainWindowViewModel;
        private IConfigWindowViewModel configWindowViewModel;

        public TestServiceProvider(IApplicationController applicationController) : base() { }

        public override object GetView(ViewFor view)
        {
            return null;
        }

        public void SetMainWindowViewModel(IMainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public void SetConfigWindowViewModel(IConfigWindowViewModel configWindowViewModel)
        {
            this.configWindowViewModel = configWindowViewModel;
        }

        public override IMainWindowViewModel GetMainWindowViewModel()
        {
            return mainWindowViewModel;
        }

        public override IConfigWindowViewModel GetConfigWindowViewModel()
        {
            return configWindowViewModel;
        }
    }
}
