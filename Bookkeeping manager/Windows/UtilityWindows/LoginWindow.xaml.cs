using Bookkeeping_manager.Passwords;
using System.Windows;
using System.Windows.Controls;

namespace Bookkeeping_manager.Windows.UtilityWindows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public bool Successful { get; set; }
        private Account Account { get; set; }
        private const int MaxAttempts = 3;
        private int Attemps;
        public LoginWindow()
        {
            Successful = false;
            InitializeComponent();
            DataContext = this;
            Account = new Account()
            {
                UserName = "sfyorks",
                Password = "/lrh1g5moCQg5p1sQ05bcNM7P5ayOzrsydKo4/PRPB95k/TzOwHXELLnOl+zRI8B12X5zD/9UNByBrkkjDTXtcBdAAA="
            };
            Attemps = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(UserNameBox.Text.Trim()))
            {
                MessageBox.Show("Please enter valid values");
                PasswordBox.Password = "";
                return;
            }

            if (string.IsNullOrEmpty(PasswordBox.Password.Trim()) || PasswordBox.Password.Length > 15)
            {
                MessageBox.Show("Please enter valid values");
                PasswordBox.Password = "";
                return;
            }

            Account a = new Account()
            {
                UserName = UserNameBox.Text.Trim(),
                Password = PasswordBox.Password.Trim()
            };
            PasswordBox.Password = "";
            if (a != Account)
            {
                if (Attemps++ >= MaxAttempts)
                {
                    MessageBox.Show("No more attemps");
                    Close();
                    return;
                }
                MessageBox.Show("Invalid UserName or Password");
            }
            else
            {
                Successful = true;
                Close();
            }
        }
    }
}
