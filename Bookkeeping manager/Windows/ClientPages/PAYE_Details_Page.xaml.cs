using Bookkeeping_manager.src.Clients;
using System.Windows;
using System.Windows.Controls;
using ToggleSwitch;

namespace Bookkeeping_manager.Windows.ClientPages
{
    /// <summary>
    /// Interaction logic for PAYE_Details_Page.xaml
    /// </summary>
    public partial class PAYE_Details_Page : Page
    {
        private PAYE_Details_Data PAYE { get; set; }
        public PAYE_Details_Page(Client client)
        {
            PAYE = client.PAYE_Details;
            InitializeComponent();

            WeeklyToggle.IsChecked = PAYE.Weekly_PayRollEnabled;
            TwoWeeklyToggle.IsChecked = PAYE.TwoWeekly_PayRollEnabled;
            MonthlyToggle.IsChecked = PAYE.Monthly_PayRollEnabled;

            PAYE.Initalize(Weekly_DateTB, TwoWeekly_DateTB, Monthly_DateTB,
                (Style)FindResource("RegularTB"), (Style)FindResource("ReadOnlyTB"));
            DataContext = PAYE;
        }

        private void SwitchToggle_On(object sender, RoutedEventArgs e)
        {
            HorizontalToggleSwitch s = (HorizontalToggleSwitch)sender;
            string name = s.Name;

            switch (name)
            {
                case "WeeklyToggle":
                    PAYE.Weekly_PayRollEnabled = true;
                    break;
                case "TwoWeeklyToggle":
                    PAYE.TwoWeekly_PayRollEnabled = true;
                    break;
                case "MonthlyToggle":
                    PAYE.Monthly_PayRollEnabled = true;
                    break;
            }
        }
        private void SwitchToggle_Off(object sender, RoutedEventArgs e)
        {
            HorizontalToggleSwitch s = (HorizontalToggleSwitch)sender;
            string name = s.Name;

            switch (name)
            {
                case "WeeklyToggle":
                    PAYE.Weekly_PayRollEnabled = false;
                    break;
                case "TwoWeeklyToggle":
                    PAYE.TwoWeekly_PayRollEnabled = false;
                    break;
                case "MonthlyToggle":
                    PAYE.Monthly_PayRollEnabled = false;
                    break;
            }
        }
    }
}
