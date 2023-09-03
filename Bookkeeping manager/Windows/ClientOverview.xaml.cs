using Bookkeeping_manager.src.Clients;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookkeeping_manager.Windows
{
    /// <summary>
    /// Interaction logic for ClientOverview.xaml
    /// </summary>
    public partial class ClientOverview : Page
    {
        public ClientOverview()
        {
            InitializeComponent();
            DataContext = this;
            PopulateWithClients();
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
                Background = Brushes.Transparent,
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
            vistitClient.Click += (o, e) =>
            {
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

            nameBox.TextChanged += (o, e) =>
            {
                // client.Changed = true;
            };

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
        private void PopulateWithClients()
        {
            int i = 0;
            foreach (Client client in ClientManager.AllClients)
            {
                AddClientToGrid(client, i++);
            }
        }

        private void NewClientButton_Click(object sender, RoutedEventArgs e)
        {
            Client c = new Client()
            {
                Name = "Default Name"
            };
            ClientManager.AddClient(c, out int _);

            ClearGrid();
            PopulateWithClients();

            if (ToggleAlphabetical.IsChecked.GetValueOrDefault())
            {
                ToggleAlphabetical_Checked(null, null);
            }
            else
            {
                ToggleAlphabetical_Unchecked(null, null);
            }
        }

        private void ToggleAlphabetical_Unchecked(object sender, RoutedEventArgs e)
        {
            ClientManager.AllClients = ClientManager.AllClients.OrderBy(o => o.Name).ToList();
            ClearGrid();
            PopulateWithClients();
        }

        private void ToggleAlphabetical_Checked(object sender, RoutedEventArgs e)
        {
            ClientManager.AllClients = ClientManager.AllClients.OrderByDescending(o => o.Name).ToList();
            ClearGrid();
            PopulateWithClients();
        }
    }
}
