using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Pomodole
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ConfigWindow : Window, IConfigWindow
    {
        public static RoutedCommand SubmitCommand = new RoutedCommand();

        public Action<IApplicationMessage> Subject { get; private set; }
        public int NumberOfErrors;

        public ConfigWindow()
        {
            InitializeComponent();

            var pomodoroConfigControl = new PomodoroConfigControl(this);
            ConfigWindowMainGrid.Children.Add(pomodoroConfigControl);
        }

        public void Submit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ICommand okButtonCommand = (DataContext as ConfigWindowViewModel).OkButtonCommand;
            okButtonCommand.Execute(null);
        }

        public void CanExecute_Submit(object sender, CanExecuteRoutedEventArgs e)
        {
            if (NumberOfErrors > 0)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }
    }
}
