﻿using Postal;
using System.Collections.Generic;

namespace Rss_Downloader_Console.Models
{
    public class SubscriberEmail : Email
    {
        public object Id { get; set; }
        public string EmailAddress { get; set; }
        public List<string> SubscriberList { get; set; }
    }
}
