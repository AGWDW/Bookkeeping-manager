using System.Collections.Generic;

namespace Bookkeeping_manager.src.Clients
{
    public class ClientData
    {
        protected string parentName;
        public ClientData(string parentName)
        {
            this.parentName = parentName;
        }
        public virtual void ReName(string name)
        {
            parentName = name;
        }
    }
    public class Client
    {
        public int UID { get; set; }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if(name != value)
                {
                    name = value;
                    ClientInfomation.ReName(value);
                    foreach(var c in Contacts)
                    {
                        c.ReName(value);
                    }
                    Accountant.ReName(value);
                    ServiceInfomation.ReName(value);
                    VATDetails.ReName(value);
                    CISInfomation.ReName(value);
                    AccountsAndReturns.ReName(value);
                    PAYE_Details.ReName(value);
                }
            }
        }
        public string Comments { get; set; }
        public ClientInfomation_Data ClientInfomation { get; set; }
        public List<ContactInfomation_Data> Contacts { get; set; }
        public AccountantInfomation_Data Accountant { get; set; }
        public ServicesInfomation_Data ServiceInfomation { get; set; }
        public VAT_Details_Data VATDetails { get; set; }
        public CIS_Infomation_Data CISInfomation { get; set; }
        public AccountsAndReturns_Data AccountsAndReturns { get; set; }
        public PAYE_Details_Data PAYE_Details { get; set; }
        public Client()
        {
            ClientInfomation = new ClientInfomation_Data(Name);
            Contacts = new List<ContactInfomation_Data>() { new ContactInfomation_Data(Name) };
            Accountant = new AccountantInfomation_Data(Name);
            ServiceInfomation = new ServicesInfomation_Data(Name);
            AccountsAndReturns = new AccountsAndReturns_Data(Name);
            VATDetails = new VAT_Details_Data(Name);
            CISInfomation = new CIS_Infomation_Data(Name);
            PAYE_Details = new PAYE_Details_Data(Name);
        }
    }
}

