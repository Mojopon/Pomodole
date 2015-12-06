using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface ICountdownTimer
    {
        bool CountdownEnd { get; }
        double Progress { get; }

        void Tick();
        void Reset();
        int GetMinute();
        int GetSecond();
    }
}
