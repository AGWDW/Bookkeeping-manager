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
    /// Interaction logic for VAT_Details_Page.xaml
    /// </summary>
    public partial class VAT_Details_Page : Page
    {
        private VAT_Details_Data VAT_Details { get; set; }
        public VAT_Details_Page(Client client)
        {
            VAT_Details = client.VATDetails;
            InitializeComponent();
            DataContext = VAT_Details;
            MTD_Toggle.IsChecked = VAT_Details.MTD_Ready;
            DirectDebit_Toggle.IsChecked = VAT_Details.DirectDebit;
            Standard_Toggle.IsChecked = VAT_Details.StandardScheme;
            Cach_Toggle.IsChecked = VAT_Details.CashAccountingScheme;
            FlatRate_Toggle.IsChecked = VAT_Details.FlatRate;
        }

        private void SwitchToggle_On(object sender, RoutedEventArgs e)
        {
            switch(((Control)sender).Name)
            {
                case "MTD_Toggle":
                    VAT_Details.MTD_Ready = true;
                    break;
                case "DirectDebit_Toggle":
                    VAT_Details.DirectDebit = true;
                    break;
                case "Standard_Toggle":
                    VAT_Details.StandardScheme = true;
                    break;
                case "Cach_Toggle":
                    VAT_Details.CashAccountingScheme = true;
                    break;
                case "FlatRate_Toggle":
                    VAT_Details.FlatRate = true;
                    break;
            }
        }
        private void SwitchToggle_Off(object sender, RoutedEventArgs e)
        {
            switch (((Control)sender).Name)
            {
                case "MTD_Toggle":
                    VAT_Details.MTD_Ready = false;
                    break;
                case "DirectDebit_Toggle":
                    VAT_Details.DirectDebit = false;
                    break;
                case "Standard_Toggle":
                    VAT_Details.StandardScheme = false;
                    break;
                case "Cach_Toggle":
                    VAT_Details.CashAccountingScheme = false;
                    break;
                case "FlatRate_Toggle":
                    VAT_Details.FlatRate = false;
                    break;
            }
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            _ = 0;
        }
    }
}
