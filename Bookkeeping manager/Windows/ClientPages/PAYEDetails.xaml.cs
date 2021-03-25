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

namespace Bookkeeping_manager.Windows.ClientPages
{
    /// <summary>
    /// Interaction logic for PAYEDetails.xaml
    /// </summary>
    public partial class PAYEDetails : Page
    {
        Scripts.PAYEDetails Details { get; set; }
        public PAYEDetails(Scripts.PAYEDetails details)
        {
            InitializeComponent();
            DataContext = this;
            Details = details;
            CreateContent();
        }
        private void CreateContent()
        {
            Dictionary<string, object> data = Details.ToDictionary();
            int i = 0;
            foreach (var pair in data)
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
                string style = "SmallTB";
                switch (Details.GetControlTypes()[i])
                {
                    case 0:
                        break;
                    case 1:
                        style = "RegularTB";
                        break;
                    case 2:
                        style = "LargeTB";
                        break;
                    case 3:
                        style = "NAN";
                        break;
                    case 4:
                        style = "ReadOnlyTB";
                        break;
                    case 8:
                        style = "Page";
                        control = new Frame();
                        (control as Frame).Navigate(new PayRollPage(Details.PayRolls, Details.Client));
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
                DetailsGrid.RowDefinitions.Add(new RowDefinition());
                Binding rowHeight = new Binding("MaxHeight")
                {
                    Source = control
                };
                if (style != "Switch")
                    DetailsGrid.RowDefinitions.Last().SetBinding(RowDefinition.HeightProperty, rowHeight);

                Grid.SetRow(label, i);
                Grid.SetRow(control, i++);

                Grid.SetColumn(label, style != "NAN" ? 0 : 1);
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
    }
}
