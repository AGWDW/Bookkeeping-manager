using Bookkeeping_manager.src.Clients;
using System.Windows.Controls;

namespace Bookkeeping_manager
{
    /// <summary>
    /// Interaction logic for AccountsAndReturns_Page.xaml
    /// </summary>
    public partial class AccountsAndReturns_Page : Page
    {
        private AccountsAndReturns_Data AccountsAndReturns { get; set; }
        public AccountsAndReturns_Page(Client client)
        {
            AccountsAndReturns = client.AccountsAndReturns;
            InitializeComponent();
            DataContext = AccountsAndReturns;
        }
    }
}
