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
using System.Windows.Shapes;
using Bookkeeping_manager.Passwords;

namespace Bookkeeping_manager.Windows.UtilityWindows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public bool Successful { get; set; }
        Account Account { get; set; }
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
                Password = "gb59hp2DxbUlQ/CjLzMT4DHvxC02wIq1MIkoZvFrHA8mJpsKG/nlSAxzhIzZ5sH5mY+dqDL/lOUQte66jfdt38BdAAA="
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

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Button_Click(null, null);
            }
        }
    }
}
