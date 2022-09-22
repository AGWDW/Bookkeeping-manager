using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;
using System;
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
            ape = "";
        }

        public override void ReName(string name)
        {
            TaskManager.RenameTask(ape_UID, parentName, name);
            TaskManager.RenameTask(reqAcc_UID, parentName, name);
            TaskManager.RenameTask(startPrep_UID, parentName, name);
            TaskManager.RenameTask(urgentPrep_UID, parentName, name);
            TaskManager.RenameTask(veryUrgentPrep_UID, parentName, name);
            TaskManager.RenameTask(ct600_UID, parentName, name);
            TaskManager.RenameTask(lastFiling_UID, parentName, name);
            TaskManager.RenameTask(taxDue_UID, parentName, name);
            base.ReName(name);
        }

        internal void SetAPE_Dependents()
        {
            if(string.IsNullOrEmpty(ape) || chAccountsNextDue_Current is null)
            {
                return;
            }
            // Current
            DateTime apeDate = ape.ToDate().AddYears(-1);

            chAccountsNextDue_Current.Text = apeDate.AddMonths(9).GetLastDay().GetString();
            ct600Due_Current.Text = apeDate.AddYears(1).GetLastDay().AddDays(-1).GetString();
            hmrcYearEnd_Current.Text = apeDate.AddMonths(10).GetFirstDay().GetString();
            // Future          
            chAccountsNextDue_Future.Text = chAccountsNextDue_Current.Text.ToDate().AddYears(1).GetString();
            ct600Due_Future.Text = ct600Due_Current.Text.ToDate().AddYears(1).GetString();
            hmrcYearEnd_Future.Text = hmrcYearEnd_Current.Text.ToDate().AddYears(1).GetString();

        }
        public override void UpdateTasks()
        {
            TaskManager.UpdateValue(ape_UID, ref ape);
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

        public int ape_UID { get; set; } 
        public int reqAcc_UID { get; set; } 
        public int startPrep_UID { get; set; }
        public int urgentPrep_UID { get; set; } 
        public int veryUrgentPrep_UID { get; set; } 
        public int ct600_UID { get; set; } 
        public int lastFiling_UID { get; set; } 
        public int taxDue_UID { get;set; } 
        private string ape;
        public string AccountPeriodEnd
        {
            get => ape;
            set
            {
                if(value != ape)
                {
                    ape = value;
                    if (string.IsNullOrEmpty(value))
                    {
                        if(chAccountsNextDue_Current != null)
                        {
                            chAccountsNextDue_Current.Text =
                            ct600Due_Current.Text =
                            hmrcYearEnd_Current.Text = "";

                            chAccountsNextDue_Future.Text =
                                ct600Due_Future.Text =
                                hmrcYearEnd_Future.Text = "";
                        }

                        TaskManager.DeleteTask(ape_UID);
                        TaskManager.DeleteTask(reqAcc_UID);
                        TaskManager.DeleteTask(startPrep_UID);
                        TaskManager.DeleteTask(urgentPrep_UID);
                        TaskManager.DeleteTask(veryUrgentPrep_UID);
                        TaskManager.DeleteTask(ct600_UID);
                        TaskManager.DeleteTask(lastFiling_UID);
                        TaskManager.DeleteTask(taxDue_UID);

                        return;
                    }

                    DateTime apeDate = ape.ToDate();
                    apeDate = apeDate.GetLastDay();

                    ape = apeDate.GetString();


                    ReacuringTask parentTask = (ReacuringTask)
                        TaskManager.GetOrCreate(ape_UID, TaskType.Reacuring, out int i);
                    ape_UID = i;

                    parentTask.Name = $"Year end for {parentName}";
                    parentTask.Offset = Constants.YEAR;
                    parentTask.SetDate(apeDate);



                    ReacuringTask task1 = (ReacuringTask)
                        TaskManager.GetOrCreate(reqAcc_UID, TaskType.Reacuring, out i);
                    reqAcc_UID = i;

                    task1.Name = $"Request accounts info for {parentName}";
                    task1.Offset = Constants.YEAR.Copy();
                    task1.Offset.AssertDate = AssertDate.LastofMonth;
                    DateTime date = apeDate.AddYears(-1).AddMonths(1).GetLastDay();
                    task1.SetDate(date);


                    ReacuringTask task2 = (ReacuringTask)
                        TaskManager.GetOrCreate(startPrep_UID, TaskType.Reacuring, out i);
                    startPrep_UID = i;

                    task2.Name = $"Start to prepare accounts for {parentName}";
                    task2.Offset = Constants.YEAR;
                    date = apeDate.AddYears(-1).AddMonths(4).SetDay(14);
                    task2.SetDate(date);


                    ReacuringTask task3 = (ReacuringTask)
                        TaskManager.GetOrCreate(urgentPrep_UID, TaskType.Reacuring, out i);
                    urgentPrep_UID = i;

                    task3.Name = $"Urgent prepare accounts for {parentName}";
                    task3.Offset = Constants.YEAR.Copy();
                    task3.Offset.AssertDate = AssertDate.LastofMonth;
                    date = apeDate.AddYears(-1).AddMonths(7).GetLastDay();
                    task3.SetDate(date);


                    ReacuringTask task4 = (ReacuringTask)
                        TaskManager.GetOrCreate(veryUrgentPrep_UID, TaskType.Reacuring, out i);
                    veryUrgentPrep_UID = i;

                    task4.Name = $"VERY urgent prepare accounts for {parentName}";
                    task4.Offset = Constants.YEAR.Copy();
                    task4.Offset.AssertDate = AssertDate.LastofMonth;
                    date = apeDate.AddYears(-1).AddMonths(8).GetLastDay();
                    task4.SetDate(date);

                    parentTask.TryAddChild(task1);
                    parentTask.TryAddChild(task2);
                    parentTask.TryAddChild(task3);
                    parentTask.TryAddChild(task4);



                    ReacuringTask ct600 = (ReacuringTask)
                        TaskManager.GetOrCreate(ct600_UID, TaskType.Reacuring, out i);
                    ct600_UID = i;

                    ct600.Name = $"CT600 due for {parentName}";
                    ct600.Offset = Constants.YEAR;
                    date = apeDate.GetLastDay().AddDays(-1);
                    ct600.SetDate(date);

                    ReacuringTask lastFillingDate = (ReacuringTask)
                        TaskManager.GetOrCreate(lastFiling_UID, TaskType.Reacuring, out i);
                    lastFiling_UID = i;

                    lastFillingDate.Name = $"Last date for filing accounts due for {parentName}";
                    lastFillingDate.Offset = Constants.YEAR.Copy();
                    lastFillingDate.Offset.AssertDate = AssertDate.LastofMonth;
                    date = apeDate.AddYears(-1).GetLastDay();
                    lastFillingDate.SetDate(date);

                    ReacuringTask hmrc = (ReacuringTask)
                        TaskManager.GetOrCreate(taxDue_UID, TaskType.Reacuring, out i);
                    taxDue_UID = i;

                    hmrc.Name = $"HMRC Tax due for {parentName}";
                    hmrc.Offset = Constants.YEAR.Copy();
                    hmrc.Offset.AssertDate = AssertDate.FirstOfMonth;
                    date = apeDate.AddMonths(10).GetFirstDay();
                    hmrc.SetDate(date);


                    parentTask.Save();

                    task1.Save();
                    task2.Save();
                    task3.Save();
                    task4.Save();

                    ct600.Save();
                    lastFillingDate.Save();
                    hmrc.Save();


                    SetAPE_Dependents();
                }
            }
        }
        public string CT_PaymentReference { get; set; }
        public string AccountsReccords { get; set; }
        public string AccountsProgressNotes { get; set; }
    }
}
