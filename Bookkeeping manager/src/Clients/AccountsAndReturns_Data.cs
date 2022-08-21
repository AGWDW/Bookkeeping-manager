using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Clients
{
    public class AccountsAndReturns_Data : ClientData
    {
        private TextBox chAccountsNextDue_Current;
        private TextBox ct600Due_Current;
        private TextBox hmrcYearEnd_Current;
        private TextBox chAccountsNextDue_Future;
        private TextBox ct600Due_Future;
        private TextBox hmrcYearEnd_Future;


        public AccountsAndReturns_Data(string name) : base(name)
        {
        }

        public void Initialize(TextBox chAccountsNextDue_Current, TextBox ct600Due_Current, TextBox hmrcYearEnd_Current,
            TextBox chAccountsNextDue_Future, TextBox ct600Due_Future, TextBox hmrcYearEnd_Future)
        {
            this.chAccountsNextDue_Current = chAccountsNextDue_Current;
            this.ct600Due_Current = ct600Due_Current;
            this.hmrcYearEnd_Current = hmrcYearEnd_Current;
            this.chAccountsNextDue_Future = chAccountsNextDue_Future;
            this.ct600Due_Future = ct600Due_Future;
            this.hmrcYearEnd_Future = hmrcYearEnd_Future;
        }

        public string AccountPeriodEnd { get; set; }
        public string CT_PaymentReference { get; set; }
        public string AccountsReccords { get; set; }
        public string AccountsProgressNotes { get; set; }
    }
}
