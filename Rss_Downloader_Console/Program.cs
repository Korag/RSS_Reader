using Rss_Downloader_Console;
using Rss_Downloader_Console.Services;
using System;

namespace Rss_Downloader
{
    internal class Program
    {
        private static EmailServiceProvider _emailProvider;
        private static IDownloaderMainLogic _downloader;
        private static void Main(string[] args)
        {
            _downloader = new DownloaderMainLogic();

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);//1min

            var timer = new System.Threading.Timer((e) =>
            {
                _downloader.DownloadNewContentIfItsAvailable();

            }, null, startTimeSpan, periodTimeSpan);

            //to do: everyday at 10 and 18
            _emailProvider = new EmailServiceProvider();
            _emailProvider.SendNewsletterToSubscribers();

            Console.Read();
        }
    }
}
