using HtmlAgilityPack;
using RSS_Downloader.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RSS_Downloader
{
    public class WebSiteContentDownloader
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
                var feed = XElement.Load(link);
                WebSite newWebSite = new WebSite()
                {
                    Title = feed.Descendants("title").FirstOrDefault()?.Value,
                    Description = feed.Descendants("description").FirstOrDefault()?.Value,
                    Image = feed.Descendants("image").Descendants("url").FirstOrDefault()?.Value,
                    Link = feed.Descendants("link").FirstOrDefault()?.Value,
                    LastUpdate = feed.Descendants("lastBuildDate").FirstOrDefault()?.Value,
                    SubPages = new List<WebSiteContent>()
                };
                tempWebSites.Add(newWebSite);
            }
            return tempWebSites;
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
