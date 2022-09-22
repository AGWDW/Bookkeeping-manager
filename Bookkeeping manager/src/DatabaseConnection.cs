using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Clients;
using Bookkeeping_manager.src.Tasks;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Bookkeeping_manager.src
{
    public static class DatabaseConnection
    {
        private const string TasksCollection = "Tasks";
        private const string ClientsCollection = "Clients";
        private const string ArchiveCollection = "ArchivedClients";
        private static MongoHandler Connection { get; set; }
        public static bool Deserilazing { get; set; }
        public static bool Connect()
        {
            try
            {
                Connection = new MongoHandler(@"mongodb+srv://MainClient:M3YMhbTS63XT7yuK@maindata.7qgv2.mongodb.net/Data?retryWrites=true&w=majority");
                Connection.SetDatabase("Data-V2");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static bool PopulateUIDs()
        {
            try
            {
                var t = Connection.GetAllDocuments<BsonDocument>("UID-Counters");
                BsonDocument doc = Connection.GetAllDocuments<BsonDocument>("UID-Counters")[0];
                int clientUID = ((int)doc.GetElement("ClientUID_Counter").Value);
                int taskUID = ((int)doc.GetElement("TaskUID_Counter").Value);

                ClientManager.SetUID_Counter(clientUID);
                TaskManager.SetUID_Counter(taskUID);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool PopulateClients()
        {
            try
            {
                Deserilazing = true;
                var clients = Connection.GetAllDocuments<Clients.Client>(ClientsCollection);
                clients.ForEach(client => client.UpdateName());
                clients = clients.OrderBy(c => c.Name).ToList();
                ClientManager.SetClients(clients);
                Deserilazing = false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool PopulateTasks()
        {
            try
            {
                var tasks = Connection.GetAllTasks(TasksCollection);
                tasks.ForEach(task => task.UpdateState());
                tasks = tasks.OrderBy(c => c.Name).ToList();
                TaskManager.SetTasks(tasks);
                return true;
            }
            catch
            {
                return false;
            }
        }


        private delegate void asyncCaller(int uid);

        public static void DeleteClient(int uid)
        {
            asyncCaller c = new asyncCaller(deleteClient);
            c.BeginInvoke(uid, null, null);
        }
        private static void deleteClient(int uid)
        {
            Connection.Delete(ClientsCollection, uid);
        }
        public static void ArchiveClient(int uid)
        {
            asyncCaller c = new asyncCaller(archiveClient);
            c.BeginInvoke(uid, null, null);
        }
        private static void archiveClient(int uid)
        {
            Clients.Client c = ClientManager.GetClient(uid);
            Connection.AddDocument(ArchiveCollection, c);
            deleteClient(uid);
        }


        private static void addClient(int uid)
        {
            Clients.Client c = ClientManager.GetClient(uid);
            if(c is null)
            {
                return;
            }
            Connection.AddDocument(ClientsCollection, c);
            int counter = ClientManager.uid_counter;
            Connection.Update("UID-Counters", "ClientUID_Counter", counter - 1, counter);
        }
        public static void AddClient(int uid)
        {
            asyncCaller c = new asyncCaller(addClient);
            c.BeginInvoke(uid, null, null);
        }

        public static void UpdateClient(int uid)
        {
            asyncCaller c = new asyncCaller(updateClient);
            c.BeginInvoke(uid, null, null);
        }
        private static void updateClient(int uid)
        {
            deleteClient(uid);
            addClient(uid);
        }

        private static void renameClient(int uid, string name)
        {
            Connection.Update(ClientsCollection, uid, "Name", name);
        }
        private delegate void asyncCaller2(int uid, string name);
        public static void RenameClient(int uid, string name)
        {
            if (Deserilazing)
            {
                return;
            }
            asyncCaller2 c = new asyncCaller2(renameClient);
            c.BeginInvoke(uid, name, null, null);
        }

        private static void changeCommentClient(int uid, string comment)
        {
            Connection.Update(ClientsCollection, uid, "Comments", comment);
        }
        public static void ChangeCommentClient(int uid, string comment)
        {
            if (Deserilazing)
            {
                return;
            }
            asyncCaller2 c = new asyncCaller2(changeCommentClient);
            c.BeginInvoke(uid, comment, null, null);
        }


        public static void AddTask(int uid)
        {
            // asyncCaller caller = new asyncCaller(addTask);
            // caller.BeginInvoke(uid, null, null);
            addTask(uid);
        }
        private static void addTask(int uid)
        {
            Tasks.Task t = TaskManager.GetTask(uid);
            if(t is null)
            {
                return;
            }
            Connection.AddDocument(TasksCollection, t);
            int counter = TaskManager.uid_counter;
            Connection.Update("UID-Counters", "TaskUID_Counter", counter - 1, counter);
        }

        public static void DeleteTask(int uid)
        {
            asyncCaller c = new asyncCaller(deleteTask);
            c.BeginInvoke(uid, null, null);
        }
        private static void deleteTask(int uid)
        {
            Connection.Delete(TasksCollection, uid);
        }

        public static void UpdateTask(int uid)
        {
            updateTask(uid);
            // asyncCaller c = new asyncCaller(updateTask);
            // c.BeginInvoke(uid, null, null);
        }
        private static void updateTask(int uid)
        {
            deleteTask(uid);
            addTask(uid);
        }
    }
}
