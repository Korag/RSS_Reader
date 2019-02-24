
using RSS_Downloader.Models;

namespace RSS_Downloader
{
    public interface IRssDocumentsRepository
    {
        void SaveRssDocumentToDatabase(RSSDocumentSingle newRssDocument);
    }
}