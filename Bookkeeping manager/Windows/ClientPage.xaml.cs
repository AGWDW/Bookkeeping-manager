using Bookkeeping_manager.src.Clients;
using Bookkeeping_manager.Windows.ClientPages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Bookkeeping_manager.Windows
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public Client Client { get; set; }
        public ClientPage(Client client)
        {
            InitializeComponent();
            DataContext = this;
            Client = client;

            Label label = new Label()
            {
                Content = "Please Select a Category",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 22),
                Background = Brushes.Transparent
            };
            CategoryFrame.Navigate(label);
        }

        private void ClientCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ClientCategories.SelectedIndex)
            {
                case 0: // compnay detatils
                    CategoryFrame.Navigate(new CompnayDetails_Page(Client));
                    break;
                case 1: // contact details
                    CategoryFrame.Navigate(new ContactDetails_Page(Client));
                    break;
                case 2: // accountant
                    CategoryFrame.Navigate(new AccountantDetails_Page(Client));
                    break;
                case 3: // services
                    CategoryFrame.Navigate(new ServicesInfomation_Page(Client));
                    break;
                case 4: // accounts and returns
                    CategoryFrame.Navigate(new AccountsAndReturns_Page(Client));
                    break;
                case 5: // vat details
                    CategoryFrame.Navigate(new VAT_Details_Page(Client));
                    break;
                case 6: // CIS details
                    CategoryFrame.Navigate(new CIS_Page(Client));
                    break;
                case 7: // paye detials
                    CategoryFrame.Navigate(new PAYE_Details_Page(Client));
                    break;
                case 8: // files
                    // CategoryFrame.Navigate(new Documents(Client.Documents, Client.Name));
                    break;
            }
        }

        private void CategoryFrame_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            CLientPageScroller.ScrollToVerticalOffset(CLientPageScroller.VerticalOffset - e.Delta);
        }
        // removes client
        private void RemoveClient_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to delete this client", "", MessageBoxButton.YesNo))
            {
                ClientManager.Delete(Client.UID);
                NavigationService.Navigate(new ClientOverview());
                return;
            }
        }

        private void CategoryFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
