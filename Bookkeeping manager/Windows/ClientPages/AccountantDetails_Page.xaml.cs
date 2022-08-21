using Bookkeeping_manager.src.Clients;
using System.Windows.Controls;

namespace Bookkeeping_manager.Windows.ClientPages
{
    /// <summary>
    /// Interaction logic for AccountDetails_Page.xaml
    /// </summary>
    public partial class AccountantDetails_Page : Page
    {
        private AccountantInfomation_Data accountant;
        public AccountantDetails_Page(Client client)
        {
            accountant = client.Accountant;
            InitializeComponent();
            DataContext = accountant;
        }
    }
}
