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
    /// Interaction logic for ContactDetails.xaml
    /// </summary>
    public partial class ContactDetails_Page : Page
    {

        List<ContactInfomation_Data> NewContacts;
        private ContactInfomation_Data Contact { get => NewContacts[currentContact]; }

        private int currentContact;
        public ContactDetails_Page(ContactDetials contacts)
        {
            NewContacts = new List<ContactInfomation_Data>();
            InitializeComponent();
            if (NewContacts.Count == 0)
            {
                NewContacts.Add(new ContactInfomation_Data());
            }
            ShowContact();
        }

        private void NextContactButton_Click(object sender, RoutedEventArgs e)
        {
            currentContact++;
            currentContact %= NewContacts.Count;

            DataContext = NewContacts[currentContact];
            // CurrentContact++;
            // if (Contacts.Size == CurrentContact)
            // {
            //     Contacts.Contacts.Add(new Contact(Contacts.Client, Contacts.Contacts.Count));
            // }
            // PopulateGrid(Contacts[CurrentContact]);
            // PrevContactButton.Visibility = Visibility.Visible;


        }

        private void PrevContactButton_Click(object sender, RoutedEventArgs e)
        {
            currentContact--;
            currentContact %= NewContacts.Count;

            DataContext = NewContacts[currentContact];
            // if (CurrentContact <= 0)
            // {
            //     return;
            // }
            // CurrentContact--;
            // PopulateGrid(Contacts[CurrentContact]);
            // if (CurrentContact == 0)
            //     PrevContactButton.Visibility = Visibility.Hidden;
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
            ContactInfomation_Data contact = new ContactInfomation_Data();
            NewContacts.Add(contact);
            NextContactButton_Click(null, null);
        }

        private void RemoveContactButton_Click(object sender, RoutedEventArgs e)
        {
            if(currentContact != 0)
            {
                NewContacts.RemoveAt(currentContact);
                PrevContactButton_Click(null, null);
            }

        }

        private void SwitchToggle(object sender, RoutedEventArgs e)
        {
            HorizontalToggleSwitch s = (HorizontalToggleSwitch)sender;
            string name = s.Name;
            switch (name)
            {
                case "PhotoID_Switch":
                    Contact.PhotoID_Recieved = !Contact.PhotoID_Recieved;
                    break;
                case "Address_Switch":
                    Contact.Address_Recieved = !Contact.Address_Recieved;
                    break;
            }
        }
    }
}
