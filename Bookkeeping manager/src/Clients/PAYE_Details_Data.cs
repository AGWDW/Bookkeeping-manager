using Bookkeeping_manager.src.Tasks;
using Bookkeeping_manager.Scripts;
using System.Windows;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Clients
{

    public class PAYE_Details_Data : ClientData
    {
        private TextBox weeklyDateTB, twoWeeklyDataTB, monthlyDataTB;
        private Style normal, readonly_;

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
        private int weeklyDue_UML, weeklyPrepare_UML, 
            twoWeeklyDue_UID, twoWeeklyPrepare_UID,
            monthlyDue_UID, monthlyPrepare_UID;
        public override void ReName(string name)
        {
            TaskManager.RenameTask(weeklyDue_UML, parentName, name);
            TaskManager.RenameTask(weeklyPrepare_UML, parentName, name);

            TaskManager.RenameTask(twoWeeklyDue_UID, parentName, name);
            TaskManager.RenameTask(twoWeeklyPrepare_UID, parentName, name);

            TaskManager.RenameTask(monthlyDue_UID, parentName, name);
            TaskManager.RenameTask(monthlyPrepare_UID, parentName, name);
            base.ReName(name);
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
                        string prev = Weekly_Date;
                        Weekly_Date = "";
                        Weekly_Date = prev;
                    }
                    else
                    {
                        weeklyDateTB.Style = readonly_;
                        TaskManager.DeleteTask(weeklyDue_UML);
                        TaskManager.DeleteTask(weeklyPrepare_UML);
                    }
                }
            }
        }

        private string weeklyDue;
        public string Weekly_Date
        {
            get => weeklyDue;
            set
            {
                if (weeklyDue != value)
                {
                    weeklyDue = value;
                    if (string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(weeklyDue_UML);
                        TaskManager.DeleteTask(weeklyPrepare_UML);
                    }
                    else
                    {
                        ReacuringTask task = (ReacuringTask)
                            TaskManager.GetOrCreate(weeklyDue_UML, TaskType.Reacuring, out weeklyDue_UML);
                        task.Name = $"Weekly Payroll due for {parentName}";
                        task.Offset = Constants.WEEK;
                        task.SetDate(weeklyDue.ToDate());


                        ReacuringTask task2 = (ReacuringTask)
                            TaskManager.GetOrCreate(weeklyPrepare_UML, TaskType.Reacuring, out weeklyPrepare_UML);
                        task2.Name = $"Prepare Weekly Payroll due for {parentName}";
                        task2.Offset = Constants.WEEK;
                        task2.SetDate(weeklyDue.ToDate().AddDays(-2));

                        task.TryAddChild(task2);
                    }
                }
            }
        }

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
                        string prev = TwoWeekly_Date;
                        TwoWeekly_Date = "";
                        TwoWeekly_Date = prev;
                    }
                    else
                    {
                        twoWeeklyDataTB.Style = readonly_;
                        TaskManager.DeleteTask(twoWeeklyDue_UID);
                        TaskManager.DeleteTask(twoWeeklyPrepare_UID);
                    }
                }
            }
        }
        string twoWeeklyDue;
        public string TwoWeekly_Date
        {
            get => twoWeeklyDue;
            set
            {
                if (twoWeeklyDue != value)
                {
                    twoWeeklyDue = value;
                    if (string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(twoWeeklyDue_UID);
                        TaskManager.DeleteTask(twoWeeklyPrepare_UID);
                    }
                    else
                    {
                        ReacuringTask task = (ReacuringTask)
                            TaskManager.GetOrCreate(twoWeeklyDue_UID, TaskType.Reacuring, out twoWeeklyDue_UID);
                        task.Name = $"2 Weekly Payroll due for {parentName}";
                        task.Offset = Constants.WEEK * 2;
                        task.SetDate(weeklyDue.ToDate());


                        ReacuringTask task2 = (ReacuringTask)
                            TaskManager.GetOrCreate(twoWeeklyPrepare_UID, TaskType.Reacuring, out twoWeeklyPrepare_UID);
                        task2.Name = $"Prepare 2 Weekly Payroll due for {parentName}";
                        task2.Offset = Constants.WEEK * 2;
                        task2.SetDate(twoWeeklyDue.ToDate().AddDays(-2));

                        task.TryAddChild(task2);
                    }
                }
            }
        }
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
                        string prev = Monthly_Date;
                        Monthly_Date = "";
                        Monthly_Date = prev;
                    }
                    else
                    {
                        monthlyDataTB.Style = readonly_;
                        TaskManager.DeleteTask(monthlyDue_UID);
                        TaskManager.DeleteTask(monthlyPrepare_UID);
                    }
                }
            }
        }
        public string Monthly_Date
        {
            get => twoWeeklyDue;
            set
            {
                if (twoWeeklyDue != value)
                {
                    twoWeeklyDue = value;
                    if (string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(monthlyDue_UID);
                        TaskManager.DeleteTask(monthlyPrepare_UID);
                    }
                    else
                    {
                        ReacuringTask task = (ReacuringTask)
                            TaskManager.GetOrCreate(monthlyDue_UID, TaskType.Reacuring, out monthlyDue_UID);
                        task.Name = $"Monthly Payroll due for {parentName}";
                        task.Offset = Constants.MONTH;
                        task.SetDate(weeklyDue.ToDate());


                        ReacuringTask task2 = (ReacuringTask)
                            TaskManager.GetOrCreate(monthlyPrepare_UID, TaskType.Reacuring, out monthlyPrepare_UID);
                        task2.Name = $"Prepare Monthly Payroll due for {parentName}";
                        task2.Offset = Constants.MONTH;
                        task2.SetDate(twoWeeklyDue.ToDate().AddDays(-2));

                        task.TryAddChild(task2);
                    }
                }
            }
        }
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
