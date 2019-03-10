using Rss_Downloader.Services;
using RssDbContextLib.Db_Context;
using RssModelsLib.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rss_Downloader_Console
{
    public class DownloaderMainLogic : IDownloaderMainLogic
    {
        private IWebSiteContentDownloader _downloader;
        private IRssDocumentsRepository _context;
        private List<RssDocumentSingle> _rssDocuments;

        public DownloaderMainLogic()
        {
            _downloader = new WebSiteContentDownloader("https://www.rmf24.pl/kanaly/rss");
            _context = new RssDocumentsRepository();

            if (_context.CheckIfDatabaseIsEmpty())
            {
                SaveDocumentSingleToDatabase();
                Console.WriteLine("Downloading finished");
            }
        }
        public void SaveDocumentSingleToDatabase()
        {
            _rssDocuments = _downloader.GetAllDocumentsWithoutSubContent();
            Parallel.ForEach(_rssDocuments, (content) =>
            {
                _downloader.FillSingleDocumentWithSubContent(content);
            });
            _context.SaveManyRssDocumentsToDatabase(_rssDocuments);
        }

        public List<RssDocumentSingle> GetDocumentsWithNewContent()
        {
            var documentsFromDb = _context.GetAllDocuments();
            return _downloader.GetDocumentsWithNewContentAvailable(documentsFromDb);
        }

        public void DownloadNewContentIfItsAvailable()
        {
            var newContent = GetDocumentsWithNewContent();
            if (newContent.Count > 0)
            {
                Console.WriteLine("New content is available");

                var documentsWithNewContent = _downloader.AddNewContentToDocuments(newContent);
                _context.AddNewContent(documentsWithNewContent);

                Console.WriteLine("New content has been added to: ");
                foreach (var document in documentsWithNewContent)
                {
                    Console.WriteLine(document.Title);
                }
            }
            else
            {
                Console.WriteLine("New content is not available ");
            }
        }
    }
}
