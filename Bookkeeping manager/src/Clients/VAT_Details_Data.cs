using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Clients
{
    public class VAT_Details_Data : ClientData
    {
        public VAT_Details_Data(string name) : base(name)
        {
        }
        public object VAT_Frequency { get; set; }
        public string VAT_PeriodEnd { get; set; }
        public string NextReturnDate { get; set; }
        public string ReccordsRecieved { get; set; }
        public string ProgressNotes { get; set; }
        public string VAT_Number { get; set; }
        public string VAT_Address { get; set; }
        public string DateOfRegistration { get; set; }
        public string EffectiveDate { get; set; }
        public string AppliedForMTD { get; set; }
        public bool MTD_Ready { get; set; }
        public bool DirectDebit { get; set; }
        public bool StandardScheme { get; set; }
        public bool CashAccountingScheme { get; set; }
        public bool FlatRate { get; set; }
        public string FlatRateCategory { get; set; }
        public string GeneralNotes { get; set; }
    }
}
