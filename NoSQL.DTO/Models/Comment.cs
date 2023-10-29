using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQL.Mongo.DTO.Models
{
    [BsonIgnoreExtraElements]
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("body")]
        public string CommentBody { get; set; }

        [BsonElement("ownerid")]
        public string OwnerId { get; set; }

        [BsonElement("date")]
        public object CommentCreatedDate { get; set; }

        [BsonElement("like")]
        public Like Like { get; set; }

        public Comment()
        {
            this.Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
