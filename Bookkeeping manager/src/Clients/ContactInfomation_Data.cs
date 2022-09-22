using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;
using MongoDB.Driver.Core.Operations;
using System;

namespace Bookkeeping_manager.src.Clients
{
    public class ContactInfomation_Data : ClientData
    {

        public ContactInfomation_Data(string name) : base(name)
        {
        }
        public override void ReName(string name)
        {
            TaskManager.RenameTask(amlReview_UID, parentName, name);
            base.ReName(name);
        }

        public override void UpdateTasks()
        {
            TaskManager.UpdateValue(amlReview_UID, ref aml_ReviewDue);
        }

        public int amlReview_UID { get; set; }
        public string Position { get; set; }
        public string Title { get; set; }
        private string fName;
        public string FirstName
        {
            get => fName;
            set
            {
                if(fName != value)
                {
                    TaskManager.RenameTask(amlReview_UID, $"{fName} ({parentName}", $"{value} ({parentName}");
                    fName = value;
                    Task t = TaskManager.GetTask(amlReview_UID);
                    if(t != null)
                    {
                        t.Save();
                    }
                }
            }
        }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalAddress { get; set; }
        public string NI_Number { get; set; }
        public string UTR_Number { get; set; }
        public string TermsSigned { get; set; }
        public string AML_Signed { get; set; }
        public bool PhotoID_Recieved { get; set; }
        public bool Address_Recieved { get; set; }
        public string AML_FormChecked { get; set; }
        private string aml_ReviewDue;
        public string AML_ReviewDue
        {
            get => aml_ReviewDue;
            set
            {
                if (value != aml_ReviewDue)
                {
                    aml_ReviewDue = value;
                    if(string.IsNullOrEmpty(value))
                    {
                        TaskManager.DeleteTask(amlReview_UID);
                        return;
                    }
                    ReacuringTask task = (ReacuringTask)
                        TaskManager.GetOrCreate(amlReview_UID, TaskType.Reacuring, out int t);
                    amlReview_UID = t;

                    task.Name = $"AML Review due for {FirstName} ({parentName})";
                    task.Offset = Constants.YEAR;
                    DateTime date = value.ToDate();
                    task.SetDate(date);
                    task.Save();
                    aml_ReviewDue = date.GetString();
                }
            }
        }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
    }
}
