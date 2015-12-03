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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pomodole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PomodoroViewModel pomodoroViewModel;
        public MainWindow()
        {
            InitializeComponent();

            var pomodoroConfig = new PomodoroConfig(5, 3, 2, 10);
            Pomodoro newPomodoro = new Pomodoro();
            newPomodoro.Configure(pomodoroConfig);

            pomodoroViewModel = new PomodoroViewModel(newPomodoro);

            TimerGrid.DataContext = pomodoroViewModel;
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            pomodoroViewModel.OnProgressTime();
        }
    }
}
