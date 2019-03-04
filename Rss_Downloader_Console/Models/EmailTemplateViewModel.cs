using Rss_Downloader.Models;
using System.Collections.Generic;

namespace Rss_Downloader_Console.Models
{
    internal class EmailTemplateViewModel
    {
        public string EmailAddress { get; set; }
        public List<string> SubscriberList { get; set; }

        public List<RssDocumentSingle> RssList { get; set; }
    }
}
