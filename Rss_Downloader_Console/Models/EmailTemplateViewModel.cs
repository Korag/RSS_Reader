using System.Collections.Generic;
using Rss_Downloader.Models;

namespace Rss_Downloader_Console.Models
{
    class EmailTemplateViewModel
    {
        public string EmailAddress { get; set; }
        public List<string> SubscriberList { get; set; }

        public List<RSSDocumentSingle> RssList { get; set; }
    }
}
