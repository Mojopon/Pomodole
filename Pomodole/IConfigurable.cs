using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IConfigurable
    {
        void Configure(IConfigManager configManager);
    }
}
