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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PomodoroConfigControl : UserControl
    {
        private ConfigWindow parent;
        public PomodoroConfigControl(ConfigWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if(e.Action == ValidationErrorEventAction.Added)
                parent.NumberOfErrors++;
            else
                parent.NumberOfErrors--;

            Console.WriteLine(parent.NumberOfErrors);
        }
    }
}
