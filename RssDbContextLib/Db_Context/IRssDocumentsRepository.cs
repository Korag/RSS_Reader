using MongoDB.Driver;
using RssModelsLib.Models;
using System.Collections.Generic;

namespace RssDbContextLib.Db_Context
{
    public interface IRssDocumentsRepository
    {
        void InitializeContext(string connectionstring, string database);
        void SaveOneRssDocumentToDatabase(RssDocumentSingle rssDocument);
        void SaveManyRssDocumentsToDatabase(List<RssDocumentSingle> rssDocuments);
        bool CheckIfDatabaseIsEmpty();
        void InsertToMailingList(string emailAddress, ICollection<string> subscriberList);
        IMongoCollection<RssDocumentSingle> GetAllRssDocumentsFromDatabase();
        RssDocumentSingle GetDocumentByIdFromDatabase(string id);
        void DeleteFromMailingList(string emailAddress);
        List<RssDocumentSingle> GetAllDocuments();
        void AddNewContent(List<RssDocumentSingle> documentsWithNewContent);
    }
}