using Rss_Downloader.Db_Context;
using Rss_Downloader.Services;

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

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
            timer.Start();


        }
        public static void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            var rssDocuments = _downloader.GetContentFromWebSite();
            foreach (var document in rssDocuments)
            {
                _context.SaveRssDocumentToDatabase(document);
            }

        }
    }
}
