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
        public int confirmationDate_UID { get; set; }
        // not created
        public int submitConfirmation_UID { get; set; }
        public ClientInfomation_Data(string parentName) : base(parentName)
        {
        }
        public void Initalize(TextBox confirDateBox, Style readOnly, Style regular)
        {
            this.confirDateBox = confirDateBox;
            this.readOnly = readOnly;
            this.regular = regular;
            if (ConfirmationEnabled)
            {
                if (confirDateBox != null)
                {
                    confirDateBox.Style = regular;
                }
            }
            else
            {
                if (confirDateBox != null)
                {
                    confirDateBox.Style = readOnly;
                }
            }
        }
        public override void ReName(string name)
        {
            TaskManager.RenameTask(confirmationDate_UID, parentName, name);
            TaskManager.RenameTask(submitConfirmation_UID, parentName, name);

            base.ReName(name);
        }
        public override void UpdateTasks()
        {
            TaskManager.UpdateValue(confirmationDate_UID, ref confirmationStatementDate);
            base.UpdateTasks();
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
                        if(confirDateBox != null)
                        {
                            confirDateBox.Style = regular;
                        }
                        ConfirmationStatementDate = RESET_CHAR;
                    }
                    else
                    {
                        if (confirDateBox != null)
                        {
                            confirDateBox.Style = readOnly;
                        }
                        TaskManager.DeleteTask(confirmationDate_UID);
                        TaskManager.DeleteTask(submitConfirmation_UID);
                    }
                }
            }
        }
        private void createTasks(string date)
        {
            ReacuringTask task = (ReacuringTask)
                TaskManager.GetOrCreate(confirmationDate_UID, TaskType.Reacuring, out int t);
            confirmationDate_UID = t;
            task.Name = $"Confirmation Statement Due for {parentName}";
            task.SetDate(date.ToDate());
            task.Offset = Constants.YEAR;
            task.Save();

            /*TimeLimitedTask task2 = (TimeLimitedTask)
                TaskManager.GetOrCreate(submitConfirmation_UID, TaskType.TimeLimited, out submitConfirmation_UID);
            task2.Name = $"Submit Confirmation for {parentName}";
            task2.Offset = Constants.YEAR;*/
        }

        private string confirmationStatementDate;
        public string ConfirmationStatementDate
        {
            get => confirmationStatementDate;
            set
            {
                if (value != confirmationStatementDate)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(confirmationDate_UID);
                        TaskManager.DeleteTask(submitConfirmation_UID);
                        return;
                    }
                    if(value != RESET_CHAR)
                    {
                        if (confirmationStatementDate == "")
                        {
                            return;
                        }
                        confirmationStatementDate = value.ToDate().GetString();
                    }


                    createTasks(value);
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
