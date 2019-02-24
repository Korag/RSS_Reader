using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace RSS_Downloader
{
    public class WebSiteContentDownloader
    {
        private readonly HtmlNode _documentNode;

        public WebSiteContentDownloader(string path)
        {
            var htmlWeb = new HtmlWeb();
            _documentNode = htmlWeb.Load(path).DocumentNode;
        }

        public List<string> GetAllRssLinksFromWebSite()
        {
            var links = _documentNode.Descendants("a")
                         .Where(d => d.Attributes["href"]?.Value
                         .EndsWith("feed") == true || d.Attributes["href"]?.Value
                         .EndsWith(".xml") == true)
                         .Select(s => s.InnerHtml)
                         .ToList();
            return links;
        }

    }
}
