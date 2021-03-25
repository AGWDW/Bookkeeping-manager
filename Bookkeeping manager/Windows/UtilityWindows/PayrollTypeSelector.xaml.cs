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

namespace Bookkeeping_manager.Windows.UtilityWindows
{
    /// <summary>
    /// Interaction logic for PayrollTypeSelector.xaml
    /// </summary>
    public partial class PayrollTypeSelector : Window
    {
        public string Selected { get; set; }
        public PayrollTypeSelector()
        {
            InitializeComponent();
            Selected = "";
        }

        private void WeeklyButton_Click(object sender, RoutedEventArgs e)
        {
            Selected = "Weekly";
            Close();
        }

        private void BiWeeklyButton_Click(object sender, RoutedEventArgs e)
        {
            Selected = "2Weekly";
            Close();
        }

        private void MonthlyButton_Click(object sender, RoutedEventArgs e)
        {
            Selected = "Monthly";
            Close();
        }
    }
}
