using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Clients
{
    public class AccountantInfomation_Data : ClientData
    {
        public AccountantInfomation_Data(string name) : base(name)
        {
        }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Notes { get; set; }
    }
}
