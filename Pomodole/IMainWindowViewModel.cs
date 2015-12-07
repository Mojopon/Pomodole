using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace Pomodole
{
    public interface IMainWindowViewModel : INotifyPropertyChanged
    {
        double Progress { get; }
        TaskbarItemProgressState ProgressState { get; }
        string Minute { get; }
        string Second { get; }
        string PomodoroSetMessage { get; }
        string MainButtonMessage { get; }
    }
}
