using MongoDB.Driver;
using RSS_Downloader.Models;

namespace RSS_Downloader
{
    public class WebSiteRepository : IWebSiteRepository
    {
        private IMongoCollection<WebSite> _webSites;

        public WebSiteRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var server = client.GetDatabase("TestRssDB");
            _webSites = server.GetCollection<WebSite>("WebSites");
        }

        public void SaveWebSiteToDatabase(WebSite newWebSite)
        {
            _webSites.InsertOne(newWebSite);
        }


    }
}
