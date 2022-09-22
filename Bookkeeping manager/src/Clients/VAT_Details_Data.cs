using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;
using System;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Clients
{
    public class VAT_Details_Data : ClientData
    {
        private TextBox nextReturnDate;
        public VAT_Details_Data(string name) : base(name)
        {
            vatFreq = 0;
            vatPeriodEnd = "";
        }
        internal void Initalize(TextBox nextReturnDate)
        {
            this.nextReturnDate = nextReturnDate;
        }
        public override void ReName(string name)
        {
            TaskManager.RenameTask(periodEnd_UID, parentName, name);
            TaskManager.RenameTask(reqVAT_UID, parentName, name);
            TaskManager.RenameTask(fileVAT_UID, parentName, name);
            base.ReName(name);
        }
        public override void UpdateTasks()
        {
            base.UpdateTasks();
            TaskManager.UpdateValue(periodEnd_UID, ref vatPeriodEnd);
        }
        private void UpdateTaskFrequency()
        {
            DateTimeInterval interval = Constants.MONTH;
            if (VAT_Frequency == 1)
            {
                interval *= 3;
            }
            ReacuringTask task = (ReacuringTask) TaskManager.GetTask(periodEnd_UID);
            if(task != null)
            {
                task.Offset = interval;
                task.Save();
            }
            task = (ReacuringTask)TaskManager.GetTask(reqVAT_UID);
            if (task != null)
            {
                task.Offset = interval;
                task.Save();
            }
            task = (ReacuringTask)TaskManager.GetTask(fileVAT_UID);
            if (task != null)
            {
                task.Offset = interval;
                task.Save();
            }
        }
        public void UpdateDependents()
        {
            if(nextReturnDate is null)
            {
                return;
            }
            if(!string.IsNullOrEmpty(vatPeriodEnd))
            {
                DateTime date = vatPeriodEnd.ToDate();

                nextReturnDate.Text = date.AddMonths(2).SetDay(7).GetString();
            }
            else
            {
                nextReturnDate.Text = "";
            }
        }
        private int vatFreq;
        public int VAT_Frequency
        {
            get => vatFreq;
            set
            {
                if(vatFreq != value)
                {
                    vatFreq = value;
                    UpdateTaskFrequency();
                }
            }
        }
        private string vatPeriodEnd;
        public int periodEnd_UID { get; set; }
        public int reqVAT_UID { get; set; }
        public int fileVAT_UID { get; set; }
        public string VAT_PeriodEnd
        {
            get => vatPeriodEnd;
            set
            {
                if(value != vatPeriodEnd)
                {
                    vatPeriodEnd = value;
                    if(string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(periodEnd_UID);
                        TaskManager.DeleteTask(reqVAT_UID);
                        TaskManager.DeleteTask(fileVAT_UID);
                    }
                    else
                    {
                        DateTimeInterval interval = Constants.MONTH;
                        if(VAT_Frequency == 1)
                        {
                            interval *= 3;
                        }

                        DateTime date = vatPeriodEnd.ToDate();
                        date = date.GetLastDay();
                        vatPeriodEnd = date.GetString();


                        ReacuringTask periodEndTask = (ReacuringTask)
                            TaskManager.GetOrCreate(periodEnd_UID, TaskType.Reacuring, out int i);
                        periodEnd_UID = i;

                        periodEndTask.Name = $"VAT period end for {parentName}";
                        periodEndTask.Offset = interval.Copy();
                        periodEndTask.Offset.AssertDate = AssertDate.LastofMonth;
                        periodEndTask.SetDate(date);
                        periodEndTask.Save();


                        ReacuringTask reqTask = (ReacuringTask)
                            TaskManager.GetOrCreate(reqVAT_UID, TaskType.Reacuring, out i);
                        reqVAT_UID = i;

                        reqTask.Name = $"Request VAT info for {parentName}";
                        reqTask.Offset = interval;
                        reqTask.SetDate(date.SetDay(5));
                        reqTask.Save();


                        ReacuringTask fileTask = (ReacuringTask)
                            TaskManager.GetOrCreate(fileVAT_UID, TaskType.Reacuring, out i);
                        fileVAT_UID = i;

                        fileTask.Name = $"File VAT for {parentName}";
                        fileTask.Offset = interval;
                        fileTask.SetDate(date.SetDay(7));
                        fileTask.Save();

                        UpdateDependents();
                    }
                }
            }
        }
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
