using System.Windows;
using System.Windows.Controls;
using ToggleSwitch;

namespace Bookkeeping_manager.Windows.ClientPages
{
    class ServicesInfomation_Data
    {
        private TextBox selfAssess_TB;
        private TextBox p11d_TB;
        private Style normal;
        private Style readonly_;

        public void Initalize(TextBox selfAssess_TB, TextBox p11d_TB, Style normal, Style readonly_)
        {
            this.selfAssess_TB = selfAssess_TB;
            this.p11d_TB = p11d_TB;
            this.normal = normal;
            this.readonly_ = readonly_;

            SelfAssessmentEnabled = true;
            SelfAssessmentEnabled = false;

            P11D_Enabled = true;
            P11D_Enabled = false;
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
                if(value != selfAssessmentEnabled)
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

    /// <summary>
    /// Interaction logic for ServicesInfomation_Page.xaml
    /// </summary>
    public partial class ServicesInfomation_Page : Page
    {
        private ServicesInfomation_Data Services { get; set; }
        public ServicesInfomation_Page()
        {
            Services = new ServicesInfomation_Data();
            InitializeComponent();
            Services.Initalize(SelfAssess_TB, P11D_TB, (Style)FindResource("RegularTB"), (Style)FindResource("ReadOnlyTB"));
            DataContext = Services;
        }

        private void Toggle_Checked(object sender, RoutedEventArgs e)
        {
            HorizontalToggleSwitch s = (HorizontalToggleSwitch)sender;
            string name = s.Name;
            switch (name)
            {
                case "SAToggle":
                    Services.SelfAssessmentEnabled = !Services.SelfAssessmentEnabled;
                    break;
                case "P11DToggle":
                    Services.P11D_Enabled = !Services.P11D_Enabled;
                    break;
            }
        }
    }
}
