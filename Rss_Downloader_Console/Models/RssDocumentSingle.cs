using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;


namespace Rss_Downloader.Models
{
    public class RSSDocumentSingle
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public object Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string LastUpdate { get; set; }
        public string Flag { get; set; }
        public DateTime LastFetched { get; set; }
        public List<RssDocumentItem> RssDocumentContent { get; set; }

    }
}