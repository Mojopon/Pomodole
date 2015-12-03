using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface ITickTimer
    {
        event Action OnTick;
        void Start();
        void Stop();
    }
}
