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

        private Dictionary<ViewFor, object> views = new Dictionary<ViewFor, object>();
        public void SetView(ViewFor viewType, object view)
        {
            views.Add(viewType, view);
        }

        public override object GetView(ViewFor view)
        {
            return views[view];
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
