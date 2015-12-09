using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;
using System.Windows.Input;

namespace Pomodole
{
    public interface IMainWindowViewModel : IViewModel, INotifyPropertyChanged, IConfigurable
    {
        void RegisterMainWindowService(IMainWindowService mainWindowService);

        double Progress { get; }
        TaskbarItemProgressState ProgressState { get; }
        Point BackgroundGradiationEndpoint { get; }
        System.Windows.Media.Color BackgroundStartColor { get; }
        System.Windows.Media.Color BackgroundEndColor { get; }

        string Minute { get; }
        string Second { get; }
        string PomodoroSetMessage { get; }
        string MainButtonMessage { get; }

        ICommand StartCommand { get; }
    }
}
