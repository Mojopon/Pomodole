using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public static class PomodoleHelper
    {
        public static string ShapeTimeNumber(int time)
        {
            if (time < 10)
            {
                return "0" + time.ToString();
            }

            return time.ToString();
        }
    }
}
