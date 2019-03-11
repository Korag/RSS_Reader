using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace RssModelsLib.Models
{
    public class SubscriberEmail
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string EmailAddress { get; set; }
        public List<string> SubscriberList { get; set; }
    }
}
