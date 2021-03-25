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
using ToggleSwitch;
using Scripts = Bookkeeping_manager.Scripts;

namespace Bookkeeping_manager.Windows.ClientPages
{
    /// <summary>
    /// Interaction logic for ContactDetails.xaml
    /// </summary>
    public partial class ContactDetails : Page
    {
        public ContactDetials Contacts { get; set; }
        private int CurrentContact;
        public Contact ActiveContact;
        public ContactDetails(ContactDetials contacts)
        {
            InitializeComponent();
            DataContext = this;
            PrevContactButton.Visibility = Visibility.Hidden;
            CurrentContact = 0;
            Contacts = contacts;
            ActiveContact = Contacts[0];
            SetUpGrid();
        }

        public void SetUpGrid()
        {
            Binding nameBinding = new Binding("Name")
            {
                Source = ActiveContact
            };
            BindingOperations.ClearBinding(ContactName, TextBox.TextProperty);
            ContactName.SetBinding(TextBox.TextProperty, nameBinding);
            Dictionary<string, object> details = ActiveContact.ToDictionary<object>();
            int i = 1;
            foreach (KeyValuePair<string, object> pair in details)
            {
                if (pair.Key == "Name") continue;
                string style = "RegularTB";
                switch (ActiveContact.GetControlTypes()[i-1])
                {
                    case 0:
                        style = "RegularTB";
                        break;
                    case 3:
                        style = "Switch";
                        break;
                    case 2:
                        style = "LargeTB";
                        break;
                }
                ContactDetailsGrid.RowDefinitions.Add(new RowDefinition());
                Label label = new Label()
                {
                    Content = pair.Key.Split(true),
                    Height = double.NaN,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Background = Brushes.Transparent
                };

                Control control = new Control();
                if (style != "Switch")
                {
                    control = new TextBox()
                    {
                        Style = FindResource(style) as Style,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    Binding textBinding = new Binding(pair.Key)
                    {
                        Source = ActiveContact
                    };
                    Binding widthBinding = new Binding("ActualWidth")
                    {
                        Source = label
                    };
                    control.SetBinding(WidthProperty, widthBinding);
                    control.SetBinding(TextBox.TextProperty, textBinding);
                }
                else
                {
                    control = new HorizontalToggleSwitch()
                    {
                        CheckedContent = "Y",
                        UncheckedContent = "N",

                        CheckedBackground = Brushes.Orange,
                        UncheckedBackground = Brushes.LightGray,
                        Height = 35,
                        Width = 100
                    };
                    if (pair.Key == "PhotoIDVerified")
                    {
                        (control as HorizontalToggleSwitch).IsChecked = bool.Parse(ActiveContact.PhotoIDReadReceived == "" ? "false" : ActiveContact.PhotoIDReadReceived);
                    }
                    else
                    {
                        (control as HorizontalToggleSwitch).IsChecked = bool.Parse(ActiveContact.AddressReadReceived == "" ? "false" : ActiveContact.AddressReadReceived);
                    }
                    (control as HorizontalToggleSwitch).Checked += (o, e) =>
                    {
                        if(pair.Key == "PhotoIDVerified")
                        {
                            ActiveContact.PhotoIDReadReceived = "true";
                        }
                        else
                        {
                            ActiveContact.AddressReadReceived = "true";
                        }
                    };
                    (control as HorizontalToggleSwitch).Unchecked += (o, e) =>
                    {
                        if (pair.Key == "PhotoIDVerified")
                        {
                            ActiveContact.PhotoIDReadReceived = "false";
                        }
                        else
                        {
                            ActiveContact.AddressReadReceived = "false";
                        }
                    };
                }

                Binding rowHeight = new Binding("MaxHeight")
                {
                    Source = control
                };
                Grid.SetRow(label, i);
                Grid.SetRow(control, i++);

                Grid.SetColumn(label, 0);
                Grid.SetColumn(control, 1);

                ContactDetailsGrid.Children.Add(label);
                ContactDetailsGrid.Children.Add(control);
            }
        }

        private void PopulateGrid(Contact contact)
        {
            ActiveContact = contact;
            ContactDetailsGrid.Children.RemoveRange(4, ContactDetailsGrid.Children.Count - 4);
            SetUpGrid();
        }

        private void NextContactButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentContact++;
            if (Contacts.Size == CurrentContact)
            {
                Contacts.Contacts.Add(new Contact(Contacts.Client, Contacts.Contacts.Count));
            }
            PopulateGrid(Contacts[CurrentContact]);
            PrevContactButton.Visibility = Visibility.Visible;

        }

        private void PrevContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentContact <= 0)
            {
                return;
            }
            CurrentContact--;
            PopulateGrid(Contacts[CurrentContact]);
            if(CurrentContact == 0)
                PrevContactButton.Visibility = Visibility.Hidden;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _ = 0;
        }
    }
}
