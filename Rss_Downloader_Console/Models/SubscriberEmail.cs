using Postal;
using Rss_Downloader.Models;
using System.Collections.Generic;

namespace Rss_Downloader_Console.Models
{
    public class SubscriberEmail : Email
    {
        public string EmailAddress { get; set; }
        public List<RSSDocumentSingle> SubscriberList { get; set; }
    }
}
