using Rss_Downloader.Db_Context;
using Rss_Downloader.Models;
using Rss_Downloader.Services;
using System;
using System.Collections.Generic;

namespace Rss_Downloader
{
    internal class Program
    {
        private static WebSiteContentDownloader _downloader;
        private static RssDocumentsRepository _context;
        private static List<RSSDocumentSingle> _rssDocuments;

        private static void Main(string[] args)
        {
            _downloader = new WebSiteContentDownloader("https://www.rmf24.pl/kanaly/rss");
            _context = new RssDocumentsRepository();

            SaveDocumentSingleToDatabase();

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);//1min

            var timer = new System.Threading.Timer((e) =>
            {
                CheckIfNewContentIsAvailable();
            }, null, startTimeSpan, periodTimeSpan);

            Console.Read();
        }
        public static void SaveDocumentSingleToDatabase()
        {
            _rssDocuments = _downloader.GetContentFromWebSite();
            foreach (var content in _rssDocuments)
            {
                _context.SaveRssDocumentToDatabase(content);
            }
        }

        public static void SaveDocumentItemsToDatabase()
        {
            foreach (var content in _rssDocuments)
            {
                _downloader.GetSubContentOfMainSite(content);
                _context.SaveRssDocumentToDatabase(content);
            }

        }
        public static void CheckIfNewContentIsAvailable()
        {
            if (true)
            {
                SaveDocumentItemsToDatabase();
            }
        }
    }
}
