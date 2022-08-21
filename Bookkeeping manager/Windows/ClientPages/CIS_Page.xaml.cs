using Bookkeeping_manager.src.Clients;
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

namespace Bookkeeping_manager.Windows.ClientPages
{
    /// <summary>
    /// Interaction logic for CIS_Page.xaml
    /// </summary>
    public partial class CIS_Page : Page
    {
        private readonly CIS_Infomation_Data cisInfo;
        public CIS_Page(Client client)
        {
            cisInfo = client.CISInfomation;
            InitializeComponent();

            Withheld_Toggle.IsChecked = cisInfo.WithheldEnabled;
            Suffered_Toggle.IsChecked = cisInfo.SufferedEnabled;

            cisInfo.Initalize(Withheld_TB, Suffered_TB, (Style)FindResource("RegularTB"), (Style)FindResource("ReadOnlyTB"));
            DataContext = cisInfo;
            
        }

        private void EnabledToggle_On(object sender, RoutedEventArgs e)
        {
            switch(((Control)sender).Name)
            {
                case "Withheld_Toggle":
                    cisInfo.WithheldEnabled = true;
                    break;
                case "Suffered_Toggle":
                    cisInfo.SufferedEnabled= true;
                    break;
            }
        }
        private void EnabledToggle_Off(object sender, RoutedEventArgs e)
        {
            switch (((Control)sender).Name)
            {
                case "Withheld_Toggle":
                    cisInfo.WithheldEnabled = false;
                    break;
                case "Suffered_Toggle":
                    cisInfo.SufferedEnabled = false;
                    break;
            }
        }
    }
}
