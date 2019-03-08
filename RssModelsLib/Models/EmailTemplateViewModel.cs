using System.Collections.Generic;

namespace RssModelsLib.Models
{
    internal class EmailTemplateViewModel
    {
        public string EmailAddress { get; set; }
        public List<string> SubscriberList { get; set; }

        public List<RssDocumentSingle> RssList { get; set; }
    }
}
