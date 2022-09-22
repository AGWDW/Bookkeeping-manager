using Bookkeeping_manager.src.Tasks;
using System;
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

            if (SelfAssessmentEnabled)
            {
                if (selfAssess_TB != null)
                {
                    selfAssess_TB.Style = normal;
                }
            }
            else
            {
                if (selfAssess_TB != null)
                {
                    selfAssess_TB.Style = readonly_;
                }
            }

            if (P11D_Enabled)
            {
                if (p11d_TB != null)
                {
                    p11d_TB.Style = normal;
                }
            }
            else
            {
                if (p11d_TB != null)
                {
                    p11d_TB.Style = readonly_;
                }
            }
        }
        public int p11dDue_UID { get; set; }
        public int p11dPrepare_UID { get; set; }
        public int saDue_UID { get; set; }
        public int fileSA_UID { get; set; }
        public int requestSA_UID { get; set; }
        public int prepareSA_UID { get; set; }
public override void ReName(string name)
        {
            TaskManager.RenameTask(p11dDue_UID, parentName, name);
            TaskManager.RenameTask(p11dPrepare_UID, parentName, name);

            TaskManager.RenameTask(saDue_UID, parentName, name);
            TaskManager.RenameTask(fileSA_UID, parentName, name);
            TaskManager.RenameTask(requestSA_UID, parentName, name);
            TaskManager.RenameTask(prepareSA_UID, parentName, name);

            base.ReName(name);
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
                        if(selfAssess_TB != null)
                        {
                            selfAssess_TB.Style = normal;
                        }
                        ReacuringTask t = (ReacuringTask)
                            TaskManager.GetOrCreate(saDue_UID, TaskType.Reacuring, out int i);
                        saDue_UID = i;

                        t.Name = $"Self Assessment due for {parentName}";
                        t.Offset = Constants.YEAR;
                        t.SetDate(DateTime.Today.Year, 1, 31);
                        t.Save();

                        t = (ReacuringTask)
                            TaskManager.GetOrCreate(fileSA_UID, TaskType.Reacuring, out i);
                        fileSA_UID = i;

                        t.Name = $"File SA for {parentName}";
                        t.Offset = Constants.YEAR;
                        t.SetDate(DateTime.Today.Year, 1, 5);
                        t.Save();

                        t = (ReacuringTask)
                            TaskManager.GetOrCreate(requestSA_UID, TaskType.Reacuring, out i);
                        requestSA_UID = i;

                        t.Name = $"Request SA Info for {parentName}";
                        t.Offset = Constants.YEAR;
                        t.SetDate(DateTime.Today.Year - 1, 5, 15);
                        t.Save();

                        t = (ReacuringTask)
                            TaskManager.GetOrCreate(prepareSA_UID, TaskType.Reacuring, out i);
                        prepareSA_UID = i;

                        t.Name = $"Prepare SA for {parentName}";
                        t.Offset = Constants.YEAR;
                        t.SetDate(DateTime.Today.Year - 1, 9, 15);
                        t.Save();
                    }
                    else
                    {
                        if (selfAssess_TB != null)
                        {
                            selfAssess_TB.Style = normal;
                        }
                        TaskManager.DeleteTask(saDue_UID);
                        TaskManager.DeleteTask(fileSA_UID);
                        TaskManager.DeleteTask(requestSA_UID);
                        TaskManager.DeleteTask(prepareSA_UID);
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
                    if (value && p11d_TB != null)
                    {
                        p11d_TB.Style = normal;
                    }
                    else if(p11d_TB != null)
                    {
                        p11d_TB.Style = readonly_;
                    }

                    p11d_Enabled = value;
                    if (!value)
                    {
                        TaskManager.DeleteTask(p11dDue_UID);
                        TaskManager.DeleteTask(p11dPrepare_UID);
                        return;
                    }

                    ReacuringTask task = (ReacuringTask)
                        TaskManager.GetOrCreate(p11dDue_UID, TaskType.Reacuring, out int i);
                    p11dDue_UID = i;

                    task.Name = $"P11D Due for {parentName}";
                    task.SetDate(DateTime.Today.Year, 7, 6);
                    task.Offset = Constants.YEAR;
                    task.Save();

                    task = (ReacuringTask)
                        TaskManager.GetOrCreate(p11dPrepare_UID, TaskType.Reacuring, out i);
                    p11dPrepare_UID = i;

                    task.Name = $"Prepare P11D for {parentName}";
                    task.SetDate(DateTime.Today.Year, 5, 6);
                    task.Offset = Constants.YEAR;
                    task.Save();
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
