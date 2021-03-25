using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookkeeping_manager.Scripts;

namespace AM_Converter
{
    public static class AM_To_BM
    {
        public static void Convert()
        {
            MongoHandler AM = new MongoHandler(@"mongodb+srv://ReadWrite:pXVzPD3HBM2IzHdK@primary-84jli.mongodb.net/Data?retryWrites=true&w=majority");
            AM.SetDatabase("Data");
            MongoHandler BM = new MongoHandler(@"mongodb+srv://MainClient:M3YMhbTS63XT7yuK@maindata.7qgv2.mongodb.net/Data?retryWrites=true&w=majority");
            BM.SetDatabase("Data");

            List<AMScripts.ClientString> AMClients = AM.GetAllDocuments<AMScripts.ClientString>("Client");
            AMClients = AMClients.OrderBy(o => o.Name).ToList();
            DataHandler.AllowSet = true;
            foreach (AMScripts.ClientString client in AMClients)
            {

                Client c = new Client(client.Name);
                c.CompanyDetails  = Convert(client.CompanyDetatils, c);
                c.ContactDetials  = Convert(client.Contacts, c);
                c.Accountant      = Convert(client.Accountant, c);
                c.Services        = Convert(client.Servives, c);
                c.AccountsReturns = Convert(client.Accounts_Returns, c);
                c.VATDetails      = Convert(client.VAT, c);
                c.PAYEDetails     = Convert(client.PAYE, c);
                Convert(c, client);

                DataHandler.AllClients.Add(c);
            }
        }
        private static void Convert(Client res, AMScripts.ClientString client)
        {
            res.VATDetails.VATPeriodEnd = NotNull(client.VATPeriod);
            res.AccountsReturns.AccountsPeriodEnd = NotNull(client.APE);

        }
        private static CompanyDetails Convert(AMScripts.CompanyDetatilsString detatils, Client client)
        {
            CompanyDetails res = new CompanyDetails(client)
            {
                CompanyNumber = NotNull(detatils.CompanyNumber),
                CharityNumber = NotNull(detatils.CharityNumber),
                IncorporationDate = NotNull(detatils.IncorporationDate),
                ConfirmationStatementDate = NotNull(detatils.ConfirmationStatementDate),
                ConfirmationEnabled = NotNull(detatils.ConfirmationStatementDate) != "",
                CompanyTradingAs = NotNull(detatils.CompanyTradingAs),
                RegisteredAddress = NotNull(detatils.RegisteredAddress),
                CompanyPostalAdress = NotNull(detatils.CompanyPostalAdress),
                CompanyEmail = NotNull(detatils.CompanyEmail),
                CompanyTelephone = NotNull(detatils.CompanyTelephone),
                SICCode = NotNull(detatils.SICCode),
                NatureOfBusiness = NotNull(detatils.NatureOfBusiness),
                CompanyUTR = NotNull(detatils.CompanyUTR),
            };
            
            return res;
        }
        private static ContactDetials Convert(List<AMScripts.ContactString> detatils, Client client)
        {
            List<Contact> contacts = new List<Contact>();
            int i = 0;
            foreach(AMScripts.ContactString contact in detatils)
            {
                Contact c = new Contact(client, i++)
                {
                    Name = NotNull(contact.FirstName),
                    Position = NotNull(contact.Position),
                    Title = NotNull(contact.Title),
                    FirstName = NotNull(contact.FirstName),
                    MiddleName = NotNull(contact.MiddleName),
                    LastName = NotNull(contact.LastName),
                    DateOfBirth = NotNull(contact.DateOfBirth),
                    EmailAddress = NotNull(contact.Email),
                    PostalAddress = NotNull(contact.PostalAddress),
                    PhoneNumber = NotNull(contact.TelephoneNumber),
                    NINumber = NotNull(contact.NINumber),
                    PersonalUTRNmber = NotNull(contact.PersonalUTRNumber),
                    TermsSigned = NotNull(contact.TermsSigned),
                    AMLPapperworkSigned = NotNull(contact.AMLPaperworkReceived),
                    PhotoIDReadReceived = NotNull(contact.PhotoIDVerified.ToString().ToLower()),
                    AddressReadReceived = NotNull(contact.AddressVerified.ToString().ToLower()),
                    MaritalStatus = NotNull(contact.MaritalStatus),
                    Nationality = NotNull(contact.Nationality)
                };

                contacts.Add(c);
            }
            ContactDetials res = new ContactDetials(client, contacts.ToArray());
            return res;
        }
        private static Accountant Convert(AMScripts.AccountantString detatils, Client client)
        {
            Accountant res = new Accountant(client)
            {
                Name = NotNull( detatils.FullName),
                ContactEmail = NotNull( detatils.ContactEmail),
                PhoneNumber = NotNull( detatils.PhoneNumber),
                Notes = NotNull( detatils.Notes)
            };
            return res;
        }
        private static Services Convert(AMScripts.ServivesString detatils, Client client)
        {
            Services res = new Services(client)
            {
                Bookkeeping           = NotNull( detatils.Bookkeeping),
                VATReturns            = NotNull( detatils.VATReturn),
                ManagementReturns     = NotNull( detatils.ManagementReturns),
                Payroll               = NotNull( detatils.Payroll),
                AutoEnrolment         = NotNull( detatils.AutoEnrolment),
                CIS                   = NotNull( detatils.CIS),
                BillPayments          = NotNull( detatils.BillPayments),
                Invoicing             = NotNull( detatils.Invoicing),
                AccountsPreparation   = NotNull( detatils.AccountsPrep),
                CT600Returns          = NotNull( detatils.CT600Return),
                SelfAssessmentEnabled = detatils.SelfAssesmentChecked,
                SelfAssessment        = NotNull( detatils.SelfAssessment),
                ConfirmationStatement = NotNull( detatils.ConfirmationStatement),
                P11DEnabled           = detatils.P11DChecked,
                P11D                  = NotNull( detatils.P11D),
                ConsultationAdvice    = NotNull( detatils.Consultation_Advice),
                Software              = NotNull( detatils.Software),
                CompanySetup          = NotNull( detatils.CompanySetup)
            };
            return res;
        }
        private static AccountsReturns Convert(AMScripts.Accounts_ReturnsString detatils, Client client)
        {
            AccountsReturns res = new AccountsReturns(client)
            {
                CTPaymentReference      = NotNull( detatils.CTPaymentReference),
                AccountsRecordsReceived = NotNull( detatils.AccountsRecordsReceived),
                AccountsProgressNotes   = NotNull( detatils.AccountsProgressNotes)
            };

            return res;
        }
        private static VATDetails Convert(AMScripts.VATString detatils, Client client)
        {
            VATDetails res = new VATDetails(client)
            {
                RecordsReceived      = NotNull( detatils.ReccordsRecieved),
                ProgressNotes        = NotNull( detatils.ProgressNotes),
                VATNumber            = NotNull( detatils.VATNumber),
                VATAddress           = NotNull( detatils.VATAddress),
                DateOfRegistration   = NotNull( detatils.DateOfRegistration),
                EffectiveDate        = NotNull( detatils.EffectiveDate),
                AppliedForMTD        = NotNull( detatils.AppliedForMTD),
                MTDReady             = detatils.MTDReady,
                DirectDebit          = detatils.DirectDebit,
                StandardScheme       = detatils.StandardScheme,
                CashAccountingScheme = detatils.CashAccountingScheme,
                FlatRate             = detatils.FlatRate,
                FlatRateCategory     = NotNull( detatils.FlatRateCategory),
                GeneralNotes         = NotNull( detatils.GeneralNotesVAT),
            };
            return res;
        }
        private static PAYEDetails Convert(AMScripts.PAYEString detatils, Client client)
        {
            PAYEDetails res = new PAYEDetails(client)
            {
                EmployersReference                = NotNull( detatils.EmployersRefernce),
                AccountsOfficeReference           = NotNull( detatils.AccountsOfficeReference),
                PAYESchemeCeased                  = NotNull( detatils.PAYESchemeCeased),
                PAYERecordsReceived               = NotNull( detatils.PAYEReccordsRecieved),
                GeneralNotes                      = NotNull( detatils.GeneralNotesPAYE),
                AutoEnrolmentRecordsRecieved      = NotNull( detatils.AutoEnrolmentRecordsReceived),
                AutoEnrolmentProgressNote         = NotNull( detatils.AutoEnrolmentProgressNote),
                AutoEnrolmentStaging              = NotNull( detatils.AutoEnrolmentStaging),
                PostponemnetDate                  = NotNull( detatils.PostponementDate),
                PensionsRegulatorOptOutDate       = NotNull( detatils.ThePensionsRegulatorOptOutDate),
                ReEnrolmentDate                   = NotNull( detatils.ReEnrolmentDate),
                PensionProvider                   = NotNull( detatils.PensionProvider),
                PensionID                         = NotNull( detatils.PensionID),
                DeclarationOfComplianceDue        = NotNull( detatils.DeclarationOfComplianceDue),
                DeclarationOfComplianceSubmission = NotNull( detatils.DeclarationOfComplianceSubmission),
                P11DRecordsReceived               = NotNull( detatils.P11DRecordsReceived),
                P11DProgressNote                  = NotNull( detatils.P11DProgressNote)
            };
            return res;
            // the Payroll
            int i = 0;
            foreach(AMScripts.PAYEFrequencyString payrol in detatils.PAYEFrequencies)
            {
                if (string.IsNullOrEmpty(payrol.FistPayDate))
                    continue;
                res.PayRolls.Add(new PayRoll(client)
                {
                    Date = NotNull( payrol.FistPayDate),
                    NumberOfEmployees = NotNull( payrol.NumberOfEmployees),
                    RTIDeadline = NotNull(payrol.RTIDeadline)
                });
                switch (i++)
                {
                    case 0: // weekly
                        res.PayRolls.Last().Type = "Weekly";
                        break;
                    case 1: // 2weekly
                        res.PayRolls.Last().Type = "2Weekly";
                        break;
                    case 2: // monthly
                        res.PayRolls.Last().Type = "Monthly";
                        break;
                }
            }
            return res;
        }

        private static string NotNull(string value)
        {
            return value == null ? "" : value;
        }
    }
}
