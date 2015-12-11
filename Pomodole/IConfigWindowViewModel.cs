using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pomodole
{
    public interface IConfigWindowViewModel : IViewModel
    {
        event Action OpenConfigWindow;

        void Open();
    }
}
