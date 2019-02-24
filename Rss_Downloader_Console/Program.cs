using Rss_Downloader.Db_Context;
using Rss_Downloader.Services;
using System;

namespace Rss_Downloader
{
    internal class Program
    {
        private static WebSiteContentDownloader _downloader;
        private static RssDocumentsRepository _context;


        private static void Main(string[] args)
        {
            _downloader = new WebSiteContentDownloader("https://www.rmf24.pl/kanaly/rss");
            _context = new RssDocumentsRepository();

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);

            var timer = new System.Threading.Timer((e) =>
            {
                Download();
            }, null, startTimeSpan, periodTimeSpan);
            Console.Read();
        }
        public static void Download()
        {
            var rssDocuments = _downloader.GetContentFromWebSite();
            foreach (var document in rssDocuments)
            {

                foreach (var content in rssDocuments)
                {
                    _downloader.GetSubContentOfMainSite(content);
                }
                _context.SaveRssDocumentToDatabase(document);
            }

        }
    }
}
