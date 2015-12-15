using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pomodole
{
    public interface IConfigWindowViewModel : IViewModel, IPomodoroConfig, IApplicationMessageUser
    {
        event Action OpenConfigWindow;
        void Open();
        event Action CloseConfigWindow;
        void Close();

        ICommand OkButtonCommand { get; }
    }
}
