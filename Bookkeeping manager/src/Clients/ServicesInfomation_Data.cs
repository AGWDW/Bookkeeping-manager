using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Clients
{

    public class ServicesInfomation_Data : ClientData
    {
        private TextBox selfAssess_TB;
        private TextBox p11d_TB;
        private Style normal;
        private Style readonly_;

        public ServicesInfomation_Data(string name) : base(name)
        {
        }

        public void Initalize(TextBox selfAssess_TB, TextBox p11d_TB, Style normal, Style readonly_)
        {
            this.selfAssess_TB = selfAssess_TB;
            this.p11d_TB = p11d_TB;
            this.normal = normal;
            this.readonly_ = readonly_;

            SelfAssessmentEnabled = !SelfAssessmentEnabled;
            SelfAssessmentEnabled = !SelfAssessmentEnabled;

            P11D_Enabled = !P11D_Enabled;
            P11D_Enabled = !P11D_Enabled;
        }
        public string Bookkeeping { get; set; }
        public string VAT_Return { get; set; }
        public string ManagmentReturns { get; set; }
        public string Payroll { get; set; }
        public string AutoEnrolment { get; set; }
        public string CIS { get; set; }
        public string BillPayments { get; set; }
        public string Invoicing { get; set; }
        public string AccountsPrep { get; set; }
        public string CT600_Return { get; set; }
        private bool selfAssessmentEnabled;
        public bool SelfAssessmentEnabled
        {
            get => selfAssessmentEnabled;
            set
            {
                if (value != selfAssessmentEnabled)
                {
                    selfAssessmentEnabled = value;
                    if (value)
                    {
                        selfAssess_TB.Style = normal;
                    }
                    else
                    {
                        selfAssess_TB.Style = readonly_;
                    }
                }
            }
        }
        public string SelfAssessment { get; set; }
        public string ConfirmationStatement { get; set; }
        private bool p11d_Enabled;
        public bool P11D_Enabled
        {
            get => p11d_Enabled;
            set
            {
                if (value != p11d_Enabled)
                {
                    p11d_Enabled = value;
                    if (value)
                    {
                        p11d_TB.Style = normal;
                    }
                    else
                    {
                        p11d_TB.Style = readonly_;
                    }
                }
            }
        }
        public string P11D { get; set; }
        public string CharitiesCommision { get; set; }
        public string ConsultationAdvice { get; set; }
        public string Software { get; set; }
        public string CompanySetup { get; set; }
    }
}
