using Autofac;
using Rss_Downloader.Services;
using Rss_Downloader_Console.IoC;

namespace Rss_Downloader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplicationStarter>();
                app.Run();
            }
        }
    }
}
