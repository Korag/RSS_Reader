using RssModelsLib.Models;
using System.Collections.Generic;

namespace Rss_Downloader.Services
{
    public interface IDownloaderMainLogic
    {
        void DownloadNewContentIfItsAvailable();
        List<RssDocumentSingle> GetDocumentsWithNewContent();
        void SaveDocumentSingleToDatabase();
    }
}