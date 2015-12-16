﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IApplicationController : IApplicationMessageEvent
    {
        void Show(ViewFor view);
        void Close(ViewFor view);
    }
}
