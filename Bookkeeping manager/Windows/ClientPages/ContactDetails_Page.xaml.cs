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
    /// Interaction logic for ContactDetails.xaml
    /// </summary>
    public partial class ContactDetails_Page : Page
    {

        private List<ContactInfomation_Data> Contacts { get; set; }
        private ContactInfomation_Data Contact { get => Contacts[currentContact]; }

        private int currentContact;
        public ContactDetails_Page(Client client)
        {
            Contacts = client.Contacts;
            InitializeComponent();

            if (Contacts.Count == 0)
            {
                Contacts.Add(new ContactInfomation_Data(""));
            }
            ShowContact();

            PhotoID_Switch.IsChecked = Contact.PhotoID_Recieved;
            Address_Switch.IsChecked = Contact.Address_Recieved;
        }

        private void NextContactButton_Click(object sender, RoutedEventArgs e)
        {
            currentContact++;
            currentContact %= Contacts.Count;

            DataContext = Contacts[currentContact];
        }

        private void PrevContactButton_Click(object sender, RoutedEventArgs e)
        {
            currentContact--;
            currentContact %= Contacts.Count;

            DataContext = Contacts[currentContact];
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _ = 0;
        }

        private void ShowContact()
        {
            DataContext = Contact;
        }

        private void NewContactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactInfomation_Data contact = new ContactInfomation_Data("");
            Contacts.Add(contact);
            NextContactButton_Click(null, null);
        }

        private void RemoveContactButton_Click(object sender, RoutedEventArgs e)
        {
            if(currentContact != 0)
            {
                Contacts.RemoveAt(currentContact);
                PrevContactButton_Click(null, null);
            }

        }

        private void SwitchToggle_On(object sender, RoutedEventArgs e)
        {
            HorizontalToggleSwitch s = (HorizontalToggleSwitch)sender;
            string name = s.Name;
            switch (name)
            {
                case "PhotoID_Switch":
                    Contact.PhotoID_Recieved = true;
                    break;
                case "Address_Switch":
                    Contact.Address_Recieved = true;
                    break;
            }
        }
        private void SwitchToggle_Off(object sender, RoutedEventArgs e)
        {
            HorizontalToggleSwitch s = (HorizontalToggleSwitch)sender;
            string name = s.Name;
            switch (name)
            {
                case "PhotoID_Switch":
                    Contact.PhotoID_Recieved = false;
                    break;
                case "Address_Switch":
                    Contact.Address_Recieved = false;
                    break;
            }
        }
    }

}
