using MongoDB.Driver;
using Rss_Downloader.Models;
using Rss_Downloader_Console.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rss_Downloader.Db_Context
{
    public class RssDocumentsRepository : IRssDocumentsRepository
    {
        private IMongoCollection<RSSDocumentSingle> _rssDocumentCollection;

        private MongoClient _mongoClient;
        private IMongoDatabase _server;
        public string collectionRSS = "Stored_RSS";


        public RssDocumentsRepository()
        {
            string path = @"C:\credentials.txt";
            string user = "Admin";
            string password = File.ReadLines(path).First();
            string database = "rss_downloader_web_application";
            string connectionstring = $"mongodb://{user}:{password}@ds062818.mlab.com:62818/{database}";

            InitializeContext(connectionstring, database);
        }

        public void InitializeContext(string connectionstring, string database)
        {
            _mongoClient = new MongoClient(connectionstring);
            _server = _mongoClient.GetDatabase(database);
            _rssDocumentCollection = _server.GetCollection<RSSDocumentSingle>(collectionRSS);
        }

        public void SaveOneRssDocumentToDatabase(RSSDocumentSingle rssDocument)
        {
            _rssDocumentCollection.InsertOne(rssDocument);
        }

        public void SaveManyRssDocumentsToDatabase(List<RSSDocumentSingle> rssDocuments)
        {
            _rssDocumentCollection.InsertMany(rssDocuments);
        }

        public bool CheckIfDatabaseIsEmpty()
        {
            if (_rssDocumentCollection.AsQueryable().Count() == 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckIfDatabaseContainsEnoughDocuments()
        {
            if (_rssDocumentCollection.AsQueryable().Count() == 31)
            {
                return true;
            }
            return false;
        }

        public void DeleteAllDocumentsFromDatabse()
        {
            _rssDocumentCollection.DeleteMany(x=>true);
        }

    }
}