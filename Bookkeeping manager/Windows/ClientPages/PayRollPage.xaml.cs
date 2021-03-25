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
using ToggleSwitch;
using Bookkeeping_manager.Scripts;
namespace Bookkeeping_manager.Windows.ClientPages
{
    /// <summary>
    /// Interaction logic for PayRollPage.xaml
    /// </summary>
    public partial class PayRollPage : Page
    {
        List<Scripts.PayRoll> Details { get; set; }
        public Scripts.PayRoll ActivePayRoll { get; set; }
        private int CurrentPayRoll;
        private Client Client;
        public PayRollPage(List<Scripts.PayRoll> details, Client client)
        {
            InitializeComponent();
            DataContext = this;
            Details = details;
            CurrentPayRoll = 0;
            if (Details.Count > 0)
            {
                ActivePayRoll = Details[CurrentPayRoll];
            }
            PopulateGrid(ActivePayRoll);
            Client = client;
        }

        private void PopulateGrid(PayRoll payroll)
        {
            ActivePayRoll = payroll;
            DetailsGrid.Children.RemoveRange(5, DetailsGrid.Children.Count - 5);
            try
            {
                DetailsGrid.RowDefinitions.RemoveRange(1, DetailsGrid.RowDefinitions.Count - 1);
            }
            catch
            {

            }

            PrevButton.Visibility = Visibility.Visible;
            NextButton.Visibility = Visibility.Visible;
            if (CurrentPayRoll == 0)
            {
                PrevButton.Visibility = Visibility.Hidden;
            }
            if (CurrentPayRoll >= Details.Count - 1)
            {
                NextButton.Visibility = Visibility.Hidden;
            }
            if (payroll is null)
            {
                PayRollType.Content = "N/A";
                return;
            }
            PayRollType.Content = ActivePayRoll.Type;
            PopulateGrid();
        }
        void PopulateGrid()
        {
            Dictionary<string, object> details = ActivePayRoll.ToDictionary<object>();
            int i = 1;
            foreach (KeyValuePair<string, object> pair in details)
            {
                if (pair.Key == "Type")
                    continue;
                Label label = new Label()
                {
                    Content = pair.Key.Split(true),
                    Height = double.NaN,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Background = Brushes.Transparent
                };
                Control control = new Control();
                string style = "";
                switch (ActivePayRoll.GetControlTypes()[i - 1])
                {
                    case 0:
                        style = "SmallTB";
                        break;
                    case 1:
                        style = "RegularTB";
                        break;
                    case 2:
                        style = "LargeTB";
                        break;
                    case 4:
                        style = "NAN";
                        break;
                    case 5:
                        style = "ReadOnlyTB";
                        break;
                    case 6:
                        style = "Switch";
                        control = new HorizontalToggleSwitch();
                        bool property = ActivePayRoll.GetProperty<bool>(pair.Key);
                        (control as HorizontalToggleSwitch).IsChecked = property;
                        (control as HorizontalToggleSwitch).Checked += (o, e) =>
                        {
                            ActivePayRoll.SetProperty(pair.Key, true);
                        };
                        (control as HorizontalToggleSwitch).Unchecked += (o, e) =>
                        {
                            ActivePayRoll.SetProperty(pair.Key, false);
                        };
                        break;
                    case 7:
                        control = new ComboBox();
                        var c = (control as ComboBox);
                        string[] list = new string[(pair.Value as string[]).Length];
                        (pair.Value as string[]).CopyTo(list, 0);
                        for (short j = 1; j < list.Length; j++)
                        {
                            c.Items.Add(list[j]);
                        }
                        c.SelectedIndex = int.Parse(list[0]);
                        c.SelectionChanged += (o, e) =>
                        {
                            list[0] = c.SelectedIndex.ToString();
                            ActivePayRoll.SetProperty(pair.Key, list);
                        };
                        break;
                }
                if (style.Contains("TB")) // is a TextBox
                {
                    control = new TextBox()
                    {
                        Style = FindResource(style) as Style,
                    };
                    Binding textBinding = new Binding(pair.Key)
                    {
                        Source = ActivePayRoll
                    };
                    Binding widthBinding = new Binding("ActualWidth")
                    {
                        Source = label
                    };
                    control.SetBinding(WidthProperty, widthBinding);
                    BindingOperations.ClearBinding(control, TextBox.TextProperty);
                    control.SetBinding(TextBox.TextProperty, textBinding);
                }
                DetailsGrid.RowDefinitions.Add(new RowDefinition());
                Binding rowHeight = new Binding("MaxHeight")
                {
                    Source = control
                };
                if (style != "Switch")
                    DetailsGrid.RowDefinitions.Last().SetBinding(RowDefinition.HeightProperty, rowHeight);

                Grid.SetRow(label, i);
                Grid.SetRow(control, i++);

                Grid.SetColumn(label, style == "NAN" ? 1 : 0);
                Grid.SetColumn(control, 1);

                DetailsGrid.Children.Add(label);
                if (style != "NAN")
                    DetailsGrid.Children.Add(control);
            }
        }

        public void PrevButton_Click(object sender, RoutedEventArgs args)
        {
            if (CurrentPayRoll <= 0)
            {
                return;
            }
            CurrentPayRoll--;
            PopulateGrid(Details[CurrentPayRoll]);
            if (CurrentPayRoll == 0)
                PrevButton.Visibility = Visibility.Hidden;
            
        }

        public void NextButton_Click(object sender, RoutedEventArgs args)
        {
            CurrentPayRoll++;
            if (Details.Count == CurrentPayRoll - 1)
            {
                NextButton.Visibility = Visibility.Hidden;
            }
            PopulateGrid(Details[CurrentPayRoll]);
            PrevButton.Visibility = Visibility.Visible;
        }

        private void RemovePayRoll_Click(object sender, RoutedEventArgs e)
        {
            // string[] names = new string[]
            // {
            //     $"Payroll {ActivePayRoll.Type} ({this.name})",  $"Prepare Payroll {ActivePayRoll.Type} ({this.name})"
            // };
            // DataHandler.AllEvents.RemoveAll(e_ => names.Contains(e_.DisplayName)); // removes all ape tasks
            try
            {
                ActivePayRoll.Date = ""; // calls the set which then deletes the tasks
                Details.RemoveAt(CurrentPayRoll);
            }
            catch
            {
                return;
            }
            CurrentPayRoll--;
            if(CurrentPayRoll < 0)
            {
                CurrentPayRoll++;
            }


            if (CurrentPayRoll >= Details.Count)
            {
                CurrentPayRoll = 0;
                PopulateGrid(null);
            }
            else
            {
                PopulateGrid(Details[CurrentPayRoll]);
            }
        }

        private void AddPayRoll_Click(object sender, RoutedEventArgs e)
        {
            UtilityWindows.PayrollTypeSelector typeSelector = new UtilityWindows.PayrollTypeSelector();
            typeSelector.ShowDialog();
            string type = typeSelector.Selected;
            if(type == "")
            {
                return;
            }
            Details.Add(new PayRoll(Client)
            {
                Type = type
            });
            CurrentPayRoll = Details.Count - 1;
            PopulateGrid(Details.Last());
        }
    }
}
