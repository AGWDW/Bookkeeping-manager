using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Clients
{

    public class PAYE_Details_Data : ClientData
    {
        private TextBox weeklyDateTB, twoWeeklyDataTB, monthlyDataTB;
        private Style normal, readonly_;
        private readonly string parentName;

        public void Initalize(TextBox weekly, TextBox twoWeekly, TextBox monthly, Style norm, Style ro)
        {
            weeklyDateTB = weekly;
            twoWeeklyDataTB = twoWeekly;
            monthlyDataTB = monthly;
            normal = norm;
            readonly_ = ro;

            Weekly_PayRollEnabled = !Weekly_PayRollEnabled;
            Weekly_PayRollEnabled = !Weekly_PayRollEnabled;

            TwoWeekly_PayRollEnabled = !TwoWeekly_PayRollEnabled;
            TwoWeekly_PayRollEnabled = !TwoWeekly_PayRollEnabled;

            Monthly_PayRollEnabled = !Monthly_PayRollEnabled;
            Monthly_PayRollEnabled = !Monthly_PayRollEnabled;
        }

        public PAYE_Details_Data(string name) : base(name)
        {
        }

        public string EmployerReference { get; set; }
        public string AccountsOfficeReference { get; set; }

        #region Payroll
        #region Weekly
        private bool weekly_PayrollEnabled;
        public bool Weekly_PayRollEnabled
        {
            get => weekly_PayrollEnabled;
            set
            {
                if (weekly_PayrollEnabled != value)
                {
                    weekly_PayrollEnabled = value;
                    if (value)
                    {
                        weeklyDateTB.Style = normal;
                    }
                    else
                    {
                        weeklyDateTB.Style = readonly_;
                    }
                }
            }
        }
        public string Weekly_Date { get; set; }
        public int Weekly_NumberOfEmployees { get; set; }
        public string Weekly_RTI_Deadline { get; set; }
        #endregion
        #region 2 Weekly
        private bool twoWeeklyDataEnabled;
        public bool TwoWeekly_PayRollEnabled
        {
            get => twoWeeklyDataEnabled;
            set
            {
                if (twoWeeklyDataEnabled != value)
                {
                    twoWeeklyDataEnabled = value;
                    if (value)
                    {
                        twoWeeklyDataTB.Style = normal;
                    }
                    else
                    {
                        twoWeeklyDataTB.Style = readonly_;
                    }
                }
            }
        }
        public string TwoWeekly_Date { get; set; }
        public int TwoWeekly_NumberOfEmployees { get; set; }
        public string TwoWeekly_RTI_Deadline { get; set; }
        #endregion
        #region Monthly
        private bool monthlyPayRollEnabled;
        public bool Monthly_PayRollEnabled
        {
            get => monthlyPayRollEnabled;
            set
            {
                if (monthlyPayRollEnabled != value)
                {
                    monthlyPayRollEnabled = value;
                    if (value)
                    {
                        monthlyDataTB.Style = normal;
                    }
                    else
                    {
                        monthlyDataTB.Style = readonly_;
                    }
                }
            }
        }
        public string Monthly_Date { get; set; }
        public int Monthly_NumberOfEmployees { get; set; }
        public string Monthly_RTI_Deadline { get; set; }
        #endregion
        #endregion

        public string PAYE_SchemeCeased { get; set; }
        public string PAYE_RecordsReceived { get; set; }
        public string GeneralNotes { get; set; }
        public string AutoEnrolmentRecordsRecieved { get; set; }
        public string AutoEnrolmentProgressNote { get; set; }
        public string AutoEnrolmentStaging { get; set; }
        public string PostponementDate { get; set; }
        public string PensionsRegulatorOptOutDate { get; set; }
        public string ReEnrolmentDate { get; set; }
        public string PensionProvider { get; set; }
        public string PensionID { get; set; }
        public string DeclarationOfComplianceDue { get; set; }
        public string DeclarationOfComplianceSubmission { get; set; }
        public string NextP11D_ReturnDue { get; set; }
        public string P11D_RecordsReceived { get; set; }
        public string P11D_ProgressNote { get; set; }

    }
}
