using System.Windows.Controls;

namespace Bookkeeping_manager.Windows.ClientPages
{
    class AccountantInfomation_Data
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Notes { get; set; }
    }
    /// <summary>
    /// Interaction logic for AccountDetails_Page.xaml
    /// </summary>
    public partial class AccountantDetails_Page : Page
    {
        private AccountantInfomation_Data accountant;
        public AccountantDetails_Page()
        {
            accountant = new AccountantInfomation_Data();
            InitializeComponent();
            DataContext = accountant;
        }
    }
}
