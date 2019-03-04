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
        List<RssDocumentSingle> GetAllDocumentsWithoutSubContent();

        void FillSingleDocumentWithSubContent(RssDocumentSingle mainContent);

        List<RssDocumentSingle> GetDocumentsWithNewContentAvailable
            (List<RssDocumentSingle> rssDocumentsFromDb);

        List<RssDocumentSingle> AddNewContentToDocuments
            (List<RssDocumentSingle> documentsWithNewContentAvailable);
    }

    public class WebSiteContentDownloader : IWebSiteContentDownloader
    {
        private readonly HtmlNode _documentNode;
        private Dictionary<string, string> _linksWithTitles;

        public WebSiteContentDownloader(string path)
        {
            var htmlWeb = new HtmlWeb();
            _documentNode = htmlWeb.Load(path).DocumentNode;
            _linksWithTitles = GetLinksFromDivWithSpecyficClassName();
        }

        public List<RssDocumentSingle> GetAllDocumentsWithoutSubContent()
        {
            List<RssDocumentSingle> allRssDocumentsWithoutSubContent = new List<RssDocumentSingle>();

            foreach (var link in _linksWithTitles)
            {
                var rssDocumentFromWebSite = XElement.Load(link.Key);
                RssDocumentSingle newWebSite = new RssDocumentSingle()
                {
                    Title = link.Value,
                    Link = link.Key,
                    Flag = link.Key.Contains("podcast") ? "Podcast" : "Text",
                    Description = rssDocumentFromWebSite.Descendants("description").FirstOrDefault()?.Value,
                    Image = rssDocumentFromWebSite.Descendants("image").Descendants("url").FirstOrDefault()?.Value,
                    LastUpdate = rssDocumentFromWebSite.Descendants("lastBuildDate").FirstOrDefault()?.Value,
                    LastFetched = DateTime.UtcNow.AddHours(1),
                    RssDocumentContent = new List<RssDocumentItem>()
                };
                allRssDocumentsWithoutSubContent.Add(newWebSite);
            }
            return allRssDocumentsWithoutSubContent;
        }

        public void FillSingleDocumentWithSubContent(RssDocumentSingle mainContent)
        {
            List<RssDocumentItem> subContentOfSingleRssDocument = new List<RssDocumentItem>();

            var itemsInsideMainRssDocument = XElement.Load(mainContent.Link)
                                                    .Descendants("item").ToList();

            foreach (var item in itemsInsideMainRssDocument)
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
                subContentOfSingleRssDocument.Add(rssDocumentContent);
            }
            mainContent.RssDocumentContent = subContentOfSingleRssDocument;
        }

        public List<RssDocumentSingle> AddNewContentToDocuments
            (List<RssDocumentSingle> documentsWithNewContentAvailable)
        {
            List<RssDocumentSingle> documentsWithNewContent = new List<RssDocumentSingle>();

            foreach (var document in documentsWithNewContentAvailable)
            {
                var subContent = XElement.Load(document.Link).Descendants("item").ToList();

                foreach (var item in subContent)
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

        public List<RssDocumentSingle> GetDocumentsWithNewContentAvailable(List<RssDocumentSingle> rssDocumentsFromDb)
        {
            List<RssDocumentSingle> documentsWithNewContentAvailable = new List<RssDocumentSingle>();

            foreach (var rssDocument in rssDocumentsFromDb)
            {
                var publishDateOfTheLatestSubContent = XElement.Load(rssDocument.Link).Descendants("item")
                                                        .FirstOrDefault()?.Descendants("pubDate")
                                                        .FirstOrDefault()?.Value;

                if (CheckIfNewConteIsAvailable(rssDocument, publishDateOfTheLatestSubContent))
                {
                    documentsWithNewContentAvailable.Add(rssDocument);
                }
            }
            return documentsWithNewContentAvailable;
        }

        private bool CheckIfNewConteIsAvailable(RssDocumentSingle rssDocument, string date)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(date))
            {
                var checkDate = Convert.ToDateTime(date);

                if (rssDocument.LastFetched < checkDate)
                {
                    return true;
                }
            }
            return result;
        }

        private Dictionary<string, string> GetLinksFromDivWithSpecyficClassName()
        {
            Dictionary<string, string> titlesWithLinks = new Dictionary<string, string>();

            var liElementsFromWebSite = _documentNode.Descendants("li")
                                        .Where(d => d.Attributes["class"]?.Value
                                        .StartsWith("i") == true).ToList();

            foreach (var liElement in liElementsFromWebSite)
            {
                var title = liElement.Descendants("div").FirstOrDefault()?.InnerHtml;
                var url = liElement.Descendants("a").FirstOrDefault()?.InnerHtml;

                titlesWithLinks.Add(url, title);
            }
            return titlesWithLinks;
        }

    }
}
