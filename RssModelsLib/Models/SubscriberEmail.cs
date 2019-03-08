using System.Collections.Generic;

namespace RssModelsLib.Models
{
    public class SubscriberEmail
    {
        public string EmailAddress { get; set; }
        public List<string> SubscriberList { get; set; }
    }
}
