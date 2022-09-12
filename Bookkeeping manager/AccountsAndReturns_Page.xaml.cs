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
            AccountsAndReturns.Initialize(CH_Accounts_Current_TB, CT600_Current_TB, 
                HMRC_YearEnd_Current_TB, CH_Accounts_Future_TB, 
                CT600_Future_TB, HMRC_YearEnd_Future_TB);

            AccountsAndReturns.SetAPE_Dependents();

            DataContext = AccountsAndReturns;
        }
    }
}
