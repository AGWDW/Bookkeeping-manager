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
using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.Windows.ClientPages;
using ClientPages = Bookkeeping_manager.Windows.ClientPages;

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
                Margin = new Thickness(0,0,0,22),
                Background = Brushes.Transparent
            };
            CategoryFrame.Navigate(label);

            foreach(string c in Client.Categories)
            {
                ListViewItem item = new ListViewItem()
                {
                    Content = " \u2022 " + c
                };
                ClientCategories.Items.Add(item);
            }
        }

        private void ClientCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ClientCategories.SelectedIndex)
            {
                case 0: // compnay detatils
                    CategoryFrame.Navigate(new ClientInfoViewer(Client.CompanyDetails));
                    break;
                case 1: // contact details
                    CategoryFrame.Navigate(new ContactDetails(Client.ContactDetials));
                    break;
                case 2: // accountant
                    CategoryFrame.Navigate(new ClientInfoViewer(Client.Accountant));
                    break;
                case 3: // services
                    CategoryFrame.Navigate(new ClientInfoViewer(Client.Services));
                    break;
                case 4: // accounts and returns
                    CategoryFrame.Navigate(new ClientInfoViewer(Client.AccountsReturns));
                    break;
                case 5: // vat details
                    CategoryFrame.Navigate(new ClientInfoViewer(Client.VATDetails));
                    break;
                case 6: // CIS details
                    CategoryFrame.Navigate(new ClientInfoViewer(Client.CISDetails));
                    break;
                case 7: // paye detials
                    CategoryFrame.Navigate(new ClientPages.PAYEDetails(Client.PAYEDetails));
                    break;
                case 8: // files
                    CategoryFrame.Navigate(new ClientPages.Documents(Client.Documents, Client.Name));
                    break;
            }
        }

        private void CategoryFrame_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            CLientPageScroller.ScrollToVerticalOffset(CLientPageScroller.VerticalOffset - e.Delta);
        }
        // removes client
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to delete this client", "", MessageBoxButton.YesNo))
            {
                DataHandler.AllEvents.ForEach(ev => ev.Delete = ev.DisplayName.Contains($"({Client.Name})"));
                Client.Delete = true;
                NavigationService.Navigate(new ClientOverview());
                return;
            }
            Client.Delete = false;
        }
    }
}
