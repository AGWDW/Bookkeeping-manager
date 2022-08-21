using Bookkeeping_manager.src.Clients;
using System.Windows;
using System.Windows.Controls;

namespace Bookkeeping_manager.Windows.ClientPages
{
    /// <summary>
    /// Interaction logic for ClientInfoViewer.xaml
    /// </summary>
    public partial class CompnayDetails_Page : Page
    {
        public ClientInfomation_Data ClientInfomation { get; set; }
        public CompnayDetails_Page(Client client)
        {
            ClientInfomation = client.ClientInfomation;
            InitializeComponent();
            ConfireEnabledToggle.IsChecked = ClientInfomation.ConfirmationEnabled;

            ClientInfomation.Initalize(ConfirDateBox, FindResource("ReadOnlyTB") as Style, FindResource("RegularTB") as Style);
            DataContext = ClientInfomation;
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _ = 0;
        }

        private void ConfirEnableToggle_On(object sender, RoutedEventArgs e)
        {
            ClientInfomation.ConfirmationEnabled = true;
        }

        private void ConfirEnableToggle_Off(object sender, RoutedEventArgs e)
        {
            ClientInfomation.ConfirmationEnabled = false;
        }
    }
}
