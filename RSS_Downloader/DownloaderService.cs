using System.ServiceProcess;

namespace RSS_Downloader
{
    public partial class DownloaderService : ServiceBase
    {
        private WebSiteContentDownloader _downloader;
        private RssDocumentsRepository _context;
        public DownloaderService()
        {
            InitializeComponent();
            _downloader = new WebSiteContentDownloader("https://www.rmf24.pl/kanaly/rss");
        }

        protected override void OnStart(string[] args)
        {

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
            timer.Start();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            var rssDocuments = _downloader.GetContentFromWebSite();
            foreach (var document in rssDocuments)
            {
                _context.SaveRssDocumentToDatabase(document);
            }

        }

        protected override void OnStop()
        {
        }
    }
}
