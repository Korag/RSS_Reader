using RssModelsLib.Models;
using System.Collections.Generic;

namespace Rss_Downloader.Services
{
    public interface IWebSiteContentDownloader
    {
        List<RssDocumentSingle> AddNewContentToDocuments(List<RssDocumentSingle> documentsWithNewContentAvailable);
        void FillSingleDocumentWithSubContent(RssDocumentSingle mainContent);
        List<RssDocumentSingle> GetAllDocumentsWithoutSubContent();
        List<RssDocumentSingle> GetDocumentsWithNewContentAvailable(List<RssDocumentSingle> rssDocumentsFromDb);
    }

}