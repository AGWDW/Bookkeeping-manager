using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
    static class DataHandler 
    { 
        public static List<Client> AllClients { get; set; }
        public static List<Event> AllEvents { get; set; }
        public static bool AllowSet { get; set; }
        private static MongoHandler Handler;
        public static void Init()
        {
            Handler = new MongoHandler(@"mongodb+srv://MainClient:M3YMhbTS63XT7yuK@maindata.7qgv2.mongodb.net/Data?retryWrites=true&w=majority");
            Handler.SetDatabase("Data");
            AllClients = new List<Client>();
            AllEvents = new List<Event>();

            GetColours();
            GetClientsFromDatabase();
            GetEventsFromDatabase();
            GetDocumentsFromDatabase();
            SetBindings();
        }
        private static void GetClientsFromDatabase()
        {
            AllClients = Handler.GetAllDocuments<Client>("Clients");
            foreach (Client client in AllClients)
            {
                client.SetNames();
            }
            AllClients = AllClients.OrderBy(c => c.Name).ToList();
            AllowSet = true;
        }
        private static void GetEventsFromDatabase()
        {
            AllEvents = Handler.GetAllDocuments<Event>("Events");
        }
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
        private static void SetBindings()
        {
            foreach(Event e in AllEvents)
            {
                if (e.BindingProperty == "")
                    continue;
                if(e.BindingType == "Client")
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
        }

        public static void UploadToDatabase()
        {
            Handler.RemoveAllFiles();
            foreach (Client client in AllClients)
            {
                if (client.Changed || client.Delete)
                {
                    Handler.Delete("Clients", "Name", client.Name);
                    if(client.Changed)
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


            foreach (Event @event in AllEvents)
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
                    }*/
                    @event.Date = @event.Date.Date;
                    if(@event.Changed)
                        Handler.AddDocument("Events", @event);
                }
                @event._id = MongoDB.Bson.ObjectId.Empty;
            }
        }
        public static void RemoveEvent(string name)
        {
            for(int i = 0; i < AllEvents.Count; i++)
            {
                Event e = AllEvents[i];
                if(e.DisplayName == name)
                {
                    e.Delete = true;
                }
            }
        }
        public static bool RemoveEventsContaining(string name)
        {
            return AllEvents.RemoveAll(e => e.DisplayName.Contains(name)) > 0;
        }
        public static Event AddEvent(Event e)
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
