using RSS_Downloader.Models;

namespace RSS_Downloader
{
    public interface IWebSiteRepository
    {
        void SaveWebSiteToDatabase(WebSite newWebSite);
    }
}