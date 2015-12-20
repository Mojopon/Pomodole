using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pomodole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        public Action<IApplicationMessage> Subject { get; private set; }

        private MainWindowServiceController mainWindowServiceController;
        public MainWindow()
        {
            InitializeComponent();
            mainWindowServiceController = new MainWindowServiceController(this);
            Subject += ((IApplicationMessage message) => message.Execute(this));
        }


        public void ActivateWindow()
        {
            mainWindowServiceController.ActivateWindow();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
