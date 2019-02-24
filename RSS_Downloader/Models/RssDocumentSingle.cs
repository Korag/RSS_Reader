using System.Collections.Generic;


namespace RSS_Downloader.Models
{
    public class RSSDocumentSingle
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string LastUpdate { get; set; }
        public List<RssDocumentItem> RssDocumentContent { get; set; }
    }
}
