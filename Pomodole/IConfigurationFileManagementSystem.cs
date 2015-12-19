using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodole
{
    public interface IConfigurationFileManagementSystem
    {
        void Save<T>(T obj, string fileName);
        T Load<T>(string fileName);
    }
}
