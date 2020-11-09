using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeedingPrecision.Models.Responses
{
    public class HistoryStatus
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id{ get; set; }     
        [BsonElement("recvTime")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime recvTime { get; set; }
        [BsonElement("attrName")]
        public string attrName { get; set; }
        [BsonElement("attrType")]
        public string attrType { get; set; }
        [BsonElement("attrValue")]
        public string attrValue { get; set; }

    }
}
