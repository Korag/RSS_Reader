using MongoDB.Driver;
using Rss_Reader.Models;

namespace Rss_Reader.Db_Context
{
    public interface IRssDocumentsRepository
    {
        IMongoCollection<RSSDocumentSingle> GetAllRssDocumentsFromDatabase();
    }
}