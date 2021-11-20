using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Bookkeeping_manager.Scripts
{
    using Database = Tuple<IMongoDatabase, string>;

    internal class MongoHandler
    {
        private MongoClient Client { get; set; }
        private Database ActiveDatabase { get; set; }
        public MongoHandler(string connectionString)
        {
            Client = new MongoClient(connectionString);
        }
        /// <summary>
        /// returns the active database
        /// </summary>
        public IMongoDatabase GetDatabase()
        {
            return ActiveDatabase.Item1;
        }
        /// <summary>
        /// returns the active database's name
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            return ActiveDatabase.Item2;
        }
        /// <summary>
        /// sets the active database not sure if it would create one 
        /// </summary>
        /// <param name="name"></param>
        public void SetDatabase(string name)
        {
            ActiveDatabase = new Database(Client.GetDatabase(name), name);
        }
        /// <summary>
        /// converts the bson doc then adds to the specified collection InsertOne
        /// </summary>
        public void AddDocument(string collectionName, MongoObject doc)
        {
            var collection = GetCollection(collectionName);
            collection.InsertOne(doc.ToBsonDocument());
        }
        /// <summary>
        /// converts the bson doc then adds to the specified collection InsertMany
        /// </summary>
        public void AddDocuments(string collectionName, params MongoObject[] docs)
        {
            var collection = GetCollection(collectionName);
            List<BsonDocument> docs_ = new List<BsonDocument>();
            foreach (MongoObject obj in docs)
            {
                docs_.Add(obj.ToBsonDocument());
            }
            collection.InsertMany(docs_);
        }
        /// <summary>
        /// gets the doc with the specifed id and deserilizes
        /// </summary>
        public T GetDocument<T>(string collectionName, ObjectId id)
        {
            var collection = GetCollection(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var doc = collection.Find(filter).FirstOrDefault();
            return Deserialize<T>(doc);
        }
        /// <summary>
        /// gets all docs with the specifed and deserilizes
        /// </summary>
        public List<T> GetAllDocuments<T>(string collectionName)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);
            IFindFluent<BsonDocument, BsonDocument> doc = collection.Find(new BsonDocument());
            List<BsonDocument> docs = doc.ToList();
            List<T> documents = new List<T>();
            docs.ForEach(d => documents.Add(Deserialize<T>(d)));
            return documents;
        }
        /// <summary>
        /// manualy deserialize the docs calls the constructor and the task group static menthods based on their type
        /// </summary>
        /// <returns></returns>
        public IndexSet<Tasks.TaskGroup> GetTasks()
        {
            IMongoCollection<BsonDocument> collection = GetCollection("Tasks");
            IFindFluent<BsonDocument, BsonDocument> doc = collection.Find(new BsonDocument());
            List<BsonDocument> docs = doc.ToList();
            IndexSet<Tasks.TaskGroup> result = new IndexSet<Tasks.TaskGroup>();

            for (int i = 0; i < docs.Count; i++)
            {
                BsonDocument bson = docs[i];

                ObjectId _id = bson["_id"].AsObjectId;
                string type = bson["Type"].AsString;
                string clientName = bson["ClientName"].AsString;
                string amlName = bson["AMLContactName"].AsString;
                string colour = bson["Colour"].AsString;
                string comment = bson["TaskComment"].AsString;
                string period = bson["PayRollPeriod"].AsString;
                string interval = bson["PayRollInterval"].AsString;
                DateTime baseDate = bson["BaseDate"].AsString.ToDate();
                int vatPeriod = bson["VATPeriod"].AsInt32;
                int[] advCnt = BsonSerializer.Deserialize<int[]>(bson["AdvanceCounts"].ToJson());

                Tasks.TaskGroup res = new Tasks.TaskGroup();
                switch (type)
                {
                    case "APE":
                        res = Tasks.TaskGroup.CreateAPE(clientName, baseDate);
                        break;
                    case "VATPE":
                        res = Tasks.TaskGroup.CreateVATPE(clientName, baseDate, vatPeriod);
                        break;
                    case "CA":
                        res = Tasks.TaskGroup.CreateCA(clientName, baseDate);
                        break;
                    case "AML":
                        res = Tasks.TaskGroup.CreateAML(clientName, baseDate, amlName);
                        break;
                    case "Custom":
                        res = Tasks.TaskGroup.CreateCustom(clientName, baseDate, colour, comment);
                        break;
                    case "Payroll":
                        res = Tasks.TaskGroup.CreatePayroll(clientName, baseDate, period, interval);
                        break;
                    case "SA":
                        res = Tasks.TaskGroup.CreateSA(clientName);
                        break;
                    case "P11D":
                        res = Tasks.TaskGroup.CreateP11D(clientName);
                        break;
                    case "CISW":
                        res = Tasks.TaskGroup.CreateCISW(clientName);
                        break;
                    case "CISS":
                        res = Tasks.TaskGroup.CreateCISS(clientName);
                        break;
                }
                res._id = _id;
                res.AdvanceTo(advCnt);
                res.HasChanged = false;
                result.Add(res);
            }

            return result;
        }
        public void Update<T>(string collectionName, ObjectId id, string attribute, T value)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var update = Builders<BsonDocument>.Update.Set(attribute, value);
            GetCollection(collectionName).UpdateOne(filter, update);
        }
        public void Update(string collectionName, MongoObject value)
        {
            foreach (KeyValuePair<string, object> pair in value.ToDictionary())
            {
                Update(collectionName, value._id, pair.Key, pair.Value);
            }
        }
        /// <summary>
        /// returns the specifed collection will create if not found
        /// </summary>
        public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return GetDatabase().GetCollection<BsonDocument>(collectionName); ;
        }
        /// <summary>
        /// deltes item where the id matches DeleteOne
        /// </summary>
        public bool Delete(string collectionName, ObjectId id)
        {
            var fillter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var res = GetCollection(collectionName).DeleteOne(fillter);
            return res.IsAcknowledged && res.DeletedCount > 0;
        }
        /// <summary>
        /// deltes item where the property has the given value DeleteOne
        /// </summary>
        public bool Delete(string collectionName, string property, object value)
        {
            var fillter = Builders<BsonDocument>.Filter.Eq(property, value);
            var res = GetCollection(collectionName).DeleteOne(fillter);
            return res.IsAcknowledged && res.DeletedCount > 0;
        }
        /// <summary>
        /// clears the document
        /// </summary>
        /// <param name="collectionName"></param>
        public void DeleteAll(string collectionName)
        {
            GetCollection(collectionName).DeleteMany(new BsonDocument());
        }
        /// <summary>
        /// attemtps the deserialise shows messagebox then throws error
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <returns></returns>
        public T Deserialize<T>(BsonDocument doc)
        {
            try
            {
                return BsonSerializer.Deserialize<T>(doc);
            }
            catch
            {
                MessageBox.Show($"Cannot Deserialize to {typeof(T).FullName}");
                throw new Exception();
            }
        }


        // Documents
        public void UploadFile(string filePath, string fileName)
        {
            ObjectId id;
            var fs = new GridFSBucket(GetDatabase());
            using (var s = File.OpenRead(filePath))
            {
                id = fs.UploadFromStream(fileName, s);
            }
        }
        public byte[] DownloadFile(string fileName)
        {
            var fs = new GridFSBucket(GetDatabase());
            return fs.DownloadAsBytesByName(fileName);
        }
        public void DownloadFile(string fileName, string filePath)
        {
            var fs = new GridFSBucket(GetDatabase());
            fs.DownloadAsBytesByName(fileName).ToFile(filePath);
        }
        public void RemoveFile(string fileName)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName);

            GridFSBucket bucket = new GridFSBucket(GetDatabase());
            var files = bucket.Find(filter).ToList();

            foreach (var file in files)
            {
                bucket.Delete(file.Id);
            }
        }
        public void RemoveAllFiles()
        {
            GridFSBucket bucket = new GridFSBucket(GetDatabase());
            var files = bucket.Find(new BsonDocument()).ToList();

            foreach (var file in files)
            {
                bucket.Delete(file.Id);
            }
        }

        public string[] GetAllFileNames()
        {
            var bucket = new GridFSBucket(GetDatabase());
            var filter = Builders<GridFSFileInfo>.Filter.Empty;

            using (var cursor = bucket.Find(filter))
            {
                return cursor.ToList().Select(x => x.Filename).ToArray();
            }
        }
    }
    public class MongoObject
    {
#pragma warning disable IDE1006 // Naming Styles
        public ObjectId _id { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
