using System.Collections.Generic;

namespace Rss_Downloader_Console.Models
{
    public class SubscriberEmail
    {
        public string EmailAddress { get; set; }
        public List<string> SubscriberList { get; set; }
    }
}
