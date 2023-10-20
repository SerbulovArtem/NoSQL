using MongoDB.Driver;
using NoSQL.DAL.Data;
using NoSQL.DAL.Repositories.Abstract.DataBaseMongoDBNoSQL;
using NoSQL.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQL.DAL.Repositories.Concreate.DataBaseMongoDBNoSQL;
using MongoDB.Bson;
using System.ComponentModel.Design;

namespace NoSQL.BLL.Repositories.Concreate.DataBaseMongoDBNoSQL
{
    public class PostsRepositoryBLL : PostsRepository
    {
        public PostsRepositoryBLL(Driver driver) : base(driver)
        {   }

        public void CreateComment(Comment entity, string postid) 
        {
            var updateDefinition = Builders<Post>.Update.Push("comments", entity);
            _collection.UpdateOne(p => p.Id == postid, updateDefinition);
        }

        public void LikeComment(string postid, string commentid, string userid)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.Id, postid) & Builders<Post>.Filter.ElemMatch(p => p.Comments, c => c.Id == commentid);
            var updateDefinition = Builders<Post>.Update
                .Inc("Comments.$.Like.Likes", 1)
                .AddToSet("Comments.$.Like.UsersIdLiked", userid);

            _collection.UpdateOne(filter, updateDefinition);
        }

        public void UnLikeComment(string postid, string commentid, string userid)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.Id, postid) & Builders<Post>.Filter.ElemMatch(p => p.Comments, c => c.Id == commentid);
            var updateDefinition = Builders<Post>.Update
                .Inc("Comments.$.Like.Likes", -1)
                .Pull("Comments.$.Like.UsersIdLiked", userid);

            _collection.UpdateOne(filter, updateDefinition);
        }

        public void LikePost(string postid, string userid)
        {
            var updateDefinition = Builders<Post>.Update.Inc("like.likes", 1).Push("like.usersidliked", userid);
            _collection.UpdateOne(p => p.Id == postid, updateDefinition);
        }

        public void UnLikePost(string postid, string userid)
        {
            var updateDefinition = Builders<Post>.Update.Inc("like.likes", -1).Pull("like.usersidliked", userid);
            _collection.UpdateOne(p => p.Id == postid, updateDefinition);
        }
    }
}
