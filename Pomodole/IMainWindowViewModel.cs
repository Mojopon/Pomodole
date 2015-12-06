using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace Pomodole
{
    public interface IMainWindowViewModel : INotifyPropertyChanged, IMainWindowViewElements
    {
        double Progress { get; }
        TaskbarItemProgressState ProgressState { get; }
    }
}
