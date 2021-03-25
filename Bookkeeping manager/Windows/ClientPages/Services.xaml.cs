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
    /// Interaction logic for Services.xaml
    /// </summary>
    public partial class Services : Page
    {
        private Scripts.Services Details { get; set; }
        public Services(Scripts.Services details)
        {
            InitializeComponent();
            DataContext = this;
            Details = details;
            PopulateGrid();
        }
        private void PopulateGrid()
        {
            Dictionary<string, object> details = Details.ToDictionary();
            int i = 0;
            foreach (KeyValuePair<string, object> pair in details)
            {
                string tbStyle = "RegularTB";
                switch (Details.GetControlTypes()[i])
                {
                    case 0:
                        tbStyle = "SmallTB";
                        break;
                    case 1:
                        tbStyle = "RegularTB";
                        break;
                    case 2:
                        tbStyle = "LargeTB";
                        break;
                }
                ServicesGrid.RowDefinitions.Add(new RowDefinition());
                Label label = new Label()
                {
                    Content = pair.Key.Split(true),
                    Height = double.NaN,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Background = Brushes.Transparent
                };
                TextBox textBox = new TextBox()
                {
                    Style = FindResource(tbStyle) as Style,
                };

                Binding textBinding = new Binding(pair.Key)
                {
                    Source = Details
                };
                Binding rowHeight = new Binding("MaxHeight")
                {
                    Source = textBox
                };
                Binding widthBinding = new Binding("ActualWidth")
                {
                    Source = label
                };
                textBox.SetBinding(TextBox.TextProperty, textBinding);
                textBox.SetBinding(TextBox.WidthProperty, widthBinding);
                ServicesGrid.RowDefinitions.Last().SetBinding(RowDefinition.HeightProperty, rowHeight);
                Grid.SetRow(label, i);
                Grid.SetRow(textBox, i++);

                Grid.SetColumn(label, 0);
                Grid.SetColumn(textBox, 1);

                ServicesGrid.Children.Add(label);
                ServicesGrid.Children.Add(textBox);
            }
        }
    }
}
