using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IMainWindowViewModel : INotifyPropertyChanged, IMainWindowViewElements
    {
        int Progress { get; }
    }
}
