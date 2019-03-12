using Rss_Downloader_Console;
using System;

namespace Rss_Downloader
{
    internal class Program
    {
        private static IDownloaderMainLogic _downloader;
        private static void Main(string[] args)
        {
            _downloader = new DownloaderMainLogic();

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
