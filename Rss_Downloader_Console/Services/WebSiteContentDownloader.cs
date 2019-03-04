using HtmlAgilityPack;
using Rss_Downloader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Rss_Downloader.Services
{
    public interface IWebSiteContentDownloader
    {
        List<RSSDocumentSingle> GetAllDocuments();
        void GetSubContentOfSingleDocument(RSSDocumentSingle mainContent);
    }

    public class WebSiteContentDownloader : IWebSiteContentDownloader
    {
        private readonly HtmlNode _documentNode;
        private Dictionary<string,string> _newsLinks;

        public WebSiteContentDownloader(string path)
        {
            var htmlWeb = new HtmlWeb();
            _documentNode = htmlWeb.Load(path).DocumentNode;
            _newsLinks = GetLinksFromDivWithSpecyficClassName();
        }

        public List<RSSDocumentSingle> GetAllDocuments()
        {
            List<RSSDocumentSingle> tempWebSites = new List<RSSDocumentSingle>();
            foreach (var link in _newsLinks)
            {
                var mainContent = XElement.Load(link.Key);
                RSSDocumentSingle newWebSite = new RSSDocumentSingle()
                {
                    Title = link.Value,
                    Description = mainContent.Descendants("description").FirstOrDefault()?.Value,
                    Image = mainContent.Descendants("image").Descendants("url").FirstOrDefault()?.Value,
                    Link = link.Key,
                    LastUpdate = mainContent.Descendants("lastBuildDate").FirstOrDefault()?.Value,
                    Flag = link.Value.Contains("podcast") ? "podcast" : "text",
                    RssDocumentContent = new List<RssDocumentItem>(),
                    LastFetched = DateTime.UtcNow.AddHours(1)
                };
                tempWebSites.Add(newWebSite);
            }
            return tempWebSites;
        }

        public void GetSubContentOfSingleDocument(RSSDocumentSingle mainContent)
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

        public List<RSSDocumentSingle> GetDocumentsWithNewContentAvailable(List<RSSDocumentSingle> rssDocumentsFromDb)
        {
            List<RSSDocumentSingle> documentsWithNewContentAvailable = new List<RSSDocumentSingle>();

            foreach (var item in rssDocumentsFromDb)
            {
                var subContent = XElement.Load(item.Link);
                var contentInsideMainWebSite = subContent.Descendants("item").ToList();

                var date = contentInsideMainWebSite
                            .FirstOrDefault()?.Descendants("pubDate").FirstOrDefault()?.Value;

                if (CheckIfNewConteIsAvailable(item, date))
                {
                    documentsWithNewContentAvailable.Add(item);
                }
            }
            return documentsWithNewContentAvailable;
        }

        public List<RSSDocumentSingle> AddNewContentToDocumentsInDb
            (List<RSSDocumentSingle> documentsWithNewContentAvailable)
        {
            List<RSSDocumentSingle> documentsWithNewContent = new List<RSSDocumentSingle>();

            foreach (var document in documentsWithNewContentAvailable)
            {
                var subContent = XElement.Load(document.Link);
                var contentInsideMainWebSite = subContent.Descendants("item").ToList();

                foreach (var item in contentInsideMainWebSite)
                {
                    var checkDate = item.Descendants("pubDate").FirstOrDefault()?.Value;
                    if (document.LastFetched < Convert.ToDateTime(checkDate))
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
                        document.RssDocumentContent.Add(rssDocumentContent);
                    }
                }
                documentsWithNewContent.Add(document);
            }
            return documentsWithNewContent;
        }

        private bool CheckIfNewConteIsAvailable(RSSDocumentSingle rssDocument, string date)
        {
            bool result = false;

            var checkDate = Convert.ToDateTime(date);

            if (rssDocument.LastFetched < checkDate)
            {
                return true;
            }
            return result;
        }

        private Dictionary<string, string> GetLinksFromDivWithSpecyficClassName()
        {
            Dictionary<string, string> titleswithLinks = new Dictionary<string, string>();

            var liElementsInsideDiv = _documentNode.Descendants("li")
                         .Where(d => d.Attributes["class"]?.Value
                         .StartsWith("i") == true).ToList();

            foreach (var liElement in liElementsInsideDiv)
            {
                var title = liElement.Descendants("div")
                            .Where(d => d.Attributes["class"]?.Value
                            .Equals("title") == true).FirstOrDefault().InnerHtml;

                var url = liElement.Descendants("div")
                            .Where(d => d.Attributes["class"]?.Value
                            .Equals("url") == true).FirstOrDefault()
                            .Descendants("a").FirstOrDefault()?.InnerHtml;

                titleswithLinks.Add(url, title);
            }
            return titleswithLinks;
        }

    }
}
