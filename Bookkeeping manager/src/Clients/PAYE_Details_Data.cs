using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Clients
{

    public class PAYE_Details_Data : ClientData
    {
        private TextBox weeklyDateTB, twoWeeklyDataTB, monthlyDataTB;
        private Style normal, readonly_;
        public PAYE_Details_Data(string name) : base(name)
        {
            Monthly_SelectedDateType = 0;
            weeklyDue = "";
            twoWeeklyDue = "";
            monthlyDue = "";
        }
        public void Initalize(TextBox weekly, TextBox twoWeekly, TextBox monthly, Style norm, Style ro)
        {
            weeklyDateTB = weekly;
            twoWeeklyDataTB = twoWeekly;
            monthlyDataTB = monthly;
            normal = norm;
            readonly_ = ro;

            if (Weekly_PayRollEnabled)
            {
                if (weeklyDateTB != null)
                {
                    weeklyDateTB.Style = normal;
                }
            }
            else
            {
                if (weeklyDateTB != null)
                {
                    weeklyDateTB.Style = readonly_;
                }
            }

            if (TwoWeekly_PayRollEnabled)
            {
                if (twoWeeklyDataTB != null)
                {
                    twoWeeklyDataTB.Style = normal;
                }
            }
            else
            {
                if (twoWeeklyDataTB != null)
                {
                    twoWeeklyDataTB.Style = readonly_;
                }
            }

            if (Monthly_PayRollEnabled)
            {
                if (monthlyDataTB != null)
                {
                    monthlyDataTB.Style = normal;
                }
            }
            else
            {
                if (monthlyDataTB != null)
                {
                    monthlyDataTB.Style = readonly_;
                }
            }
        }
        public int weeklyDue_UID { get; set; }
        public int weeklyPrepare_UID { get; set; }
        public int twoWeeklyDue_UID { get; set; }
        public int twoWeeklyPrepare_UID { get; set; }
        public int monthlyDue_UID { get; set; }
        public int monthlyPrepare_UID { get; set; }
        public override void UpdateTasks()
        {
            TaskManager.UpdateValue(weeklyDue_UID, ref weeklyDue);

            TaskManager.UpdateValue(twoWeeklyDue_UID, ref weeklyDue);

            TaskManager.UpdateValue(monthlyDue_UID, ref weeklyDue);
            base.UpdateTasks();
        }
        public override void ReName(string name)
        {
            TaskManager.RenameTask(weeklyDue_UID, parentName, name);
            TaskManager.RenameTask(weeklyPrepare_UID, parentName, name);

            TaskManager.RenameTask(twoWeeklyDue_UID, parentName, name);
            TaskManager.RenameTask(twoWeeklyPrepare_UID, parentName, name);

            TaskManager.RenameTask(monthlyDue_UID, parentName, name);
            TaskManager.RenameTask(monthlyPrepare_UID, parentName, name);
            base.ReName(name);
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
                        if (weeklyDateTB != null)
                        {
                            weeklyDateTB.Style = normal;
                        }
                        Weekly_Date = RESET_CHAR;
                    }
                    else
                    {
                        if (weeklyDateTB != null)
                        {
                            weeklyDateTB.Style = readonly_;
                        }
                        TaskManager.DeleteTask(weeklyDue_UID);
                        TaskManager.DeleteTask(weeklyPrepare_UID);
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
                    if (string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(weeklyDue_UID);
                        TaskManager.DeleteTask(weeklyPrepare_UID);
                    }
                    else
                    {
                        if (value != RESET_CHAR)
                        {
                            if (weeklyDue == "")
                            {
                                return;
                            }
                            weeklyDue = value.ToDate().GetString();

                        }
                        ReacuringTask task = (ReacuringTask)
                            TaskManager.GetOrCreate(weeklyDue_UID, TaskType.Reacuring, out int i);
                        weeklyDue_UID = i;

                        task.Name = $"Weekly Payroll due for {parentName}";
                        task.Offset = Constants.WEEK;
                        task.SetDate(weeklyDue.ToDate());


                        ReacuringTask task2 = (ReacuringTask)
                            TaskManager.GetOrCreate(weeklyPrepare_UID, TaskType.Reacuring, out i);
                        weeklyPrepare_UID = i;

                        task2.Name = $"Prepare Weekly Payroll due for {parentName}";
                        task2.Offset = Constants.WEEK;
                        task2.SetDate(weeklyDue.ToDate().AddDays(-2));

                        task2.Save();

                        task.TryAddChild(task2);
                        task.Save();
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
                        if (weeklyDateTB != null)
                        {
                            twoWeeklyDataTB.Style = normal;
                        }
                        TwoWeekly_Date = RESET_CHAR;
                    }
                    else
                    {
                        if (weeklyDateTB != null)
                        {
                            twoWeeklyDataTB.Style = readonly_;
                        }
                        TaskManager.DeleteTask(twoWeeklyDue_UID);
                        TaskManager.DeleteTask(twoWeeklyPrepare_UID);
                    }
                }
            }
        }

        private string twoWeeklyDue;
        public string TwoWeekly_Date
        {
            get => twoWeeklyDue;
            set
            {
                if (twoWeeklyDue != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(twoWeeklyDue_UID);
                        TaskManager.DeleteTask(twoWeeklyPrepare_UID);
                    }
                    else
                    {
                        if (twoWeeklyDue == "")
                        {
                            return;
                        }
                        if (value != RESET_CHAR)
                        {
                            twoWeeklyDue = value.ToDate().GetString();
                        }
                        ReacuringTask task = (ReacuringTask)
                            TaskManager.GetOrCreate(twoWeeklyDue_UID, TaskType.Reacuring, out int i);
                        twoWeeklyDue_UID = i;

                        task.Name = $"2 Weekly Payroll due for {parentName}";
                        task.Offset = Constants.WEEK * 2;
                        task.SetDate(twoWeeklyDue.ToDate());


                        ReacuringTask task2 = (ReacuringTask)
                            TaskManager.GetOrCreate(twoWeeklyPrepare_UID, TaskType.Reacuring, out i);
                        twoWeeklyPrepare_UID = i;

                        task2.Name = $"Prepare 2 Weekly Payroll due for {parentName}";
                        task2.Offset = Constants.WEEK * 2;
                        task2.SetDate(twoWeeklyDue.ToDate().AddDays(-2));

                        task2.Save();

                        task.TryAddChild(task2);
                        task.Save();
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
                        if (weeklyDateTB != null)
                        {
                            monthlyDataTB.Style = normal;
                        }
                        Monthly_Date = RESET_CHAR;
                    }
                    else
                    {
                        if (weeklyDateTB != null)
                        {
                            monthlyDataTB.Style = readonly_;
                        }
                        Monthly_SelectedDateType = 0;
                        TaskManager.DeleteTask(monthlyDue_UID);
                        TaskManager.DeleteTask(monthlyPrepare_UID);
                    }
                }
            }
        }
        private int monthly_SelectedDateType;
        public int Monthly_SelectedDateType
        {
            get => monthly_SelectedDateType;
            set
            {
                if (value != monthly_SelectedDateType)
                {
                    monthly_SelectedDateType = value;
                    DateTimeInterval interval = Constants.MONTH.Copy();

                    switch (Monthly_SelectedDateType)
                    {
                        case 0:
                            interval.AssertDate = AssertDate.Month28th;
                            break;
                        case 1:
                            interval.AssertDate = AssertDate.LastofMonth;
                            break;
                        case 2:
                            interval.AssertDate = AssertDate.LastFridayOfMonth;
                            break;
                    }
                    ReacuringTask task = (ReacuringTask)
                        TaskManager.GetTask(monthlyDue_UID);

                    if (task != null)
                    {
                        task.Offset = interval;
                        task.Save();
                    }

                    task = (ReacuringTask)
                        TaskManager.GetTask(monthlyPrepare_UID);

                    if (task != null)
                    {
                        task.Offset = interval;
                        task.Save();
                    }
                }
            }
        }
        private string monthlyDue;
        public string Monthly_Date
        {
            get => monthlyDue;
            set
            {
                if (monthlyDue != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(monthlyDue_UID);
                        TaskManager.DeleteTask(monthlyPrepare_UID);
                    }
                    else
                    {
                        if (value != RESET_CHAR)
                        {
                            if (monthlyDue == "")
                            {
                                return;
                            }
                            monthlyDue = value.ToDate().GetString();
                        }
                        DateTimeInterval interval = Constants.MONTH.Copy();

                        switch (Monthly_SelectedDateType)
                        {
                            case 0:
                                interval.AssertDate = AssertDate.Month28th;
                                break;
                            case 1:
                                interval.AssertDate = AssertDate.LastofMonth;
                                break;
                            case 2:
                                interval.AssertDate = AssertDate.LastFridayOfMonth;
                                break;
                        }

                        ReacuringTask task = (ReacuringTask)
                            TaskManager.GetOrCreate(monthlyDue_UID, TaskType.Reacuring, out int i);
                        monthlyDue_UID = i;

                        task.Name = $"Monthly Payroll due for {parentName}";
                        task.Offset = interval;
                        task.SetDate(monthlyDue.ToDate());


                        ReacuringTask task2 = (ReacuringTask)
                            TaskManager.GetOrCreate(monthlyPrepare_UID, TaskType.Reacuring, out i);
                        monthlyPrepare_UID = i;

                        task2.Name = $"Prepare Monthly Payroll due for {parentName}";
                        task2.Offset = Constants.MONTH;
                        task2.SetDate(monthlyDue.ToDate().AddDays(-2));
                        task2.Save();

                        task.TryAddChild(task2);
                        task.Save();

                        DateTimeInterval f = interval.Copy();
                        f.Day = f.Month = f.Year = 0;

                        monthlyDue = monthlyDue.ToDate().AddOffset(f).GetString();
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
