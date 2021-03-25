using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace AMScripts
{
    public abstract class MongoData
    {
#pragma warning disable IDE1006 // Naming Styles
        public ObjectId _id { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
    #region Client

    public class ClientString : MongoData
    {
        public string Name { get; set; }
        public string VATPeriod { get; set; }
        public string P11D { get; set; }
        public string APE { get; set; }
        public CompanyDetatilsString CompanyDetatils { get; set; }
        public List<ContactString> Contacts { get; set; }
        public AccountantString Accountant { get; set; }
        public ServivesString Servives { get; set; }
        public Accounts_ReturnsString Accounts_Returns { get; set; }
        public VATString VAT { get; set; }
        public PAYEString PAYE { get; set; }
    }
    public class CompanyDetatilsString
    {
        public string ConfirmationStatementDate { get; set; }
        public string CompanyNumber { get; set; }
        public string CharityNumber { get; set; }
        public string IncorporationDate { get; set; }
        public string CompanyTradingAs { get; set; }
        public string RegisteredAddress { get; set; }
        public string CompanyPostalAdress { get; set; }
        public string InvoiceAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyTelephone { get; set; }
        public string Turnover { get; set; }
        public string SICCode { get; set; }
        public string NatureOfBusiness { get; set; }
        public string CompanyUTR { get; set; }
        public string CompaniesHouseAuthenticationCode { get; set; }
        public string Name { get; set; }
    } 
    public class ContactString
    {
        public string Position { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PostalAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string NINumber { get; set; }
        public string PersonalUTRNumber { get; set; }
        public string TermsSigned { get; set; }
        public string AMLPaperworkReceived { get; set; }
        public bool   PhotoIDVerified { get; set; }
        public bool   AddressVerified { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string PreferredLanguage { get; set; }
    }
    public class AccountantString
    {
        public string FullName { get; set; }
        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
    }
    public class ServivesString
    {
        public bool SelfAssesmentChecked { get; set; }
        public bool P11DChecked { get; set; }
        public string Bookkeeping { get; set; }
        public string VATReturn { get; set; }
        public string ManagementReturns { get; set; }
        public string Payroll { get; set; }
        public string AutoEnrolment { get; set; }
        public string CIS { get; set; }
        public string BillPayments { get; set; }
        public string Invoicing { get; set; }
        public string AccountsPrep { get; set; }
        public string CT600Return { get; set; }
        public string SelfAssessment { get; set; }
        public string ConfirmationStatement { get; set; }
        public string P11D { get; set; }
        public string CharitiesCommission { get; set; }
        public string Consultation_Advice { get; set; }
        public string Software { get; set; }
        public string CompanySetup { get; set; }
        public string Other { get; set; }
        public string rate { get; set; }
               
        public string Monthly { get; set; }
        public string Quarterly { get; set; }
        public string Annual { get; set; }
    }
    public class Accounts_ReturnsString
    {
        public string CTPaymentReference { get; set; }
        public string AccountsRecordsReceived { get; set; }
        public string AccountsProgressNotes { get; set; }
    }
    public class VATString
    {
        public int    VATFrequency { get; set; }
        public string ReccordsRecieved { get; set; }
        public string ProgressNotes { get; set; }
        public string VATNumber { get; set; }
        public string VATAddress { get; set; }
        public string DateOfRegistration { get; set; }
        public string EffectiveDate { get; set; }
        public string EstimatedTurnover { get; set; }
        public string AppliedForMTD { get; set; }
        public bool   MTDReady { get; set; }
        public bool   DirectDebit { get; set; }
        public bool   StandardScheme { get; set; }
        public bool   CashAccountingScheme { get; set; }
        public bool   FlatRate { get; set; }
        public string FlatRateCategory { get; set; }
        public int    MonthOfLastQuarterSubmitted { get; set; }
        public string BoxOfLastQuarterSubmitted { get; set; }
        public string GeneralNotesVAT { get; set; }
    }
    public class PAYEFrequencyString
    {
        public string NumberOfEmployees { get; set; }
        public string FistPayDate { get; set; }
        public string PayPeriod { get; set; }
        public string RTIDeadline { get; set; }
        public int PayDates { get; set; }
    }
    public class PAYEString
    {
        public PAYEString()
        {
            PAYEFrequencies = new List<PAYEFrequencyString>();
        }
        public List<PAYEFrequencyString> PAYEFrequencies { get; set; }

        public string EmployersRefernce { get; set; }
        public string AccountsOfficeReference { get; set; }
        public string PAYESchemeCeased { get; set; }
        public string PAYEReccordsRecieved { get; set; }
        public string GeneralNotesPAYE { get; set; }
        // Auto-Enrolment
        public string AutoEnrolmentRecordsReceived { get; set; }
        public string AutoEnrolmentProgressNote { get; set; }
        public string AutoEnrolmentStaging { get; set; }
        public string PostponementDate { get; set; }
        public string ThePensionsRegulatorOptOutDate { get; set; }
        public string ReEnrolmentDate { get; set; }
        public string PensionProvider { get; set; }
        public string PensionID { get; set; }
        public string DeclarationOfComplianceDue { get; set; }
        public string DeclarationOfComplianceSubmission { get; set; }
        // P11D
        public string NextP11DReturnDue { get; set; }
        public string P11DRecordsReceived { get; set; }
        public string P11DProgressNote { get; set; }
    }
    #endregion
}
