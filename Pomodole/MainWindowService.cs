using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pomodole
{
    public class MainWindowServiceController : IMainWindowService
    {
        private Window window;
        public MainWindowServiceController(Window window)
        {
            this.window = window;
        }

        public void ActivateWindow()
        {
            if (!window.IsVisible)
            {
                window.Show();
            }

            if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;
            }

            window.Activate();
            window.Topmost = true;  // important
            window.Topmost = false; // important
            window.Focus();         // important
        }
    }
}
