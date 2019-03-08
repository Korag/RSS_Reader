using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace RssModelsLib.Models
{
    public class RssDocumentSingle
    {
        [BsonId]
        public ObjectId Id { get; set; }
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