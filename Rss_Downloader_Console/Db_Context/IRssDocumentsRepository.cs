
using Rss_Downloader.Models;
using System.Collections.Generic;

namespace Rss_Downloader.Db_Context
{
    public interface IRssDocumentsRepository
    {
        void SaveOneRssDocumentToDatabase(RSSDocumentSingle rssDocument);
        void SaveManyRssDocumentsToDatabase(List<RSSDocumentSingle> rssDocuments);
        bool CheckIfDatabaseIsEmpty();

    }
}