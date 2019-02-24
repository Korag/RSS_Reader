using HtmlAgilityPack;
using RSS_Downloader.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RSS_Downloader
{
    public interface IWebSiteContentDownloader
    {
        List<RSSDocumentSingle> GetContentFromWebSite();
        void GetSubContentOfMainSite(RSSDocumentSingle mainContent);
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

        public List<RSSDocumentSingle> GetContentFromWebSite()
        {
            List<RSSDocumentSingle> tempWebSites = new List<RSSDocumentSingle>();
            foreach (var link in _links)
            {
                var mainContent = XElement.Load(link);
                RSSDocumentSingle newWebSite = new RSSDocumentSingle()
                {
                    Title = mainContent.Descendants("title").FirstOrDefault()?.Value,
                    Description = mainContent.Descendants("description").FirstOrDefault()?.Value,
                    Image = mainContent.Descendants("image").Descendants("url").FirstOrDefault()?.Value,
                    Link = mainContent.Descendants("link").FirstOrDefault()?.Value,
                    LastUpdate = mainContent.Descendants("lastBuildDate").FirstOrDefault()?.Value,
                    RssDocumentContent = new List<RssDocumentItem>()
                };
                tempWebSites.Add(newWebSite);

            }
            return tempWebSites;
        }

        public void GetSubContentOfMainSite(RSSDocumentSingle mainContent)
        {
            List<RssDocumentItem> subContentList = new List<RssDocumentItem>();

            var subContent = XElement.Load(mainContent.Link);
            var contentInsideMainWebSite = subContent.Descendants("item").ToList();

            foreach (var item in contentInsideMainWebSite)
            {
                var rssDocumentContent = new RssDocumentItem()
                {
                    Guid = item.Descendants("guid").FirstOrDefault()?.Value,
                    Title = item.Descendants("title").FirstOrDefault()?.Value,
                    Description = item.Descendants("description").FirstOrDefault()?.Value,
                    Image = item.Descendants("enclosure").Attributes("url").FirstOrDefault()?.Value,
                    Links = item.Descendants("link").FirstOrDefault()?.Value,
                    DateOfPublication = item.Descendants("pubDate").FirstOrDefault()?.Value,
                    Category = item.Descendants("category").FirstOrDefault()?.Value,
                };
                subContentList.Add(rssDocumentContent);
            }
            mainContent.RssDocumentContent = subContentList;
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
