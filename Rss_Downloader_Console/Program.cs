using Rss_Downloader.Db_Context;
using Rss_Downloader.Models;
using Rss_Downloader.Services;
using Rss_Downloader_Console.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rss_Downloader
{
    internal class Program
    {
        private static WebSiteContentDownloader _downloader;
        private static RssDocumentsRepository _context;
        private static EmailServiceProvider _emailProvider;
        private static List<RssDocumentSingle> _rssDocuments;

        private static void Main(string[] args)
        {

            _downloader = new WebSiteContentDownloader("https://www.rmf24.pl/kanaly/rss");
            _context = new RssDocumentsRepository();

            if (_context.CheckIfDatabaseIsEmpty())
            {
                SaveDocumentSingleToDatabase();
                Console.WriteLine("Downloading finished");
            }

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);//1min

            var timer = new System.Threading.Timer((e) =>
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
            }, null, startTimeSpan, periodTimeSpan);

            //to do: everyday at 10 and 18
            _emailProvider = new EmailServiceProvider();
            _emailProvider.SendNewsletterToSubscribers();

            Console.Read();
        }
        public static void SaveDocumentSingleToDatabase()
        {
            _rssDocuments = _downloader.GetAllDocumentsWithoutSubContent();
            Parallel.ForEach(_rssDocuments, (content) =>
            {
                _downloader.FillSingleDocumentWithSubContent(content);
            });
            _context.SaveManyRssDocumentsToDatabase(_rssDocuments);
        }

        public static List<RssDocumentSingle> GetDocumentsWithNewContent()
        {
            var documentsFromDb = _context.GetAllDocuments();
            return _downloader.GetDocumentsWithNewContentAvailable(documentsFromDb);
        }
    }
}
