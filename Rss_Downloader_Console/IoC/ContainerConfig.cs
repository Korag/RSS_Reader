using Autofac;
using Rss_Downloader.Services;
using RssDbContextLib.Db_Context;

namespace Rss_Downloader_Console.IoC
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DownloaderMainLogic>().As<IDownloaderMainLogic>();
            builder.RegisterType<WebSiteContentDownloader>().As<IWebSiteContentDownloader>();
            builder.RegisterType<RssDocumentsRepository>().As<IRssDocumentsRepository>();

            return builder.Build();
        }
    }
}
