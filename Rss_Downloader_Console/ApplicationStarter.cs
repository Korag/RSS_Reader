namespace Rss_Downloader_Console
{
    public class ApplicationStarter : IApplicationStarter
    {
        private IDownloaderMainLogic _downloader;

        public ApplicationStarter(IDownloaderMainLogic downloader)
        {
            _downloader = downloader;
        }
        public void Run()
        {
            #region Local
            //var startTimeSpan = TimeSpan.Zero;
            //var periodTimeSpan = TimeSpan.FromMinutes(1);//1min

            //var timer = new System.Threading.Timer((e) =>
            //{
            //    _downloader.DownloadNewContentIfItsAvailable();

            //}, null, startTimeSpan, periodTimeSpan);
            #endregion

            #region Azure
            _downloader.DownloadNewContentIfItsAvailable();
            #endregion

        }
    }
}
