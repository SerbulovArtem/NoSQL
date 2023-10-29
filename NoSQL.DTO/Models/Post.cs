using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;


namespace NoSQL.Mongo.DTO.Models
{
    [BsonIgnoreExtraElements]
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("comments")]
        public List<Comment> Comments { get; set; }

        [BsonElement("date")]
        public object PostCreatedDate { get; set; }

        [BsonElement("body")]
        public string PostBody { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("like")]
        public Like Like { get; set; }

        [BsonElement("ownerid")]
        public string OwnerId { get; set; }
    }
}
