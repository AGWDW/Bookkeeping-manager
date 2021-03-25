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

namespace Bookkeeping_manager.Windows
{
    /// <summary>
    /// Interaction logic for ClientOverview.xaml
    /// </summary>
    public partial class ClientOverview : Page
    {
        public List<Client> Clients
        {
            get => DataHandler.AllClients;
            set => DataHandler.AllClients = value;
        }
        public ClientOverview()
        {
            InitializeComponent();
            DataContext = this;
            PopulateFullGrid(Clients);
        }

        private void AddClientToGrid(Client client, int rowIndex)
        {
            ClientsViewGrid.RowDefinitions.Add(new RowDefinition());
            TextBox nameBox = new TextBox
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Left,
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0, 0, 30, 0),
                Background = Brushes.Transparent
            };
            TextBox commentBox = new TextBox()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment = TextAlignment.Left,
                MaxLines = 5,
                //MinWidth = 100,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                BorderThickness = new Thickness(0),
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Background = new SolidColorBrush(Colors.AliceBlue)
            };

            Button vistitClient = new Button()
            {
                Content = "Details",
                Margin = new Thickness(0, 1, 10, 1),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(5, 0, 5, 0),
                Style = FindResource("ActionButton") as Style
            };
            vistitClient.Click += (o, e) => {
                NavigationService.Navigate(new ClientPage(client));
            };

            Rectangle rectangle = new Rectangle()
            {
                Fill = Brushes.RoyalBlue,
                Margin = new Thickness(0, -10, 10, 0),
                MinWidth = 10,
                MaxWidth = 10,
                Width = 10,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            DockPanel stack = new DockPanel()
            {
            };

            DockPanel.SetDock(vistitClient, Dock.Right);
            DockPanel.SetDock(nameBox, Dock.Left);
            DockPanel.SetDock(rectangle, Dock.Right);

            Binding nameBinding = new Binding("Name")
            {
                Source = client
            };
            Binding commentsBinding = new Binding("Comments")
            {
                Source = client
            };

            nameBox.SetBinding(TextBox.TextProperty, nameBinding);
            commentBox.SetBinding(TextBox.TextProperty, commentsBinding);

            stack.Children.Add(rectangle);
            stack.Children.Add(vistitClient);
            stack.Children.Add(nameBox);

            Grid.SetRow(stack, rowIndex);
            Grid.SetRow(commentBox, rowIndex);
            Grid.SetRow(vistitClient, rowIndex);

            Grid.SetColumn(stack, 0);
            Grid.SetColumn(nameBox, 0);
            Grid.SetColumn(commentBox, 1);

            ClientsViewGrid.Children.Add(stack);
            ClientsViewGrid.Children.Add(commentBox);
        }
        private void ClearGrid()
        {
            ClientsViewGrid.RowDefinitions.Clear();
            ClientsViewGrid.Children.Clear();
        }
        private void PopulateFullGrid(List<Client> clients)
        {
            int i = 0;
            foreach(Client client in clients)
            {
                if (client.Delete)
                    continue;
                AddClientToGrid(client, i++);
            }
        }

        private void NewClientButton_Click(object sender, RoutedEventArgs e)
        {
            Clients.Add(new Client("Empty Client"));
            if (ToggleAlphabetical.IsChecked.GetValueOrDefault())
                ToggleAlphabetical_Checked(null, null);
            else
                ToggleAlphabetical_Unchecked(null, null);
        }

        private void ToggleAlphabetical_Unchecked(object sender, RoutedEventArgs e)
        {
            Clients = Clients.OrderBy(o => o.Name).ToList();
            ClearGrid();
            PopulateFullGrid(Clients);
        }

        private void ToggleAlphabetical_Checked(object sender, RoutedEventArgs e)
        {
            Clients = Clients.OrderByDescending(o => o.Name).ToList();
            ClearGrid();
            PopulateFullGrid(Clients);
        }
    }
}
