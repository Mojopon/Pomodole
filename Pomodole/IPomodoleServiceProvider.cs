﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IPomodoleServiceProvider
    {
        object GetView(ViewFor viewFor);
        IMainWindowViewModel GetMainWindowViewModel(IApplicationController applicationController);
        IConfigWindowViewModel GetConfigWindowViewModel(IApplicationController applicationController);
    }
}