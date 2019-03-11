using Rss_Downloader_Console;
using Rss_Downloader_Console.Services;
using System;
using System.Threading;

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

            while (true)
            {
                var czas = DateTime.Now;
                Console.WriteLine(czas);
                if (czas.ToString("HH:mm") == "13:05")
                {
                    Console.WriteLine("Wysyłam maile o godzinie" + czas);
                    _emailProvider = new EmailServiceProvider();
                    _emailProvider.SendNewsletterToSubscribers();
                    Thread.Sleep(70000);
                }
            }
        }
    }
}
