using HtmlAgilityPack;
using Rss_Downloader.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Rss_Downloader.Services
{
    public interface IWebSiteContentDownloader
    {
        List<RSSDocumentSingle> GetAllDocumentsFromWebSite();
        List<RSSDocumentSingle> GetDocumentsFromLinks(List<string> links, string newsType);
        void GetSubContentOfMainSite(RSSDocumentSingle mainContent);
    }

    public class WebSiteContentDownloader : IWebSiteContentDownloader
    {
        private readonly HtmlNode _documentNode;
        private List<string> _allLinksFromWebSite;
        private List<string> _podcastLinks;
        private List<string> _textNewsLinks;


        public WebSiteContentDownloader(string path)
        {
            var htmlWeb = new HtmlWeb();
            _documentNode = htmlWeb.Load(path).DocumentNode;
            GetAllRssLinksFromWebSite();
        }

        public List<RSSDocumentSingle> GetAllDocumentsFromWebSite()
        {
            List<RSSDocumentSingle> AllWebSitesContent = new List<RSSDocumentSingle>();
            var podcasts = GetDocumentsFromLinks(_podcastLinks,"Podcast");
            var newsTexts = GetDocumentsFromLinks(_textNewsLinks, "Text");

            AllWebSitesContent = podcasts;
            AllWebSitesContent.Concat(newsTexts);

            return AllWebSitesContent;

        }

        public void GetSubContentOfMainSite(RSSDocumentSingle mainContent)
        {
            List<RssDocumentItem> subContentList = new List<RssDocumentItem>();

            var subContent = XElement.Load(mainContent.Link + "feed");
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

        public List<RSSDocumentSingle> GetDocumentsFromLinks(List<string> links, string newsType)
        {
            List<RSSDocumentSingle> tempWebSites = new List<RSSDocumentSingle>();
            foreach (var link in links)
            {
                var mainContent = XElement.Load(link);
                RSSDocumentSingle newWebSite = new RSSDocumentSingle()
                {
                    Title = mainContent.Descendants("title").FirstOrDefault()?.Value,
                    Description = mainContent.Descendants("description").FirstOrDefault()?.Value,
                    Image = mainContent.Descendants("image").Descendants("url").FirstOrDefault()?.Value,
                    Link = mainContent.Descendants("link").FirstOrDefault()?.Value,
                    LastUpdate = mainContent.Descendants("lastBuildDate").FirstOrDefault()?.Value,
                    Flag = newsType,
                    RssDocumentContent = new List<RssDocumentItem>()
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

            _allLinksFromWebSite = links;
        }

        private void GetTextNewsRssLinks()
        {
            var MainDiv = _documentNode.Descendants("div")
                         .Where(d => d.Attributes["class"]?.Value
                         .Equals("box channels") == true).FirstOrDefault();

            var links = MainDiv.Descendants("a")
                        .Where(d => d.Attributes["href"]?.Value
                        .EndsWith("feed") == true || d.Attributes["href"]?.Value
                        .EndsWith(".xml") == true)
                        .Select(s => s.InnerHtml)
                        .ToList();

            _textNewsLinks = links;
        }

        private void GetPodcastNewsRssLinks()
        {
            var MainDiv = _documentNode.Descendants("div")
                         .Where(d => d.Attributes["class"]?.Value
                         .Equals("box channels podcast") == true).FirstOrDefault();

            var links = MainDiv.Descendants("a")
                        .Where(d => d.Attributes["href"]?.Value
                        .EndsWith("feed") == true || d.Attributes["href"]?.Value
                        .EndsWith(".xml") == true)
                        .Select(s => s.InnerHtml)
                        .ToList();

            _podcastLinks = links;
        }
    }
}
