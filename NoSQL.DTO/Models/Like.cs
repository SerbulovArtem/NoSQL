using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQL.Mongo.DTO.Models
{
    public class Like
    {
        [BsonElement("usersidliked")]
        public List<string> UsersIdLiked { get; set; }

        [BsonElement("likes")]
        public int Likes { get; set; }
    }
}
