using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bookkeeping_manager.Scripts
{
    public class Document : MongoObject
    {
        public string FileName { get; set; }
        public string FileName_Cloud { get; set; }
        public string FilePath { get; set; }
        public string GetClientName()
        {
            return FileName_Cloud.Substring(0, FileName_Cloud.IndexOf('|'));
        }
    }

    internal static class DataHandler
    {
        public static List<Client> AllClients { get; set; }
        // public static List<Event> AllEvents { get; set; }
        public static IndexSet<Tasks.TaskGroup> AllTasks { get; set; }
        private static List<Tasks.TaskGroup> TaskDeathRow { get; set; }
        public static bool AllowSet { get; set; }
        private static MongoHandler Handler;
        /// <summary>
        /// astablises connection init properties to deafult gets colours the clients and tasks then AllowSet = true;
        /// </summary>
        public static void Init(bool createTasks = false)
        {
            Handler = new MongoHandler(@"mongodb+srv://MainClient:M3YMhbTS63XT7yuK@maindata.7qgv2.mongodb.net/Data?retryWrites=true&w=majority");
            Handler.SetDatabase("Data");
            AllClients = new List<Client>();
            // AllEvents = new List<Event>();
            AllTasks = new IndexSet<Tasks.TaskGroup>();
            TaskDeathRow = new List<Tasks.TaskGroup>();

            GetColours();
            GetClientsFromDatabase("Clients_", createTasks);
            // GetEventsFromDatabase();
            GetDocumentsFromDatabase();
            // SetBindings();
            GetTasks();
            AllowSet = true;
        }
        private static void GetClientsFromDatabase(string docName = "Clients_", bool createTasks = false)
        {
            AllowSet = createTasks;
            AllClients = Handler.GetAllDocuments<Client>(docName);
            foreach (Client client in AllClients)
            {
                client.SetNames();
            }
            AllClients = AllClients.OrderBy(c => c.Name).ToList();
            AllowSet = true;
        }
        /*private static void GetEventsFromDatabase()
        {
            AllEvents = Handler.GetAllDocuments<Event>("Events");
        }*/
        private static void GetDocumentsFromDatabase()
        {
            string[] docNames = Handler.GetAllFileNames();
            string tempPath = Path.GetTempPath();
            foreach (string docName in docNames)
            {
                string path = tempPath + docName.Substring(docName.IndexOf('|') + 1);
                Handler.DownloadFile(docName, path);
                Document document = new Document()
                {
                    FileName_Cloud = docName,
                    FilePath = path,
                    FileName = docName.Substring(docName.IndexOf('|') + 1)
                };
                string clientName = document.GetClientName();
                AllClients.Find(c => c.Name == clientName).Documents.Add(document);
            }
        }
        /*private static void SetBindings()
        {
            foreach (Event e in AllEvents)
            {
                if (e.BindingProperty == "")
                    continue;
                if (e.BindingType == "Client")
                {
                    try
                    {
                        e.Binding = AllClients.Find(c => e.DisplayName.Contains($"({c.Name})")).GetProperty<object>(e.BindingName);
                    }
                    catch
                    {

                    }
                }
                else // Event
                {
                    e.Binding = AllEvents.Find(c => e.DisplayName == c.BindingName);
                }
            }
            AllClients.ForEach(c => c.Changed = false);
        }*/

        /// <summary>
        /// populates AllTasks with the tasks found in the data base preserves object id and calls the taskgroup constructor then sets the task bindings
        /// </summary>
        public static void GetTasks()
        {
            AllTasks = Handler.GetTasks();
            SetTaskBinding();
        }
        /// <summary>
        /// finds the client the task refers to then based on the task type sets the appropriate property
        /// </summary>
        private static void SetTaskBinding()
        {
            for (int i = 0; i < AllTasks.Length; i++)
            {
                Tasks.TaskGroup group = AllTasks[i];
                if (group.Type == "Custom")
                {
                    continue;
                }

                Client c = AllClients.Find(x => x.Name == group.ClientName);
                if (c is null)
                {
                    continue;
                }
                switch (group.Type)
                {
                    case "APE":
                        c.APETasks = group;
                        break;
                    case "VATPE":
                        c.VATPETasks = group;
                        break;
                    case "CA":
                        c.CATasks = group;
                        break;
                    case "SA":
                        c.SATasks = group;
                        break;
                    case "P11D":
                        c.P11DTasks = group;
                        break;
                    case "CISW":
                        c.CISWTasks = group;
                        break;
                    case "CISS":
                        c.CISSTasks = group;
                        break;
                    case "AML":
                        if(c.AMLTasks is null)
                        {
                            c.AMLTasks = new Dictionary<string, Tasks.TaskGroup>();
                        }
                        if (c.AMLTasks.ContainsKey(group.AMLContactName))
                        {
                            c.AMLTasks[group.AMLContactName] = group;
                        }
                        else
                        {
                            c.AMLTasks.Add(group.AMLContactName, group);
                        }
                        break;
                    case "Payroll":
                        if (c.PayrollTasks is null)
                        {
                            c.PayrollTasks = new Dictionary<string, Tasks.TaskGroup>();
                        }
                        if (c.PayrollTasks.ContainsKey(group.PayRollPeriod))
                        {
                            c.PayrollTasks[group.PayRollPeriod] = group;
                        }
                        else
                        {
                            c.PayrollTasks.Add(group.PayRollPeriod, group);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static void UploadToDatabase()
        {
            Handler.RemoveAllFiles();
            foreach (Client client in AllClients)
            {
                if (client.Changed || client.Delete)
                {
                    Handler.Delete("Clients", "Name", client.Name);
                    if (client.Changed)
                        Handler.AddDocument("Clients", client);
                }
                client._id = MongoDB.Bson.ObjectId.Empty;
                foreach (Document document in client.Documents)
                {
                    document._id = MongoDB.Bson.ObjectId.Empty;
                    Handler.UploadFile(document.FilePath, document.FileName_Cloud);
                    File.Delete(document.FilePath);
                }
            }


            /*foreach (Event @event in AllEvents)
            {
                if (@event.Changed || @event.Delete)
                {
                    if (@event.DisplayName.Contains("Equicob"))
                    {
                        _ = 0;
                    }
                    bool t = Handler.Delete("Events", "DisplayName", @event.DisplayName);
                    /*if (!t)
                    {
                        MessageBox.Show("Failed to delete event");
                        throw new System.Exception();
                    }*
                    @event.Date = @event.Date.Date;
                    if (@event.Changed)
                        Handler.AddDocument("Events", @event);
                }
                @event._id = MongoDB.Bson.ObjectId.Empty;
            }*/
        }
        /// <summary>
        /// replaces clients in the database with there updated ones if aproriate compares the client name and the documents
        /// </summary>
        public static void UploadClients(string docName = "Clients_")
        {
            Handler.RemoveAllFiles();
            foreach (Client client in AllClients)
            {
                if (client.Changed || client.Delete)
                {
                    Handler.Delete(docName, client._id);
                    if (client.Changed && !client.Delete)
                    {
                        Handler.AddDocument(docName, client);
                    }
                }
                foreach (Document document in client.Documents)
                {
                    document._id = MongoDB.Bson.ObjectId.Empty;
                    Handler.UploadFile(document.FilePath, document.FileName_Cloud);
                    File.Delete(document.FilePath);
                }
            }
            /*
            Handler.RemoveAllFiles();
            foreach (Client client in AllClients)
            {
                if (client.Changed || client.Delete)
                {
                    Handler.Delete("Clients", "Name", client.Name);
                    if (client.Changed)
                        Handler.AddDocument("Clients", client);
                }
                client._id = MongoDB.Bson.ObjectId.Empty;
                foreach (Document document in client.Documents)
                {
                    document._id = MongoDB.Bson.ObjectId.Empty;
                    Handler.UploadFile(document.FilePath, document.FileName_Cloud);
                    File.Delete(document.FilePath);
                }
            }
            */
        }
        /// <summary>
        /// replaces where approriate compares object id and deltes the tasks found on death row
        /// </summary>
        public static void UploadTasks()
        {
            for (int i = 0; i < AllTasks.Length; i++)
            {
                Tasks.TaskGroup group = AllTasks[i];
                if (group.HasChanged)
                {
                    bool hasDeleted = Handler.Delete("Tasks", group._id);
                    /*if (!hasDeleted)
                    {
                        MessageBox.Show("Failed to delete event");
                        throw new System.Exception();
                    }*/
            if (group.HasChanged)
                    {
                        Handler.AddDocument("Tasks", group);
                    }
                }
            }
            for (int i = 0; i < TaskDeathRow.Count; i++)
            {
                Tasks.TaskGroup group = TaskDeathRow[i];
                if(group is null)
                {
                    continue;
                }
                bool hasDeleted = Handler.Delete("Tasks", group._id);
                /*if (!hasDeleted)
                {
                    MessageBox.Show("Failed to delete event");
                    throw new System.Exception();
                }*/
            }
        }
        /*public static void RemoveEvent(string name)
        {
            for (int i = 0; i < AllEvents.Count; i++)
            {
                Event e = AllEvents[i];
                if (e.DisplayName == name)
                {
                    e.Delete = true;
                }
            }
        }*/
        /*public static bool RemoveEventsContaining(string name)
        {
            return AllEvents.RemoveAll(e => e.DisplayName.Contains(name)) > 0;
        }*/
        /*public static Event AddEvent(Event e)
        {
            for (int i = 0; i < AllEvents.Count; i++)
            {
                Event e_ = AllEvents[i];
                if (e_.DisplayName == e.DisplayName)
                {
                    e_.Changed = true;

                    AllEvents.RemoveAt(i);
                    AllEvents.Insert(i, e);

                    return AllEvents[i];
                }
            }


            // RemoveEvent(e.DisplayName);
            AllEvents.Add(e);
            return AllEvents.Last();
        }*/

        /// <summary>
        /// will replace ant found task and the replaced is added to death row 
        /// </summary>
        /// <param name="task"></param>
        public static void AddTask(Tasks.TaskGroup task)
        {
            bool added = AllTasks.Add(task);
            if (!added)
            {
                RemoveTask(task);
                AllTasks.Add(task);
            }
        }
        /// <summary>
        /// removes from allTasks then adds to death row
        /// </summary>
        /// <param name="group"></param>
        public static void RemoveTask(Tasks.TaskGroup group)
        {
            TaskDeathRow.Add(AllTasks[group]);
            AllTasks.Remove(group);
        }

        public static Dictionary<string, string> EventColours { get; set; }
        public static void GetColours()
        {
            EventColours = new Dictionary<string, string>();
            const string filePath = @"TaskColours.txt";
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();
                    string[] values = line.Split(',');
                    if (line == "" || line == "\n" || line == "\t" || values.Length != 2)
                        continue;

                    EventColours.Add(values[0].Trim(), values[1].Trim());
                }
            }
            if (EventColours.Count() == 0)
            {
                EventColours.Add("NULL", "#000000");
            }
        }
    }
}
