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

            SelfAssessmentEnabled = !SelfAssessmentEnabled;
            SelfAssessmentEnabled = !SelfAssessmentEnabled;

            P11D_Enabled = !P11D_Enabled;
            P11D_Enabled = !P11D_Enabled;
        }
        private int p11dDue_UID, p11dPrepare_UID;
        private int saDue_UID, fileSA_UID, requestSA_UID, prepareSA_UID;
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
                        selfAssess_TB.Style = normal;
                        // re creates all the tasks
                        string sa = SelfAssessment;
                        SelfAssessment = "";
                        SelfAssessment = sa;
                    }
                    else
                    {
                        selfAssess_TB.Style = readonly_;
                        TaskManager.DeleteTask(saDue_UID);
                        TaskManager.DeleteTask(fileSA_UID);
                        TaskManager.DeleteTask(requestSA_UID);
                        TaskManager.DeleteTask(prepareSA_UID);
                    }
                }
            }
        }

        private string selfAssessment;
        public string SelfAssessment
        {
            get => selfAssessment;
            set
            {
                if (selfAssessment != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(saDue_UID);
                        TaskManager.DeleteTask(fileSA_UID);
                        TaskManager.DeleteTask(requestSA_UID);
                        TaskManager.DeleteTask(prepareSA_UID);
                    }

                    selfAssessment = value;
                    ReacuringTask t = (ReacuringTask)
                        TaskManager.GetOrCreate(saDue_UID, TaskType.Reacuring, out saDue_UID);
                    t.Name = $"Self Assessment due for {parentName}";
                    t.Offset = Constants.YEAR;
                    t.SetDate(DateTime.Today.Year, 1, 31);

                    t = (ReacuringTask)
                        TaskManager.GetOrCreate(fileSA_UID, TaskType.Reacuring, out fileSA_UID);
                    t.Name = $"File SA for {parentName}";
                    t.Offset = Constants.YEAR;
                    t.SetDate(DateTime.Today.Year, 1, 5);

                    t = (ReacuringTask)
                        TaskManager.GetOrCreate(requestSA_UID, TaskType.Reacuring, out requestSA_UID);
                    t.Name = $"Request SA Info for {parentName}";
                    t.Offset = Constants.YEAR;
                    t.SetDate(DateTime.Today.Year - 1, 5, 15);

                    t = (ReacuringTask)
                        TaskManager.GetOrCreate(prepareSA_UID, TaskType.Reacuring, out prepareSA_UID);
                    t.Name = $"Prepare SA for {parentName}";
                    t.Offset = Constants.YEAR;
                    t.SetDate(DateTime.Today.Year - 1, 9, 15);
                }
            }
        }
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

                    if (p11d_Enabled != value)
                    {
                        p11d_Enabled = value;
                        if (!value)
                        {
                            TaskManager.DeleteTask(p11dDue_UID);
                            TaskManager.DeleteTask(p11dPrepare_UID);
                            return;
                        }

                        ReacuringTask task = (ReacuringTask)
                            TaskManager.GetOrCreate(p11dDue_UID, TaskType.Reacuring, out p11dDue_UID);

                        task.Name = $"P11D Due for {parentName}";
                        task.SetDate(DateTime.Today.Year, 7, 6);
                        task.Offset = Constants.YEAR;

                        task = (ReacuringTask)
                            TaskManager.GetOrCreate(p11dPrepare_UID, TaskType.Reacuring, out p11dPrepare_UID);

                        task.Name = $"Prepare P11D for {parentName}";
                        task.SetDate(DateTime.Today.Year, 5, 6);
                        task.Offset = Constants.YEAR;
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
