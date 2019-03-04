
using MongoDB.Driver;
using Rss_Downloader.Models;
using Rss_Downloader_Console.Models;
using System.Collections.Generic;

namespace Rss_Downloader.Db_Context
{
    public interface IRssDocumentsRepository
    {
        void InitializeContext(string connectionstring, string database);
        void SaveOneRssDocumentToDatabase(RssDocumentSingle rssDocument);
        void SaveManyRssDocumentsToDatabase(List<RssDocumentSingle> rssDocuments);
        bool CheckIfDatabaseIsEmpty();
        IMongoCollection<SubscriberEmail> GetSubscribersList();



    }
}