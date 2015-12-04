using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IMainWindowViewElements
    {
        string StartButtonMessage { get; }
        string Minute { get; }
        string Second { get; }
        string PomodoroSetMessage { get; }
    }
}
