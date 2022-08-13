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
    public class ClientInfomation_Data
    {
        string parentName;
        TextBox confirDateBox;
        Style readOnly, regular;
        int confirmationDate_UID;
        public ClientInfomation_Data(string parentName)
        {
            this.parentName = parentName;
        }
        public void Initalize(TextBox confirDateBox, Style readOnly, Style regular)
        {
            this.confirDateBox = confirDateBox;
            this.readOnly = readOnly;
            this.regular = regular;
            ConfirmationEnabled = true;
            ConfirmationEnabled = false;
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
                    }
                    else
                    {
                        confirDateBox.Style = readOnly;
                        TaskManager.DeleteTask(confirmationDate_UID);
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
                    ReacuringTask task = (ReacuringTask)TaskManager.GetTask(confirmationDate_UID);
                    if (task is null)
                    {
                        task = new ReacuringTask(value.ToDate());
                        task.Name = $"Confirmation Statement Due for {parentName}";
                    }
                    task.SetDate(value.ToDate());
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
        public string Software { get; set; }
    }
}
