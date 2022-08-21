using Bookkeeping_manager.src.Clients;
using System.Windows;
using System.Windows.Controls;
using ToggleSwitch;

namespace Bookkeeping_manager.Windows.ClientPages
{

    /// <summary>
    /// Interaction logic for ServicesInfomation_Page.xaml
    /// </summary>
    public partial class ServicesInfomation_Page : Page
    {
        private ServicesInfomation_Data Services { get; set; }
        public ServicesInfomation_Page(Client client)
        {
            Services = client.ServiceInfomation;
            InitializeComponent();

            SAToggle.IsChecked = Services.SelfAssessmentEnabled;
            P11DToggle.IsChecked = Services.P11D_Enabled;

            Services.Initalize(SelfAssess_TB, P11D_TB, (Style)FindResource("RegularTB"), (Style)FindResource("ReadOnlyTB"));
            DataContext = Services;
        }

        private void Toggle_Checked_On(object sender, RoutedEventArgs e)
        {
            HorizontalToggleSwitch s = (HorizontalToggleSwitch)sender;
            string name = s.Name;
            switch (name)
            {
                case "SAToggle":
                    Services.SelfAssessmentEnabled = true;
                    break;
                case "P11DToggle":
                    Services.P11D_Enabled = true;
                    break;
            }
        }
        private void Toggle_Checked_Off(object sender, RoutedEventArgs e)
        {
            HorizontalToggleSwitch s = (HorizontalToggleSwitch)sender;
            string name = s.Name;
            switch (name)
            {
                case "SAToggle":
                    Services.SelfAssessmentEnabled = false;
                    break;
                case "P11DToggle":
                    Services.P11D_Enabled = false;
                    break;
            }
        }
    }
}
