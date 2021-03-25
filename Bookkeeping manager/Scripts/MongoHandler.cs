using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.IO;
using MongoDB.Driver.GridFS;
using System.Windows;

namespace Bookkeeping_manager.Scripts
{
    using Database = Tuple<IMongoDatabase, string>;
    class MongoHandler
    {
        private MongoClient Client { get; set; }
        private Database ActiveDatabase { get; set; }
        public MongoHandler(string connectionString)
        {
            Client = new MongoClient(connectionString);                      
        }
        public IMongoDatabase GetDatabase()
        {
            return ActiveDatabase.Item1;
        }
        public string GetDatabaseName()
        {
            return ActiveDatabase.Item2;
        }
        public void SetDatabase(string name)
        {
            ActiveDatabase = new Database(Client.GetDatabase(name), name);
        }
        public void AddDocument(string collectionName, MongoObject doc)
        {
            var collection = GetCollection(collectionName);
            collection.InsertOne(doc.ToBsonDocument());
        }
        public void AddDocuments(string collectionName, params MongoObject[] docs)
        {
            var collection = GetCollection(collectionName);
            List<BsonDocument> docs_ = new List<BsonDocument>();
            foreach(MongoObject obj in docs)
            {
                docs_.Add(obj.ToBsonDocument());
            }
            collection.InsertMany(docs_);
        }
        public T GetDocument<T>(string collectionName, ObjectId id)
        {
            var collection = GetCollection(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var doc = collection.Find(filter).FirstOrDefault();
            return Deserialize<T>(doc);
        }
        public List<T> GetAllDocuments<T>(string collectionName)
        {
            var collection = GetCollection(collectionName);
            var doc = collection.Find(new BsonDocument());
            var docs = doc.ToList();
            List<T> documents = new List<T>();
            docs.ForEach(d => documents.Add(Deserialize<T>(d)));
            return documents;
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
        public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return GetDatabase().GetCollection<BsonDocument>(collectionName); ;
        }
        public bool Delete(string collectionName, ObjectId id)
        {
            var fillter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var res = GetCollection(collectionName).DeleteOne(fillter);
            return res.IsAcknowledged && res.DeletedCount > 0;
        }
        public bool Delete(string collectionName, string property, object value)
        {
            var fillter = Builders<BsonDocument>.Filter.Eq(property, value);
            var res = GetCollection(collectionName).DeleteOne(fillter);
            return res.IsAcknowledged && res.DeletedCount > 0;
        }
        public void DeleteAll(string collectionName)
        {
            GetCollection(collectionName).DeleteMany(new BsonDocument());
        }
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
