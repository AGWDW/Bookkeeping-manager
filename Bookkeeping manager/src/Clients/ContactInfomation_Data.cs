using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;

namespace Bookkeeping_manager.src.Clients
{
    internal class ContactInfomation_Data
    {
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
                    ReacuringTask task = (ReacuringTask)TaskManager.GetTask(amlReview_UID);
                    if (task is null)
                    {
                        task = new ReacuringTask(value.ToDate());
                        task.Name = $"AML Review due for {FirstName} {LastName}";
                    }
                    task.SetDate(value.ToDate());
                }
            }
        }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
    }
}
