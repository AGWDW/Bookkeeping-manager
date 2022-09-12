using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Clients
{
    public class ClientInfomation_Data : ClientData
    {
        TextBox confirDateBox;
        Style readOnly, regular;
        int confirmationDate_UID, submitConfirmation_UID;
        public ClientInfomation_Data(string parentName) : base(parentName)
        {
        }
        public void Initalize(TextBox confirDateBox, Style readOnly, Style regular)
        {
            this.confirDateBox = confirDateBox;
            this.readOnly = readOnly;
            this.regular = regular;
            ConfirmationEnabled = !ConfirmationEnabled;
            ConfirmationEnabled = !ConfirmationEnabled;
        }
        public override void ReName(string name)
        {
            TaskManager.RenameTask(confirmationDate_UID, parentName, name);
            TaskManager.RenameTask(submitConfirmation_UID, parentName, name);

            base.ReName(name);
        }
        public string CompanyNumber { get; set; }
        public string CharityNumber { get; set; }
        public string IncorporationDate { get; set; }

        private bool confirmationEnabled;
        public bool ConfirmationEnabled
        {
            get => confirmationEnabled;
            set
            {
                if (value != confirmationEnabled)
                {
                    confirmationEnabled = value;
                    if (value)
                    {
                        confirDateBox.Style = regular;
                        string t = confirmationStatementDate;
                        ConfirmationStatementDate = "";
                        ConfirmationStatementDate = t;
                    }
                    else
                    {
                        confirDateBox.Style = readOnly;
                        TaskManager.DeleteTask(confirmationDate_UID);
                        TaskManager.DeleteTask(submitConfirmation_UID);
                    }
                }
            }
        }

        private string confirmationStatementDate;
        public string ConfirmationStatementDate
        {
            get => confirmationStatementDate;
            set
            {
                if (value != confirmationStatementDate)
                {
                    confirmationStatementDate = value;
                    if (value == "" || value == null)
                    {
                        TaskManager.DeleteTask(confirmationDate_UID);
                        TaskManager.DeleteTask(submitConfirmation_UID);
                        return;
                    }

                    ReacuringTask task = (ReacuringTask)
                        TaskManager.GetOrCreate(confirmationDate_UID, TaskType.Reacuring, out confirmationDate_UID);

                    task.Name = $"Confirmation Statement Due for {parentName}";
                    task.SetDate(value.ToDate());
                    task.Offset = Constants.YEAR;

                    /*TimeLimitedTask task2 = (TimeLimitedTask)
                        TaskManager.GetOrCreate(submitConfirmation_UID, TaskType.TimeLimited, out submitConfirmation_UID);
                    task2.Name = $"Submit Confirmation for {parentName}";
                    task2.Offset = Constants.YEAR;*/
                }
            }
        }
        public string TradingAs { get; set; }
        public string RegistaredAddress { get; set; }
        public string PostalAddress { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string SIC_Code { get; set; }
        public string Nature_of_the_Business { get; set; }
        public string UTR { get; set; }
        public string CompanysHouseAuthCode { get; set; }
        public int Software { get; set; }
    }
}
