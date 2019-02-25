using MongoDB.Driver;
using Rss_Reader.Models;
using System.IO;
using System.Linq;

namespace Rss_Reader.Db_Context
{
    public class RssDocumentsRepository : IRssDocumentsRepository
    {
        private IMongoCollection<RSSDocumentSingle> _rssDocumentCollection;

        public RssDocumentsRepository()
        {
            string user = "RssReader";
            string password = "Reader1";
            string database = "rss_downloader_web_application";
            string connectionstring = $"mongodb://{user}:{password}@ds062818.mlab.com:62818/{database}";
            string collectionRSS = "Stored_RSS";

            var client = new MongoClient(connectionstring);
            var server = client.GetDatabase(database);
            _rssDocumentCollection = server.GetCollection<RSSDocumentSingle>(collectionRSS);
        }

        public IMongoCollection<RSSDocumentSingle> GetAllRssDocumentsFromDatabase()
        {
            return _rssDocumentCollection;
        }


    }
}