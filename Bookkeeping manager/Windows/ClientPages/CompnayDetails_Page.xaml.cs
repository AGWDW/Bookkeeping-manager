using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Clients;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ToggleSwitch;

namespace Bookkeeping_manager.Windows.ClientPages
{
    /// <summary>
    /// Interaction logic for ClientInfoViewer.xaml
    /// </summary>
    public partial class CompnayDetails_Page : Page
    {
        public ClientInfomation_Data ClientInfomation { get; set; }
        private Scripts.ClientDetailsBase Details { get; set; }
        public CompnayDetails_Page(ClientDetailsBase details)
        {
            InitializeComponent();
            ClientInfomation.Initalize(ConfirDateBox, FindResource("ReadOnlyTB") as Style, FindResource("RegularTB") as Style);
            DataContext = ClientInfomation;
            Details = details;
            //PopulateGrid();
        }
        private void PopulateGrid()
        {
            Dictionary<string, object> details = Details.ToDictionary<object>();
            int i = 0;
            foreach (KeyValuePair<string, object> pair in details)
            {
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
                switch (Details.GetControlTypes()[i])
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
                        bool property = Details.GetProperty<bool>(pair.Key);
                        (control as HorizontalToggleSwitch).IsChecked = property;
                        (control as HorizontalToggleSwitch).Checked += (o, e) =>
                        {
                            Details.SetProperty(pair.Key, true);
                        };
                        (control as HorizontalToggleSwitch).Unchecked += (o, e) =>
                        {
                            Details.SetProperty(pair.Key, false);
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
                            Details.SetProperty(pair.Key, list);
                        };
                        break;
                    case 8:
                        style = "ReadOnlyTBRed";
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
                        Source = Details
                    };
                    Binding widthBinding = new Binding("ActualWidth")
                    {
                        Source = label
                    };
                    control.SetBinding(WidthProperty, widthBinding);
                    control.SetBinding(TextBox.TextProperty, textBinding);
                }
                DetailsGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Auto
                });
                Binding rowHeight = new Binding("MaxHeight")
                {
                    Source = control
                };
                //if(style != "Switch")
                //DetailsGrid.RowDefinitions.Last().SetBinding(RowDefinition.HeightProperty, rowHeight);

                Grid.SetRow(label, i);
                Grid.SetRow(control, i++);

                Grid.SetColumn(label, style == "NAN" ? 1 : 0);
                Grid.SetColumn(control, 1);

                DetailsGrid.Children.Add(label);
                if (style != "NAN")
                    DetailsGrid.Children.Add(control);
                else
                    label.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _ = 0;
        }

        private void ConfirEnableToggle(object sender, RoutedEventArgs e)
        {
            ClientInfomation.ConfirmationEnabled = !ClientInfomation.ConfirmationEnabled;
        }
    }
}
