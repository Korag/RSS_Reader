using System.Collections.Generic;
using RssModelsLib.Models;

namespace Rss_Downloader_Console
{
    public interface IDownloaderMainLogic
    {
        void DownloadNewContentIfItsAvailable();
        List<RssDocumentSingle> GetDocumentsWithNewContent();
        void SaveDocumentSingleToDatabase();
    }
}