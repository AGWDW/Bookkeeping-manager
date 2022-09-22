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
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading;
using Bookkeeping_manager.src;
using Bookkeeping_manager.src.Clients;
using Bookkeeping_manager.src.Tasks;

namespace Bookkeeping_manager.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer clock;
        private List<Event> Events
        {
            get => DataHandler.AllEvents;
            set => DataHandler.AllEvents = value;
        }

        public MainWindow()
        {
            // converter


            // Login
            /*UtilityWindows.LoginWindow login = new UtilityWindows.LoginWindow();
            login.ShowDialog();
            if (!login.Successful)
            {
                Close();
                return;
            }*/

            InitializeComponent();
            //DataHandler.Init(); // allows the database to be accessed
            DatabaseConnection.Connect();
            DatabaseConnection.PopulateUIDs();
            DatabaseConnection.PopulateTasks();
            DatabaseConnection.PopulateClients();

            // Converter
            // AM_Converter.AM_To_BM.Convert();
            // Converter



            DataContext = this;
            Clock.Content = $"{DateTime.Now:dddd dd/MM/yy} : {DateTime.Now:t}";
            clock = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            clock.Tick += (e, o) =>
            {
                DateTime t = DateTime.Now;
                Clock.Content = $"{t:dddd dd/MM/yy} : {t:t}";
            };
            clock.Start();

            HomeButton_Click(null, null);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Home(Events));
        }

        private void ClientOverviewButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ClientOverview());
        }

        private void CalenderViewMonth_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MonthView());
        }

        private void CalenderViewYear_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new YearView());
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _ = 0;
            //DataHandler.UploadToDatabase();
            // MessageBox.Show("Saved to database");
        }

        private void ReportsViewButton_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.Navigate(new Reports());
        }
    }
}