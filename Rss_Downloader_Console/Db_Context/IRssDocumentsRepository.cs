
using Rss_Downloader.Models;

namespace Rss_Downloader.Db_Context
{
    public interface IRssDocumentsRepository
    {
        void SaveRssDocumentToDatabase(RSSDocumentSingle newRssDocument);
    }
}