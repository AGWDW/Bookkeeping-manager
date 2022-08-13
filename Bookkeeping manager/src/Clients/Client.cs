using System.Collections.Generic;

namespace Bookkeeping_manager.src.Clients
{
    internal class Client
    {
        public int UID { get; set; }
        public string Name { get; set; }
        public ClientInfomation_Data ClientInfomation { get; set; }
        public List<ContactInfomation_Data> Contacts { get; set; }
        public Client()
        {
            ClientInfomation = new ClientInfomation_Data(Name);
            Contacts = new List<ContactInfomation_Data>() { new ContactInfomation_Data() };
        }
    }
}

