using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;
using System;

namespace Bookkeeping_manager.src.Clients
{
    public class ContactInfomation_Data : ClientData
    {

        public ContactInfomation_Data(string name) : base(name)
        {
        }

        private int amlReview_UID;
        public string Position { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
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
                    if(value == "")
                    {
                        TaskManager.DeleteTask(amlReview_UID);
                    }
                    ReacuringTask task = (ReacuringTask)TaskManager.GetOrCreate(amlReview_UID, TaskType.Reacuring, out amlReview_UID);

                    task.Name = $"AML Review due for {FirstName} ({parentName})";
                    task.Offset = Constants.YEAR;
                    task.SetDate(value.ToDateNew());
                }
            }
        }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
    }
}
