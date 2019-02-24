using HtmlAgilityPack;
using RSS_Downloader.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RSS_Downloader
{
    public interface IWebSiteContentDownloader
    {
        List<WebSite> GetContentFromWebSite();
        List<object> GetSubContentOfMainSite(WebSite mainContent);
    }

    public class WebSiteContentDownloader : IWebSiteContentDownloader
    {
        private readonly HtmlNode _documentNode;
        private List<string> _links;

        public WebSiteContentDownloader(string path)
        {
            var htmlWeb = new HtmlWeb();
            _documentNode = htmlWeb.Load(path).DocumentNode;
        }

        public List<WebSite> GetContentFromWebSite()
        {
            List<WebSite> tempWebSites = new List<WebSite>();
            foreach (var link in _links)
            {
                var mainContent = XElement.Load(link);
                WebSite newWebSite = new WebSite()
                {
                    Title = mainContent.Descendants("title").FirstOrDefault()?.Value,
                    Description = mainContent.Descendants("description").FirstOrDefault()?.Value,
                    Image = mainContent.Descendants("image").Descendants("url").FirstOrDefault()?.Value,
                    Link = mainContent.Descendants("link").FirstOrDefault()?.Value,
                    LastUpdate = mainContent.Descendants("lastBuildDate").FirstOrDefault()?.Value,
                    SubPages = new List<WebSiteContent>()
                };
                tempWebSites.Add(newWebSite);
                newWebSite.SubPages = GetSubContentFromMainSite(newWebSite);
            }
            return tempWebSites;
        }

        public List<object> GetSubContentOfMainSite(WebSite mainContent)
        {
            List<object> subContentList = new List<object>();

            var subContent = XElement.Load(mainContent.Link);
            var contentInsideMainWebSite = subContent.Descendants("item").ToList();

            foreach (var item in contentInsideMainWebSite)
            {
                var subContent = new WebSiteContent()
                {
                    Guid = item.Descendants("guid").FirstOrDefault()?.Value,
                    Title = item.Descendants("title").FirstOrDefault()?.Value,
                    Description = item.Descendants("description").FirstOrDefault()?.Value,
                    Image = item.Descendants("enclosure").Attributes("url").FirstOrDefault()?.Value,
                    Links = item.Descendants("link").FirstOrDefault()?.Value,
                    DateOfPublication = item.Descendants("pubDate").FirstOrDefault()?.Value,
                    Category = item.Descendants("category").FirstOrDefault()?.Value,
                };
                subContentList.Add(subContent);
            }
            return subContentList;
        }

        private void GetAllRssLinksFromWebSite()
        {
            var links = _documentNode.Descendants("a")
                         .Where(d => d.Attributes["href"]?.Value
                         .EndsWith("feed") == true || d.Attributes["href"]?.Value
                         .EndsWith(".xml") == true)
                         .Select(s => s.InnerHtml)
                         .ToList();

            _links = links;
        }
    }
}
