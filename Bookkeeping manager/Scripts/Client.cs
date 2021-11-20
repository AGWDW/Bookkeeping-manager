using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Bookkeeping_manager.Scripts
{
    public abstract class ClientDetailsBase
    {
        abstract public byte[] GetControlTypes();
        [BsonIgnore]
        public Client Client;
    }
    public class Client : MongoObject, INotifyPropertyChanged
    {
        [BsonIgnore]
        public Tasks.TaskGroup APETasks { get; set; }
        [BsonIgnore]
        public Tasks.TaskGroup VATPETasks { get; set; }
        [BsonIgnore]
        public Tasks.TaskGroup CATasks { get; set; }
        [BsonIgnore]
        public Dictionary<string, Tasks.TaskGroup> AMLTasks { get; set; }
        // public Tasks.TaskGroup AMLTasks { get; set; }
        [BsonIgnore]
        public Tasks.TaskGroup SATasks { get; set; }
        [BsonIgnore]
        public Tasks.TaskGroup P11DTasks { get; set; }
        [BsonIgnore]
        public Tasks.TaskGroup CISWTasks { get; set; }
        [BsonIgnore]
        public Tasks.TaskGroup CISSTasks { get; set; }
        [BsonIgnore]
        public Dictionary<string, Tasks.TaskGroup> PayrollTasks { get; set; }
        private bool change = false;
        [BsonIgnore]
        public bool Changed
        {
            get => change;
            set
            {
                if (value)
                {
                    _ = 0;
                }
                else
                {
                    _ = 0;
                }
                change = value;
            }
        }
        [BsonIgnore]
        public bool Delete { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string comments;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CompanyDetails companyDetails;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ContactDetials contactDetails;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Accountant accountant;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Services services;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AccountsReturns accountsReturns;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private VATDetails vATDetails;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PAYEDetails pAYEDetails;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Document> documents;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CISDetails cISDetails;
        public string Name
        {
            get => name;
            set
            {
                if (value == name)
                    return;
                string n = name;
                /*DataHandler.AllEvents.ForEach(e =>
                {
                    if (e.DisplayName.Contains($"({Name})"))
                    {
                        e.DisplayName = e.DisplayName.Replace($"({Name})", $"({value})");
                    }
                });*/
                // the renaming of tasks
                for (int i = 0; i < DataHandler.AllTasks.Length; i++)
                {
                    if (DataHandler.AllTasks[i].ClientName == name)
                    {
                        DataHandler.AllTasks[i].ClientName = value;
                        DataHandler.AllTasks[i].HasChanged = true;
                    }
                }
                name = value;
                SetNamesBasic();
                OnPropertyChanged(n);
            }
        }
        public string Comments
        {
            get => comments;
            set
            {
                if (value == comments)
                {
                    return;
                }

                string c = comments;
                comments = value;
                OnPropertyChanged(c);
            }
        }
        [BsonIgnore]
        public string[] Categories
        {
            get => new string[]
            {
                "Client Details",
                "Contact Details",
                "Accountant",
                "Services",
                "Account and returns",
                "VAT Details",
                "CIS Details",
                "PAYE Details",
                "Documents",
            };
        }
        public CompanyDetails CompanyDetails
        {
            get
            {
                Changed = true;
                return companyDetails;
            }
            set
            {
                if (value == companyDetails)
                    return;
                var c = companyDetails;
                companyDetails = value;
                OnPropertyChanged(c);
            }
        }
        public ContactDetials ContactDetials
        {
            get
            {
                Changed = true;
                return contactDetails;
            }
            set
            {
                if (value == contactDetails)
                    return;
                var c = contactDetails;
                contactDetails = value;
                OnPropertyChanged(c);
            }
        }
        public Accountant Accountant
        {
            get
            {
                Changed = true;
                return accountant;
            }
            set
            {
                if (value == accountant)
                    return;
                var c = accountant;
                accountant = value;
                OnPropertyChanged(c);
            }
        }
        public Services Services
        {
            get
            {
                Changed = true;
                return services;
            }
            set
            {
                if (value == services)
                    return;
                var c = services;
                services = value;
                OnPropertyChanged(c);
            }
        }
        public VATDetails VATDetails
        {
            get
            {
                Changed = true;
                return vATDetails;
            }
            set
            {
                if (value == vATDetails)
                    return;
                var c = vATDetails;
                vATDetails = value;
                OnPropertyChanged(c);
            }
        }
        public AccountsReturns AccountsReturns
        {
            get
            {
                Changed = true;
                return accountsReturns;
            }
            set
            {
                if (value == accountsReturns)
                    return;
                var c = accountsReturns;
                accountsReturns = value;
                OnPropertyChanged(c);
            }
        }
        public CISDetails CISDetails
        {
            get
            {
                Changed = true;
                return cISDetails;
            }
            set
            {
                if (value == cISDetails)
                    return;
                var c = cISDetails;
                cISDetails = value;
                OnPropertyChanged(c);
            }
        }
        public PAYEDetails PAYEDetails
        {
            get
            {
                Changed = true;
                return pAYEDetails;
            }
            set
            {
                if (value == pAYEDetails)
                    return;
                var c = pAYEDetails;
                pAYEDetails = value;
                OnPropertyChanged(c);
            }
        }
        [BsonIgnore]
        public List<Document> Documents
        {
            get
            {
                Changed = true;
                return documents;
            }
            set
            {
                if (value == documents)
                    return;
                var c = documents;
                documents = value;
                OnPropertyChanged(c);
            }
        }
        public Client(string name)
        {
            Changed = true;
            Delete = false;
            this.name = name;
            comments = "";
            companyDetails = new CompanyDetails(this);
            contactDetails = new ContactDetials(this, new Contact(this, 0));
            accountant = new Accountant(this);
            services = new Services(this);
            accountsReturns = new AccountsReturns(this);
            vATDetails = new VATDetails(this);
            cISDetails = new CISDetails(this);
            pAYEDetails = new PAYEDetails(this);
            documents = new List<Document>();
        }
        private void SetNamesBasic()
        {
            if (contactDetails is null) return;
            companyDetails.Client = this;
            contactDetails.Client = this;
            accountant.Client = this;
            services.Client = this;
            accountsReturns.Client = this;
            vATDetails.Client = this;
            cISDetails.Client = this;
            pAYEDetails.Client = this;
        }
        public void SetNames()
        {
            companyDetails.Client = this;
            contactDetails.Client = this;
            accountant.Client = this;
            services.Client = this;
            accountsReturns.Client = this;
            vATDetails.Client = this;
            cISDetails.Client = this;
            pAYEDetails.Client = this;
            if (documents is null)
            {
                documents = new List<Document>();
            }
            Changed = false;
            Delete = false;
        }
        public override string ToString()
        {
            return $"{Name} : Changed: {Changed}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Changed = true;
        }
    }

    // has tasks
    public class CompanyDetails : ClientDetailsBase, INotifyPropertyChanged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string companyNumber, charityNumber, incorpDate, condirStateDate, tradingAs, regAdress,
           postalAdress, email, phone, sic, nature, utr, chAuth;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool confirmation;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] softwares;

        public CompanyDetails(Client client)
        {
            Client = client;
            companyNumber = charityNumber = incorpDate = condirStateDate = tradingAs = regAdress =
            postalAdress = email = phone = sic = nature = utr = chAuth = "";
            confirmation = false;
            softwares = new string[4]
            {
                "0",
                "",
                "Xero",
                "Quickbooks"
            };
        }

        #region Poropeties
        public string CompanyNumber
        {
            get => companyNumber;
            set
            {
                if (companyNumber != value)
                {
                    string c = companyNumber;
                    companyNumber = DataEnforce.CompanysHouseNumber(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CharityNumber
        {
            get => charityNumber;
            set
            {
                if (charityNumber != value)
                {
                    string c = charityNumber;
                    charityNumber = DataEnforce.CharityNumber(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string IncorporationDate
        {
            get => incorpDate;
            set
            {
                if (incorpDate != value)
                {
                    string c = incorpDate;
                    incorpDate = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        // has tasks & has no dependencys
        public bool ConfirmationEnabled
        {
            get => confirmation;
            set
            {
                if (confirmation != value)
                {
                    bool c = confirmation;
                    confirmation = value;
                    if (!DataHandler.AllowSet)
                    {
                        return;
                    }
                    //string[] names = new string[2]
                    //{
                    //    $"Confirmation statement due ({Client.Name})",
                    //    $"Submit Confirmation ({Client.Name})"
                    //};
                    //if (!DataHandler.AllowSet)
                    //    return;
                    if (confirmation && condirStateDate != "" && condirStateDate != null)
                    {
                        DateTime cs = condirStateDate.ToDate();
                        /*Event e = DataHandler.AddEvent(new Event(name: names[0], colourType: "CA", initalDate: cs, canBeLate: false));
                        Interval interval = new Interval(1, 0, 0);
                        e.SetIntervals(interval);
                        e.SetBinding(this, "ConfirmationStatementDate", "Client", "CompanyDetails");
                        e.UpdateBinding += () =>
                        {
                            ConfirmationEnabled = true;
                        };

                        // should apear for 10 days
                        e = DataHandler.AddEvent(new Event(name: names[1], colourType: "CA", initalDate: cs, showPeriod: 10));
                        interval = new Interval(1, 0, 0);
                        e.SetIntervals(interval);*/

                        #region Tasks -- New --
                        Tasks.TaskGroup taskGroup = Tasks.TaskGroup.CreateCA(Client.Name, cs);
                        DataHandler.AddTask(taskGroup);
                        Client.CATasks = taskGroup;
                        #endregion
                    }
                    else
                    {
                        //DataHandler.AllEvents.RemoveAll(e => names.Contains(e.DisplayName));
                    }
                    OnPropertyChanged(c);
                }
            }
        }
        public string ConfirmationStatementDate
        {
            get
            {
                if (Client.CATasks != null)
                {
                    string d = Client.CATasks.Parent.Date.GetString();
                    if (d != condirStateDate)
                    {
                        condirStateDate = d;
                        // has no dependencys
                    }
                }
                return condirStateDate;
            }
            set
            {
                if (condirStateDate != value)
                {
                    string c = condirStateDate;
                    condirStateDate = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string CompanyTradingAs
        {
            get => tradingAs;
            set
            {
                if (tradingAs != value)
                {
                    string c = tradingAs;
                    tradingAs = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string RegisteredAddress
        {
            get => regAdress;
            set
            {
                if (regAdress != value)
                {
                    string c = regAdress;
                    regAdress = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CompanyPostalAdress
        {
            get => postalAdress;
            set
            {
                if (postalAdress != value)
                {
                    string c = postalAdress;
                    postalAdress = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CompanyEmail
        {
            get => email;
            set
            {
                if (email != value)
                {
                    string c = email;
                    email = DataEnforce.Email(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string CompanyTelephone
        {
            get => phone;
            set
            {
                if (phone != value)
                {
                    string c = phone;
                    phone = DataEnforce.PhoneNumber(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string SICCode
        {
            get => sic;
            set
            {
                if (sic != value)
                {
                    string c = sic;
                    sic = DataEnforce.SICCode(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string NatureOfBusiness
        {
            get => nature;
            set
            {
                if (nature != value)
                {
                    string c = nature;
                    nature = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CompanyUTR
        {
            get => utr;
            set
            {
                if (utr != value)
                {
                    string c = utr;
                    utr = DataEnforce.UTR(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string CHAuthenticationCode
        {
            get => chAuth;
            set
            {
                if (chAuth != value)
                {
                    string c = chAuth;
                    chAuth = DataEnforce.CompanysHouseAuthCode(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string[] MainSoftware
        {
            get => softwares;
            set
            {
                if (softwares != value)
                {
                    var c = softwares;
                    softwares = value;
                    OnPropertyChanged(c);
                }
            }
        }
        #endregion

        public override byte[] GetControlTypes()
        {
            return new byte[15]
            {
                1, 1, 1, 6, 1, 1,
                2, 2, 1,
                1, 1, 1, 1, 1,
                7
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Client is null) return;
            Client.Changed = true;
        }

    }
    // has tasks
    public class Contact : ClientDetailsBase, INotifyPropertyChanged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string position, title, fName, mName, lName, dob, email, address, phone, niNum, utr, termsSigned,
            aml, photo, adressVer, amlForm, amlRev, married, nationality;
        private string name;
        // renames in the dictionary and the task 
        public string Name
        {
            get => name;
            set
            {
                if (Client is null || Client.AMLTasks is null)
                {
                    name = value;
                    return;
                }
                if (name != value)
                {
                    if (Client.AMLTasks.ContainsKey(name))
                    {
                        var temp = Client.AMLTasks[name];
                        temp.RenameAMLTask(value);
                        Client.AMLTasks.Add(value, temp);
                        Client.AMLTasks.Remove(name);
                        string prev = name;
                        OnPropertyChanged(prev);
                    }
                }
                name = value;
            }
        }
        private readonly int index;
        // has tasks
        public Contact(Client client, int index)
        {
            this.index = index;
            Client = client;
            position = title = fName = mName = lName = dob = email = address = phone = niNum = utr = termsSigned =
            aml = photo = adressVer = amlForm = amlRev = married = nationality = "";
            name = "Empty Contact";
        }

        #region Properties

        public string Position
        {
            get => position;
            set
            {
                if (position != value)
                {
                    string c = position;
                    position = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    string c = title;
                    title = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string FirstName
        {
            get => fName;
            set
            {
                if (fName != value)
                {
                    string c = fName;
                    fName = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string MiddleName
        {
            get => mName;
            set
            {
                if (mName != value)
                {
                    string c = mName;
                    mName = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string LastName
        {
            get => lName;
            set
            {
                if (lName != value)
                {
                    string c = lName;
                    lName = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string DateOfBirth
        {
            get => dob;
            set
            {
                if (dob != value)
                {
                    string c = dob;
                    dob = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string EmailAddress
        {
            get => email;
            set
            {
                if (email != value)
                {
                    string c = email;
                    email = DataEnforce.Email(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string PostalAddress
        {
            get => address;
            set
            {
                if (address != value)
                {
                    string c = address;
                    address = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string PhoneNumber
        {
            get => phone;
            set
            {
                if (phone != value)
                {
                    string c = phone;
                    phone = DataEnforce.PhoneNumber(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string NINumber
        {
            get => niNum;
            set
            {
                if (niNum != value)
                {
                    string c = niNum;
                    niNum = DataEnforce.NI(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string PersonalUTRNmber
        {
            get => utr;
            set
            {
                if (utr != value)
                {
                    string c = utr;
                    utr = DataEnforce.UTR(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string TermsSigned
        {
            get => termsSigned;
            set
            {
                if (termsSigned != value)
                {
                    string c = termsSigned;
                    termsSigned = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string AMLPapperworkSigned
        {
            get => aml;
            set
            {
                if (aml != value)
                {
                    string c = aml;
                    aml = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string PhotoIDReadReceived
        {
            get => photo;
            set
            {
                if (photo != value)
                {
                    string c = photo;
                    photo = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string AddressReadReceived
        {
            get => adressVer;
            set
            {
                if (adressVer != value)
                {
                    string c = adressVer;
                    adressVer = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string AMLFormalCheckCompleted
        {
            get => amlForm;
            set
            {
                if (amlForm != value)
                {
                    string c = amlForm;
                    amlForm = DataEnforce.Date(amlForm, value, true);
                    OnPropertyChanged(c);
                }
            }
        }
        // has tasks & no dependencys
        public string AMLReviewDue
        {
            get
            {
                if (Client.AMLTasks != null)
                {
                    if (!Client.AMLTasks.ContainsKey(Name))
                    {
                        return amlRev;
                    }
                    string d = Client.AMLTasks[Name].Parent.Date.GetString();
                    if (d != amlRev)
                    {
                        amlRev = d;
                    }
                }
                return amlRev;
            }
            set
            {
                if (amlRev != value)
                {
                    string c = amlRev;
                    amlRev = DataEnforce.Date(amlRev, value, true);
                    if (DataHandler.AllowSet && (amlRev == "" || amlRev != c))
                    {
                        string[] names = new string[]
                        {
                            $"AML review due for {Name} ({Client.Name})"
                        };
                        if (amlRev == "")
                        {
                            //DataHandler.AllEvents.RemoveAll(e => names.Contains(e.DisplayName)); // removes all ape tasks
                            DataHandler.RemoveTask(Client.AMLTasks[Name]);
                            Client.AMLTasks = null;
                        }
                        else
                        {
                            /*Event e = DataHandler.AddEvent(new Event(name: names[0], colourType: "AML", initalDate: amlRev.ToDate()));
                            e.SetIntervals(new Interval(1, 0, 0));
                            e.SetBinding(DataHandler.AllClients.Find(cl => cl.Name == Client.Name).ContactDetials, "AMLReviewDue", "Client", "ContactDetials");
                            e.UpdateBinding = () =>
                            {
                                (e.Binding as ContactDetials)[index].AMLReviewDue = e.Date.GetString();
                            };*/


                            #region Tasks -- New --
                            Tasks.TaskGroup taskGroup = Tasks.TaskGroup.CreateAML(Client.Name, amlRev.ToDate(), Name);
                            DataHandler.AddTask(taskGroup);
                            if (Client.AMLTasks is null)
                            {
                                Client.AMLTasks = new Dictionary<string, Tasks.TaskGroup>();
                            }
                            if (Client.AMLTasks.ContainsKey(Name))
                            {
                                Client.AMLTasks[Name] = taskGroup;
                            }
                            else
                            {
                                Client.AMLTasks.Add(Name, taskGroup);
                            }
                            #endregion
                        }
                    }
                    OnPropertyChanged(c);
                }
            }
        }
        public string MaritalStatus
        {
            get => married;
            set
            {
                if (married != value)
                {
                    string c = married;
                    married = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string Nationality
        {
            get => nationality;
            set
            {
                if (nationality != value)
                {
                    string c = nationality;
                    nationality = value;
                    OnPropertyChanged(c);
                }
            }
        }
        #endregion

        public override byte[] GetControlTypes()
        {
            return new byte[20]
            {
                0,0,0,0,0,0,
                0,2,0,0,0,0,
                0,3,3,0,0,0,0,0
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Client is null) return;
            Client.Changed = true;
        }

    }
    public class ContactDetials : ClientDetailsBase
    {
        private List<Contact> contacts;

        public List<Contact> Contacts
        {
            get
            {
                foreach (Contact contact in contacts)
                {
                    contact.Client = Client;
                }
                return contacts;
            }
            set => contacts = value;
        }
        public int Size { get => Contacts.Count; }
        public ContactDetials(Client client, params Contact[] contacts)
        {
            Client = client;
            Contacts = new List<Contact>(contacts);
        }
        public override byte[] GetControlTypes()
        {
            return default;
        }

        public Contact this[int i]
        {
            get => Contacts[i];
            set => Contacts[i] = value;
        }
    }
    public class Accountant : ClientDetailsBase, INotifyPropertyChanged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string name, email, phonenumber, notes;
        public Accountant(Client client)
        {
            Client = client;
            name = email = phonenumber = notes = "";
        }
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    string c = name;
                    name = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string ContactEmail
        {
            get => email;
            set
            {
                if (email != value)
                {
                    string c = email;
                    email = DataEnforce.Email(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string PhoneNumber
        {
            get => phonenumber;
            set
            {
                if (phonenumber != value)
                {
                    string c = phonenumber;
                    phonenumber = DataEnforce.PhoneNumber(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string Notes
        {
            get => notes;
            set
            {
                if (notes != value)
                {
                    string c = notes;
                    notes = value;
                    OnPropertyChanged(c);
                }
            }
        }

        public override byte[] GetControlTypes()
        {
            return new byte[4]
              {
                1, 1, 1, 2
              };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Client is null) return;
            Client.Changed = true;
        }
    }
    // has tasks
    public class Services : ClientDetailsBase, INotifyPropertyChanged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string bookkeeping, vat, management, payroll, autoEnrol, cis, bill, invoicing,
            accounts, ct600, selfAcc, confim, p11d, charaty, consultant, software, setup;
        private bool p11dEnabled, selfAsses;
        public Services(Client client)
        {
            Client = client;
            bookkeeping = vat = management = payroll = autoEnrol = cis = bill = invoicing =
            accounts = ct600 = selfAcc = confim = p11d = consultant = software = setup = charaty = "";
            p11dEnabled = selfAsses = false;
        }

        #region Properties
        public string Bookkeeping
        {
            get => bookkeeping;
            set
            {
                if (bookkeeping != value)
                {
                    string c = bookkeeping;
                    bookkeeping = DataEnforce.Money(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string VATReturns
        {
            get => vat;
            set
            {
                if (vat != value)
                {
                    string c = vat;
                    vat = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string ManagementReturns
        {
            get => management;
            set
            {
                if (management != value)
                {
                    string c = management;
                    management = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string Payroll
        {
            get => payroll;
            set
            {
                if (payroll != value)
                {
                    string c = payroll;
                    payroll = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string AutoEnrolment
        {
            get => autoEnrol;
            set
            {
                if (autoEnrol != value)
                {
                    string c = autoEnrol;
                    autoEnrol = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CIS
        {
            get => cis;
            set
            {
                if (cis != value)
                {
                    string c = cis;
                    cis = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string BillPayments
        {
            get => bill;
            set
            {
                if (bill != value)
                {
                    string c = bill;
                    bill = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string Invoicing
        {
            get => invoicing;
            set
            {
                if (invoicing != value)
                {
                    string c = invoicing;
                    invoicing = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string AccountsPreparation
        {
            get => accounts;
            set
            {
                if (accounts != value)
                {
                    string c = accounts;
                    accounts = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CT600Returns
        {
            get => ct600;
            set
            {
                if (ct600 != value)
                {
                    string c = ct600;
                    ct600 = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        // has tasks
        public bool SelfAssessmentEnabled
        {
            get => selfAsses;
            set
            {
                if (selfAsses != value)
                {
                    bool c = selfAsses;
                    selfAsses = value;
                    if (!DataHandler.AllowSet)
                        return;
                    string[] names = new string[4]
                    {
                        $"Self assessment due({Client.Name})",
                        $"File S.A ({Client.Name})",
                        $"Request Info for S.A ({Client.Name})",
                        $"Prepare S.A ({Client.Name})"
                    };
                    if (selfAsses)
                    {
                        DateTime sa = new DateTime(DateTime.Now.Year, 1, 31);
                        /*Event e = DataHandler.AddEvent(new Event(name: names[0], colourType: "SA",
                            initalDate: sa, canBeLate: false));
                        Interval interval = new Interval(1, 0, 0);
                        e.SetIntervals(interval);

                        e = DataHandler.AddEvent(new Event(name: names[1], colourType: "SA",
                            initalDate: sa.SetDay(5)));
                        interval = new Interval(1, 0, 0);
                        e.SetIntervals(interval);

                        e = DataHandler.AddEvent(new Event(name: names[2], colourType: "SA",
                            initalDate: new DateTime(DateTime.Now.Year - 1, 5, 15)));
                        interval = new Interval(1, 0, 0);
                        e.SetIntervals(interval);

                        e = DataHandler.AddEvent(new Event(name: names[3], colourType: "SA",
                            initalDate: new DateTime(DateTime.Now.Year - 1, 9, 15)));
                        interval = new Interval(1, 0, 0);
                        e.SetIntervals(interval);*/

                        #region Tasks -- New --
                        Tasks.TaskGroup taskGroup = Tasks.TaskGroup.CreateSA(Client.Name);
                        DataHandler.AddTask(taskGroup);
                        Client.SATasks = taskGroup;
                        #endregion
                    }
                    else
                    {
                        //DataHandler.AllEvents.RemoveAll(e => names.Contains(e.DisplayName));
                        DataHandler.RemoveTask(Client.SATasks);
                    }
                    OnPropertyChanged(c);
                }
            }
        }
        public string SelfAssessment
        {
            get => selfAcc;
            set
            {
                if (selfAcc != value)
                {
                    string c = selfAcc;
                    selfAcc = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string ConfirmationStatement
        {
            get => confim;
            set
            {
                if (confim != value)
                {
                    string c = confim;
                    confim = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        // has tasks
        public bool P11DEnabled
        {
            get => p11dEnabled;
            set
            {
                if (p11dEnabled != value)
                {
                    bool c = p11dEnabled;
                    p11dEnabled = value;
                    if (!DataHandler.AllowSet)
                        return;
                    string[] names = new string[2]
                    {
                        $"P11D due({Client.Name})",
                        $"Prepare P11D ({Client.Name})"
                    };
                    if (p11dEnabled)
                    {
                        DateTime p11 = new DateTime(DateTime.Now.Year, 7, 6);
                        /*Event e = DataHandler.AddEvent(new Event(name: names[0], colourType: "P11D", initalDate: p11, canBeLate: false));
                        Interval interval = new Interval(1, 0, 0);
                        e.SetIntervals(interval);

                        p11 = new DateTime(DateTime.Now.Year, 5, 6);
                        e = DataHandler.AddEvent(new Event(name: names[1], colourType: "P11D", initalDate: p11.AddMonths(-2)));
                        interval = new Interval(1, 0, 0);
                        e.SetIntervals(interval);*/

                        #region Tasks -- New --
                        Tasks.TaskGroup taskGroup = Tasks.TaskGroup.CreateP11D(Client.Name);
                        DataHandler.AddTask(taskGroup);
                        Client.P11DTasks = taskGroup;
                        #endregion

                    }
                    else
                    {
                        //DataHandler.AllEvents.RemoveAll(e => names.Contains(e.DisplayName));
                        DataHandler.RemoveTask(Client.P11DTasks);
                    }
                    OnPropertyChanged(c);
                }
            }
        }
        public string P11D
        {
            get => p11d;
            set
            {
                if (p11d != value)
                {
                    string c = p11d;
                    p11d = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CharitiesCommission
        {
            get => charaty;
            set
            {
                if (charaty != value)
                {
                    string c = charaty;
                    charaty = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string ConsultationAdvice
        {
            get => consultant;
            set
            {
                if (consultant != value)
                {
                    string c = consultant;
                    consultant = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string Software
        {
            get => software;
            set
            {
                if (software != value)
                {
                    string c = software;
                    software = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CompanySetup
        {
            get => setup;
            set
            {
                if (setup != value)
                {
                    string c = setup;
                    setup = DataEnforce.Money(c, value); ;
                    OnPropertyChanged(c);
                }
            }
        }
        #endregion

        public override byte[] GetControlTypes()
        {
            return new byte[19]
            {
                1, 1, 1, 1,
                1, 1, 1, 1, 1, 1,
                6, 1, 1, 6, 1,
                1, 1, 1, 1
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Client is null) return;
            Client.Changed = true;
        }
    }
    // has tasks
    public class AccountsReturns : ClientDetailsBase, INotifyPropertyChanged
    {
        private string ape, chAND, ct600, tDHMRCYE, chAND_f, ct600_f, tDHMRCYE_f, ctPR, arr, aPN;
        public AccountsReturns(Client client)
        {
            Client = client;
            ape = chAND = ct600 = tDHMRCYE = chAND_f = ct600_f = tDHMRCYE_f = ctPR = arr = aPN = "";
        }
        #region Properties
        // has tasks
        public string AccountsPeriodEnd
        {
            get
            {
                if (Client.APETasks != null)
                {
                    string d = Client.APETasks.Parent.Date.GetString();
                    if (d != ape)
                    {
                        ape = d;
                        #region Dependents
                        // Current
                        DateTime APE = ape.ToDate();
                        APE = APE.AddYears(-1);

                        CH_0AccountsNextDue = APE.AddMonths(9).GetLastDay().GetString();
                        CT600Due = APE.AddYears(1).GetLastDay().AddDays(-1).GetString();
                        HMRCYearEnd = APE.AddMonths(10).GetFirstDay().GetString();
                        // Future

                        CH_0AccountsNextDue_ = CH_0AccountsNextDue.ToDate().AddYears(1).GetString();
                        CT600Due_ = CT600Due.ToDate().AddYears(1).GetString();
                        HMRCYearEnd_ = HMRCYearEnd.ToDate().AddYears(1).GetString();
                        #endregion
                    }
                }
                return ape;
            }
            set
            {
                if (ape != value)
                {
                    string c = ape;
                    ape = DataEnforce.LastDay(c, DataEnforce.Date(c, value));
                    if (DataHandler.AllowSet && (ape == "" || ape != c))
                    {
                        CH_0AccountsNextDue = CT600Due = HMRCYearEnd = "";
                        CH_0AccountsNextDue_ = CT600Due_ = HMRCYearEnd_ = "";
                        string[] names = new string[]
                        {
                                $"Request accounts info ({Client.Name})",
                                $"Start to prepare accounts ({Client.Name})",  $"Urgent prepare accounts ({Client.Name})",
                                $"VERY urgent prepare accounts ({Client.Name})", $"CT600 due ({Client.Name})",
                                $"Last date for filing accounts ({Client.Name})",
                                $"Tax due HMRC ({Client.Name})", $"Year end ({Client.Name})"
                        };
                        if (ape == "")
                        {
                            //DataHandler.AllEvents.RemoveAll(e => names.Contains(e.DisplayName)); // removes all ape tasks
                        }
                        if (ape != "")
                        {
                            var APE = ape.ToDate();
                            //#region Events
                            //// Year end 
                            //string name = $"Year end ({Client.Name})";
                            //Event rootEvent = DataHandler.AddEvent(new Event(name: name, initalDate: APE, colourType: "APE", canBeLate: true));
                            //rootEvent.SetIntervals(new Interval(1, 0, 0)); // should be year interval
                            //rootEvent.SetBinding(this, "AccountsPeriodEnd", "Client", "AccountsReturns");


                            //// Request Accounts  ------ doesnt work
                            //name = $"Request accounts info ({Client.Name})";
                            //Event e = DataHandler.AddEvent(new Event(name: name, initalDate: APE.AddYears(-1).AddMonths(1).GetLastDay(), colourType: "APE"));
                            //Interval interval = new Interval(1, 0, 0) // should be year interval
                            //{
                            //    ForceLastDayOfMonth = true
                            //};
                            //e.SetIntervals(interval);


                            //// Start to prepare accounts
                            //name = $"Start to prepare accounts ({Client.Name})";
                            //e = DataHandler.AddEvent(new Event(name: name, initalDate: APE.AddYears(-1).AddMonths(4).SetDay(14), colourType: "APE"));
                            //interval = new Interval(1, 0, 0) // should be year interval
                            //{
                            //    ForceDate = 14
                            //};
                            //e.SetIntervals(interval);

                            //// Urgent to prepare accounts
                            //name = $"Urgent prepare accounts ({Client.Name})";
                            //e = DataHandler.AddEvent(new Event(name: name, initalDate: APE.AddYears(-1).AddMonths(7).GetLastDay(), colourType: "APE"));
                            //interval = new Interval(1, 0, 0) // should be year interval
                            //{
                            //    ForceLastDayOfMonth = true
                            //};
                            //e.SetIntervals(interval);

                            //// VERY Urgent to prepare accounts
                            //name = $"VERY urgent prepare accounts ({Client.Name})";
                            //e = DataHandler.AddEvent(new Event(name: name, initalDate: APE.AddYears(-1).AddMonths(8).GetLastDay(), colourType: "APE"));
                            //interval = new Interval(1, 0, 0) // should be year interval
                            //{
                            //    ForceLastDayOfMonth = true
                            //};
                            //e.SetIntervals(interval);

                            //// CT600 due
                            //name = $"CT600 due ({Client.Name})";
                            //e = DataHandler.AddEvent(new Event(name: name, initalDate: APE.GetLastDay().AddDays(-1), colourType: "APE"));
                            //interval = new Interval(1, 0, 0); // should be year interval
                            //e.SetIntervals(interval);
                            //e.Advance(APE);

                            //// Last filling date
                            //name = $"Last date for filing accounts ({Client.Name})";
                            //e = DataHandler.AddEvent(new Event(name: name, initalDate: APE.AddYears(-1).GetLastDay(), colourType: "APE"));
                            //interval = new Interval(1, 0, 0) // should be year interval
                            //{
                            //    ForceLastDayOfMonth = true
                            //};
                            //e.SetIntervals(interval);
                            //e.Advance(APE);

                            //// Tax due HMRC
                            //name = $"Tax due HMRC ({Client.Name})";
                            //e = DataHandler.AddEvent(new Event(name: name, initalDate: APE.AddMonths(10).GetFirstDay(), colourType: "APE"));
                            //interval = new Interval(1, 0, 0) // should be year interval
                            //{
                            //    ForceFirstDayOfMonth = true
                            //};
                            //e.SetIntervals(interval);
                            //e.Advance(APE);
                            //#endregion

                            #region Dependents
                            // Current
                            APE = APE.AddYears(-1);

                            CH_0AccountsNextDue = APE.AddMonths(9).GetLastDay().GetString();
                            CT600Due = APE.AddYears(1).GetLastDay().AddDays(-1).GetString();
                            HMRCYearEnd = APE.AddMonths(10).GetFirstDay().GetString();
                            // Future          
                            APE = ape.ToDate();

                            CH_0AccountsNextDue_ = CH_0AccountsNextDue.ToDate().AddYears(1).GetString();
                            CT600Due_ = CT600Due.ToDate().AddYears(1).GetString();
                            HMRCYearEnd_ = HMRCYearEnd.ToDate().AddYears(1).GetString();
                            #endregion


                            #region Tasks -- New --
                            Tasks.TaskGroup taskGroup = Tasks.TaskGroup.CreateAPE(Client.Name, ape.ToDate());
                            DataHandler.AddTask(taskGroup);
                            Client.APETasks = taskGroup;
                            #endregion

                        }
                    }
                    OnPropertyChanged(c);
                }
            }
        }
        public string CurrentWork { get => ""; }
        public string CH_0AccountsNextDue
        {
            get => chAND;
            set
            {
                if (chAND != value)
                {
                    string c = chAND;
                    chAND = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CT600Due
        {
            get => ct600;
            set
            {
                if (ct600 != value)
                {
                    string c = ct600;
                    ct600 = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string HMRCYearEnd
        {
            get => tDHMRCYE;
            set
            {
                if (tDHMRCYE != value)
                {
                    string c = tDHMRCYE;
                    tDHMRCYE = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string FutureWork { get => ""; }
        public string CH_0AccountsNextDue_
        {
            get => chAND_f;
            set
            {
                if (chAND_f != value)
                {
                    string c = chAND_f;
                    chAND_f = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string CT600Due_
        {
            get => ct600_f;
            set
            {
                if (ct600_f != value)
                {
                    string c = ct600_f;
                    ct600_f = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string HMRCYearEnd_
        {
            get => tDHMRCYE_f;
            set
            {
                if (tDHMRCYE_f != value)
                {
                    string c = tDHMRCYE_f;
                    tDHMRCYE_f = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string _ { get => ""; } // used for visuals
        public string CTPaymentReference
        {
            get => ctPR;
            set
            {
                if (ctPR != value)
                {
                    string c = ctPR;
                    ctPR = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string AccountsRecordsReceived
        {
            get => arr;
            set
            {
                if (arr != value)
                {
                    string c = arr;
                    arr = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string AccountsProgressNotes
        {
            get => aPN;
            set
            {
                if (aPN != value)
                {
                    string c = aPN;
                    aPN = value;
                    OnPropertyChanged(c);
                }
            }
        }
        #endregion
        public override byte[] GetControlTypes()
        {
            return new byte[13] {
                1, 4, 8, 8,
                8, 4, 5, 5,
                5, 4, 1, 1, 2
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Client is null) return;
            Client.Changed = true;
        }
    }
    // has tasks
    public class VATDetails : ClientDetailsBase, INotifyPropertyChanged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] vatFreq;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string vatPeriodEnd, nextReturnDate, reccordsRec, progNotes, vatNumber, vatAddress, dateOfReg, efectiveDate,
            appliedMTD, flatCategory, notes;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool mtd, direct, stand, cashAcc, flat;
        #region Properties
        public string[] VATFrequency
        {
            get => vatFreq;
            set
            {
                if (vatFreq != value)
                {
                    var c = vatFreq;
                    vatFreq = value;
                    OnPropertyChanged(c);
                }
                var c1 = vatPeriodEnd;
                VATPeriodEnd = "";
                VATPeriodEnd = c1;
            }
        }
        // has tasks
        public string VATPeriodEnd
        {
            get
            {
                if (Client.VATPETasks != null)
                {
                    string d = Client.VATPETasks.Parent.Date.GetString();
                    if (d != vatPeriodEnd)
                    {
                        vatPeriodEnd = d;
                        #region Dependency
                        NextReturnDate = vatPeriodEnd.ToDate().AddMonths(2).SetDay(7).GetString();
                        #endregion
                    }
                }
                return vatPeriodEnd;
            }
            set
            {
                if (vatPeriodEnd != value)
                {
                    string c = vatPeriodEnd;
                    if (value == "" && Client != null)
                    {
                        vatPeriodEnd = value;
                        NextReturnDate = "";
                        DataHandler.RemoveTask(Client.VATPETasks);
                        Client.VATPETasks = null;
                        OnPropertyChanged(c);
                        return;
                    }
                    vatPeriodEnd = DataEnforce.LastDay(c, DataEnforce.Date(c, value));
                    if (DataHandler.AllowSet && (vatPeriodEnd == "" || vatPeriodEnd != c))
                    {
                        NextReturnDate = "";
                        if (vatPeriodEnd == "")
                        {
                            string[] names = new string[]
                            {
                                $"VAT period end ({Client.Name})",
                                $"Request VAT info ({Client.Name})",
                                $"File VAT ({Client.Name})"
                            };
                            //DataHandler.AllEvents.RemoveAll(e => names.Contains(e.DisplayName)); // removes all vat tasks
                        }
                        else
                        {
                            DateTime VAT = vatPeriodEnd.ToDate();
                            int period = vatFreq[0] == "0" ? 3 : 1;
                            #region Events
                            //string name = $"VAT period end ({Client.Name})";
                            //// VAT period end
                            //Event e = DataHandler.AddEvent(new Event(name: name, initalDate: VAT, colourType: "VATPerEnd", canBeLate: false));
                            //e.SetBinding(this, "VATPeriodEnd", "Client", "VATDetails");
                            //Interval interval = new Interval(0, period, 0)
                            //{
                            //    ForceLastDayOfMonth = true
                            //};
                            //e.SetIntervals(interval);

                            //// Request VAT info
                            //name = $"Request VAT info ({Client.Name})";
                            //e = DataHandler.AddEvent(new Event(name: name, initalDate: VAT.AddMonths(1).SetDay(5), colourType: "VATReq"));
                            //interval = new Interval(0, period, 0)
                            //{
                            //    ForceDate = 5
                            //};
                            //e.SetIntervals(interval);

                            //// File VAT
                            //name = $"File VAT ({Client.Name})";
                            //e = DataHandler.AddEvent(new Event(name: name, initalDate: VAT.AddMonths(2).SetDay(7), colourType: "VATFile"));
                            //interval = new Interval(0, period, 0)
                            //{
                            //    ForceDate = 7
                            //};
                            //e.SetIntervals(interval);
                            #endregion

                            #region Dependency
                            NextReturnDate = VAT.AddMonths(2).SetDay(7).GetString();
                            #endregion

                            #region Tasks -- New --
                            Tasks.TaskGroup taskGroup = Tasks.TaskGroup.CreateVATPE(Client.Name, VAT, period);
                            DataHandler.AddTask(taskGroup);
                            Client.VATPETasks = taskGroup;
                            #endregion
                        }
                    }
                    OnPropertyChanged(c);
                }
            }
        }
        public string NextReturnDate
        {
            get => nextReturnDate;
            set
            {
                if (nextReturnDate != value)
                {
                    var c = nextReturnDate;
                    nextReturnDate = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string RecordsReceived
        {
            get => reccordsRec;
            set
            {
                if (reccordsRec != value)
                {
                    var c = reccordsRec;
                    reccordsRec = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string ProgressNotes
        {
            get => progNotes;
            set
            {
                if (progNotes != value)
                {
                    var c = progNotes;
                    progNotes = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string VATNumber
        {
            get => vatNumber;
            set
            {
                if (vatNumber != value)
                {
                    var c = vatNumber;
                    vatNumber = DataEnforce.VATNumber(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string VATAddress
        {
            get => vatAddress;
            set
            {
                if (vatAddress != value)
                {
                    var c = vatAddress;
                    vatAddress = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string DateOfRegistration
        {
            get => dateOfReg;
            set
            {
                if (dateOfReg != value)
                {
                    var c = dateOfReg;
                    dateOfReg = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string EffectiveDate
        {
            get => efectiveDate;
            set
            {
                if (efectiveDate != value)
                {
                    var c = efectiveDate;
                    efectiveDate = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string AppliedForMTD
        {
            get => appliedMTD;
            set
            {
                if (appliedMTD != value)
                {
                    var c = appliedMTD;
                    appliedMTD = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public bool MTDReady
        {
            get => mtd;
            set
            {
                if (mtd != value)
                {
                    var c = mtd;
                    mtd = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public bool DirectDebit
        {
            get => direct;
            set
            {
                if (direct != value)
                {
                    var c = direct;
                    direct = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public bool StandardScheme
        {
            get => stand;
            set
            {
                if (stand != value)
                {
                    var c = stand;
                    stand = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public bool CashAccountingScheme
        {
            get => cashAcc;
            set
            {
                if (cashAcc != value)
                {
                    var c = cashAcc;
                    cashAcc = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public bool FlatRate
        {
            get => flat;
            set
            {
                if (flat != value)
                {
                    var c = flat;
                    flat = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string FlatRateCategory
        {
            get => flatCategory;
            set
            {
                if (flatCategory != value)
                {
                    var c = flatCategory;
                    flatCategory = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string GeneralNotes
        {
            get => notes;
            set
            {
                if (notes != value)
                {
                    var c = notes;
                    notes = value;
                    OnPropertyChanged(c);
                }
            }
        }
        #endregion
        public VATDetails(Client client)
        {
            Client = client;
            vatPeriodEnd = nextReturnDate = reccordsRec = progNotes = vatNumber = vatAddress = dateOfReg = efectiveDate =
            appliedMTD = flatCategory = notes = "";
            mtd = direct = stand = cashAcc = flat = false;
            vatFreq = new string[3]
            {
                "0", "Quarterly", "Monthly"
            };
        }
        public override byte[] GetControlTypes()
        {
            return new byte[17]
            {
                7, 1, 5, 1, 2, 1, 2, 1, 1,
                1, 6, 6, 6, 6, 6, 1, 2
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Client is null) return;
            Client.Changed = true;
        }
    }
    // has taks
    public class PayRoll : ClientDetailsBase, INotifyPropertyChanged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string type, numEmply, rtiDead, date;
        private string[] day;
        #region Properties
        public string Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    var c = type;
                    type = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string[] IntervalType
        {
            get => day;
            set
            {
                if (day != value)
                {
                    var c = day;
                    day = value;
                    OnPropertyChanged(c);
                }
                string c1 = date;
                Date = "";
                Date = c1;
            }
        }
        // has tasks
        public string Date
        {
            get
            {
                if (Client.PayrollTasks != null)
                {
                    if (!Client.PayrollTasks.ContainsKey(Type))
                    {
                        return date;
                    }
                    string d = Client.PayrollTasks[Type].Parent.Date.GetString();
                    if (d != date)
                    {
                        date = d;
                    }
                }
                return date;
            }
            set
            {
                if (date != value)
                {
                    var c = date;
                    date = DataEnforce.Date(date, value);
                    if (DataHandler.AllowSet && (date == "" || date != c))
                    {
                        //Interval interval = new Interval();
                        if (date != "")
                        {
                            switch (IntervalType[0])
                            {
                                case "1": // 28th
                                    date = DataEnforce.Day(date, 28);
                                    break;
                                case "2": // every other friday
                                    date = DataEnforce.Friday(date);
                                    break;
                                case "3": // last day of month
                                    date = DataEnforce.LastDay(c, date);
                                    break;
                                case "4": // last friday of month
                                    date = DataEnforce.LastFriday(date);
                                    break;
                            }
                        }
                        string[] names = new string[]
                        {
                            $"Payroll {Type} ({Client.Name})",  $"Prepare Payroll {Type} ({Client.Name})",
                        };
                        if (date == "" || IntervalType[0] == "0" && Type != "Weekly")
                        {
                            //DataHandler.AllEvents.RemoveAll(e => names.Contains(e.DisplayName));
                            DataHandler.RemoveTask(Client.PayrollTasks[Type]);
                        }
                        else
                        {
                            DateTime DATE = date.ToDate();
                            /*Event e = DataHandler.AddEvent(new Event(name: $"Payroll {Type} ({Client.Name})", colourType: "Payroll", initalDate: DATE, canBeLate: false));
                            e.SetIntervals(interval);
                            e.SetBinding(0, "", "", "");
                            e.UpdateBinding = () =>
                            {
                                DataHandler.AddEvent(new Event(name: $"Prepare Payroll {type} ({Client.Name})", colourType: "Payroll", initalDate: e.Date.AddDays(-2)));
                            };
                            e.UpdateBinding();*/

                            #region Tasks -- New --
                            Tasks.TaskGroup taskGroup = Tasks.TaskGroup.CreatePayroll(Client.Name, DATE, Type, IntervalType[0]);
                            DataHandler.AddTask(taskGroup);
                            if (Client.PayrollTasks is null)
                            {
                                Client.PayrollTasks = new Dictionary<string, Tasks.TaskGroup>();
                            }
                            if (Client.PayrollTasks.ContainsKey(Type))
                            {
                                Client.PayrollTasks[Type] = taskGroup;
                            }
                            else
                            {
                                Client.PayrollTasks.Add(Type, taskGroup);
                            }
                            #endregion
                        }
                    }

                    OnPropertyChanged(c);
                }
            }
        }
        public string NumberOfEmployees
        {
            get => numEmply;
            set
            {
                if (numEmply != value)
                {
                    var c = numEmply;
                    numEmply = DataEnforce.Integer(numEmply, value).ToString();
                    OnPropertyChanged(c);
                }
            }
        }
        public string RTIDeadline
        {
            get => rtiDead;
            set
            {
                if (rtiDead != value)
                {
                    var c = rtiDead;
                    rtiDead = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        #endregion
        public PayRoll(Client client)
        {
            Client = client;
            // notifaction on task 2 days before
            type = "Weekly";
            date = numEmply = rtiDead = "";
            day = new string[]
            {
                "0",
                "NAN",
                "28th of the Month", "Every other friday",
                "Last day of the Month", "Last friday of the Month"
            };
        }
        public override byte[] GetControlTypes()
        {
            return new byte[4] { 7, 1, 1, 1 };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Client is null) return;
            Client.Changed = true;
        }
    }
    public class PAYEDetails : ClientDetailsBase, INotifyPropertyChanged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string emplRef, accOffRef, payeScheCeas, payeRecRec, notes, aerr, aepn, aes, postDate, penRegOOD, reEnDa, penProv, penID, decOfCompDue, decOfCompSub, np11dRD, p11dRR, p11dNotes;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<PayRoll> payRolls;
        #region Properties
        public string EmployersReference
        {
            get => emplRef;
            set
            {
                if (emplRef != value)
                {
                    var c = emplRef;
                    emplRef = DataEnforce.PAYEEmployer(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string AccountsOfficeReference
        {
            get => accOffRef;
            set
            {
                if (accOffRef != value)
                {
                    var c = accOffRef;
                    accOffRef = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public List<PayRoll> Payrols
        {
            get
            {
                payRolls.ForEach(p => p.Client = Client);
                return payRolls;
            }
            set
            {
                if (payRolls != value)
                {
                    var c = payRolls;
                    payRolls = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string PAYESchemeCeased
        {
            get => payeScheCeas;
            set
            {
                if (payeScheCeas != value)
                {
                    var c = payeScheCeas;
                    payeScheCeas = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string PAYERecordsReceived
        {
            get => payeRecRec;
            set
            {
                if (payeRecRec != value)
                {
                    var c = payeRecRec;
                    payeRecRec = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string GeneralNotes
        {
            get => notes;
            set
            {
                if (notes != value)
                {
                    var c = notes;
                    notes = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string AutoEnrolment { get => ""; } //##########################################################################################################
        public string AutoEnrolmentRecordsRecieved
        {
            get => aerr;
            set
            {
                if (aerr != value)
                {
                    var c = aerr;
                    aerr = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string AutoEnrolmentProgressNote
        {
            get => aepn;
            set
            {
                if (aepn != value)
                {
                    var c = aepn;
                    aepn = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string AutoEnrolmentStaging
        {
            get => aes;
            set
            {
                if (aes != value)
                {
                    var c = aes;
                    aes = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string PostponementDate
        {
            get => postDate;
            set
            {
                if (postDate != value)
                {
                    var c = postDate;
                    postDate = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string PensionsRegulatorOptOutDate
        {
            get => penRegOOD;
            set
            {
                if (penRegOOD != value)
                {
                    var c = penRegOOD;
                    penRegOOD = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string ReEnrolmentDate
        {
            get => reEnDa;
            set
            {
                if (reEnDa != value)
                {
                    var c = reEnDa;
                    reEnDa = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string PensionProvider
        {
            get => penProv;
            set
            {
                if (penProv != value)
                {
                    var c = penProv;
                    penProv = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string PensionID
        {
            get => penID;
            set
            {
                if (penID != value)
                {
                    var c = penID;
                    penID = value;
                    OnPropertyChanged(c);
                }
            }
        }
        public string DeclarationOfComplianceDue
        {
            get => decOfCompDue;
            set
            {
                if (decOfCompDue != value)
                {
                    var c = decOfCompDue;
                    decOfCompDue = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string DeclarationOfComplianceSubmission
        {
            get => decOfCompSub;
            set
            {
                if (decOfCompSub != value)
                {
                    var c = decOfCompSub;
                    decOfCompSub = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string P11D { get => ""; } //####################################################################################################################
        public string NextP11DReturnDue
        {
            get
            {
                np11dRD = new DateTime(DateTime.Today.Year, 6, 7).GetString();
                return np11dRD;
            }
            set { }
        }
        public string P11DRecordsReceived
        {
            get => p11dRR;
            set
            {
                if (p11dRR != value)
                {
                    var c = p11dRR;
                    p11dRR = DataEnforce.Date(c, value);
                    OnPropertyChanged(c);
                }
            }
        }
        public string P11DProgressNote
        {
            get => p11dNotes;
            set
            {
                if (p11dNotes != value)
                {
                    var c = p11dNotes;
                    p11dNotes = value;
                    OnPropertyChanged(c);
                }
            }
        }
        #endregion

        public PAYEDetails(Client client)
        {
            Client = client;
            emplRef = accOffRef = payeScheCeas = payeRecRec = notes = aerr = aepn = aes = postDate = penRegOOD = reEnDa = penProv = penID = decOfCompDue = decOfCompSub = np11dRD = p11dRR = p11dNotes = "";
            payRolls = new List<PayRoll>();
        }
        public override byte[] GetControlTypes()
        {
            return new byte[]
            {
                1, 1, 8, 1, 1, 2,
                3, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1,
                3, 4, 1, 2
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Client is null) return;
            Client.Changed = true;
        }
    }
    // has tasks
    public class CISDetails : ClientDetailsBase, INotifyPropertyChanged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool withheld, suffered;
        public CISDetails(Client client)
        {
            Client = client;
            withheld = suffered = false;
        }
        #region Properties
        // has tasks
        public bool CISWithheld
        {
            get => withheld;
            set
            {
                if (withheld != value)
                {
                    var c = withheld;
                    withheld = value;
                    OnPropertyChanged(c);
                    string[] names = new string[]
                    {
                        $"CIS Withheld ({Client.Name})"
                    };
                    if (withheld)
                    {
                        /*Event e = DataHandler.AddEvent(new Event(name: names[0], colourType: "CISW", initalDate: DateTime.Today.SetDay(19), showPeriod: 13));
                        e.SetIntervals(new Interval(0, 1, 0));*/


                        #region Tasks -- New --
                        Tasks.TaskGroup taskGroup = Tasks.TaskGroup.CreateCISW(Client.Name);
                        DataHandler.AddTask(taskGroup);
                        Client.CISWTasks = taskGroup;
                        #endregion
                    }
                    else
                    {
                        //DataHandler.AllEvents.RemoveAll(e => names.Contains(e.DisplayName));
                        DataHandler.RemoveTask(Client.CISWTasks);
                    }
                }
            }
        }
        // has tasks
        public bool CISSuffered
        {
            get => suffered;
            set
            {
                if (suffered != value)
                {
                    var c = suffered;
                    suffered = value;
                    OnPropertyChanged(c);
                    string[] names = new string[]
                    {
                       $"CIS Suffered ({Client.Name})"
                    };
                    if (suffered)
                    {
                        /*Event e = DataHandler.AddEvent(new Event(name: names[0], colourType: "CISS", initalDate: DateTime.Today.SetDay(19), showPeriod: 13));
                        e.SetIntervals(new Interval(0, 1, 0));*/

                        #region Tasks -- New --
                        Tasks.TaskGroup taskGroup = Tasks.TaskGroup.CreateCISS(Client.Name);
                        DataHandler.AddTask(taskGroup);
                        Client.CISSTasks = taskGroup;
                        #endregion
                    }
                    else
                    {
                        //DataHandler.AllEvents.RemoveAll(e => names.Contains(e.DisplayName));
                        DataHandler.RemoveTask(Client.CISSTasks);
                    }
                }
            }
        }
        #endregion
        public override byte[] GetControlTypes()
        {
            return new byte[3]
            {
                6, 6, 1
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(object prevValue, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (Client is null) return;
            Client.Changed = true;
        }
    }
}
