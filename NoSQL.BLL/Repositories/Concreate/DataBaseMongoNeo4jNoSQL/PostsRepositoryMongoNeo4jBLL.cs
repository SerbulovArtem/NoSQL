﻿using MongoDB.Driver;
using NoSQL.Mongo.DTO.Models;
using MongoPost = NoSQL.Mongo.DTO.Models.Post;
using Neo4jPost = NoSQL.Neo4j.DTO.Models.Post;
using MongoComment = NoSQL.Mongo.DTO.Models.Comment;
using Neo4jComment = NoSQL.Neo4j.DTO.Models.Comment;
using MongoIPostsRepository = NoSQL.Mongo.DAL.Repositories.Abstract.DataBaseMongoNoSQL.IPostsRepository;
using Neo4jIPostsRepository = NoSQL.Neo4j.DAL.Repositories.Abstract.DataBaseNeo4jNoSQL.IPostsRepository;
using Neo4jICommentsRepository = NoSQL.Neo4j.DAL.Repositories.Abstract.DataBaseNeo4jNoSQL.ICommentsRepository;
using MongoPostsRepository = NoSQL.Mongo.DAL.Repositories.Concreate.DataBaseMongoNoSQL.PostsRepository;
using Neo4jPostsRepository = NoSQL.Neo4j.DAL.Repositories.Concreate.DataBaseNeo4jNoSQL.PostsRepository;
using Neo4jCommentsRepository = NoSQL.Neo4j.DAL.Repositories.Concreate.DataBaseNeo4jNoSQL.CommentsRepository;
using MongoDriver = NoSQL.Mongo.DAL.Data.Driver;
using Neo4jDriver = NoSQL.Neo4j.DAL.Data.Driver;
using NoSQL.Neo4j.DTO.Models;

namespace NoSQL.BLL.Repositories.Concreate.DataBaseMongoNeo4jNoSQL
{
    public class PostsRepositoryMongoNeo4jBLL : MongoIPostsRepository, Neo4jIPostsRepository, Neo4jICommentsRepository
    {
        private MongoPostsRepository _mongoPostsRepository;
        private Neo4jPostsRepository _neo4jPostsRepository;
        private Neo4jCommentsRepository _neo4jCommentsRepository;
        public PostsRepositoryMongoNeo4jBLL(MongoDriver mongoDriver, Neo4jDriver neo4jDriver)
        {
            _mongoPostsRepository = new MongoPostsRepository(mongoDriver);
            _neo4jPostsRepository = new Neo4jPostsRepository(neo4jDriver);
            _neo4jCommentsRepository = new Neo4jCommentsRepository(neo4jDriver);
        }

        public void CreateComment(MongoComment mongoentity, string postid, string userid) 
        {
            var updateDefinition = Builders<MongoPost>.Update.Push("comments", mongoentity);
            _mongoPostsRepository.GetCollection().UpdateOne(p => p.Id == postid, updateDefinition);
        }

        public void CreateCommentRelations(Neo4jComment neo4jentity, string postid, string userid)
        {
            Create(neo4jentity);
            _neo4jCommentsRepository.Driver.GraphClient.Cypher
                .Match("(p:Post {postid: $PostId})", "(c:Comment { commentid: $CommentId})")
                .WithParam("PostId", postid)
                .WithParam("CommentId", neo4jentity.CommentId)
                .Create("(p)-[:HAS_COMMENT]->(c)")
                .ExecuteWithoutResultsAsync().Wait();

            _neo4jCommentsRepository.Driver.GraphClient.Cypher
                .Match("(u:User {userid: $UserId})", "(c:Comment { commentid: $CommentId})")
                .WithParam("UserId", userid)
                .WithParam("CommentId", neo4jentity.CommentId)
                .Create("(u)-[:COMMENTED_BY]->(c)")
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void LikeComment(string postid, string commentid, string userid)
        {
            var filter = Builders<MongoPost>.Filter.Eq(p => p.Id, postid) & Builders<MongoPost>.Filter.ElemMatch(p => p.Comments, c => c.Id == commentid);
            var updateDefinition = Builders<MongoPost>.Update
                .Inc("Comments.$.Like.Likes", 1)
                .AddToSet("Comments.$.Like.UsersIdLiked", userid);

            _mongoPostsRepository.GetCollection().UpdateOne(filter, updateDefinition);

            _neo4jCommentsRepository.Driver.GraphClient.Cypher
                .Match("(u:User {userid: $UserId})", "(c:Comment {commentid: $CommentId})")
                .WithParam("UserId", userid)
                .WithParam("CommentId", commentid)
                .Create("(u)-[:LIKED_BY]->(c)")
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void UnLikeComment(string postid, string commentid, string userid)
        {
            var filter = Builders<MongoPost>.Filter.Eq(p => p.Id, postid) & Builders<MongoPost>.Filter.ElemMatch(p => p.Comments, c => c.Id == commentid);
            var updateDefinition = Builders<MongoPost>.Update
                .Inc("Comments.$.Like.Likes", -1)
                .Pull("Comments.$.Like.UsersIdLiked", userid);

            _mongoPostsRepository.GetCollection().UpdateOne(filter, updateDefinition);

            _neo4jCommentsRepository.Driver.GraphClient.Cypher
                .Match("(u:User {userid: $UserId})-[lb:LIKED_BY]->(c:Comment {commentid: $CommentId})")
                .WithParam("UserId", userid)
                .WithParam("CommentId", commentid)
                .Delete("lb")
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void LikePost(string postid, string userid)
        {
            var updateDefinition = Builders<MongoPost>.Update.Inc("like.likes", 1).Push("like.usersidliked", userid);
            _mongoPostsRepository.GetCollection().UpdateOne(p => p.Id == postid, updateDefinition);

            _neo4jCommentsRepository.Driver.GraphClient.Cypher
                .Match("(u:User {userid: $UserId})", "(p:Post {postid: $PostId})")
                .WithParam("UserId", userid)
                .WithParam("PostId", postid)
                .Create("(u)-[:LIKED_BY]->(p)")
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void UnLikePost(string postid, string userid)
        {
            var updateDefinition = Builders<MongoPost>.Update.Inc("like.likes", -1).Pull("like.usersidliked", userid);
            _mongoPostsRepository.GetCollection().UpdateOne(p => p.Id == postid, updateDefinition);

            _neo4jCommentsRepository.Driver.GraphClient.Cypher
                .Match("(u:User {userid: $UserId})-[lb:LIKED_BY]->(p:Post {postid: $PostId})")
                .WithParam("UserId", userid)
                .WithParam("PostId", postid)
                .Delete("lb")
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void Create(MongoPost entity)
        {
            _mongoPostsRepository.Create(entity);
        }

        public void Delete(MongoPost entity)
        {
            _mongoPostsRepository.Delete(entity);
        }

        public void Replace(MongoPost entity)
        {
            _mongoPostsRepository.Replace(entity);
        }

        public IMongoCollection<MongoPost> GetCollection()
        {
            return _mongoPostsRepository.GetCollection();
        }

        public void Create(Neo4jPost entity)
        {
            _neo4jPostsRepository.Create(entity);
        }

        public void Delete(Neo4jPost entity)
        {
            _neo4jPostsRepository.Delete(entity);
        }

        public void Create(Neo4jComment entity)
        {
            _neo4jCommentsRepository.Create(entity);
        }

        public void Delete(Neo4jComment entity)
        {
            _neo4jCommentsRepository.Delete(entity);
        }

        public void CreateUserPostRelationship(string userid, string postid)
        {
            _neo4jCommentsRepository.Driver.GraphClient.Cypher
                .Match("(u:User {userid: $UserId})", "(p:Post { postid: $PostId})")
                .WithParam("UserId", userid)
                .WithParam("PostId", postid)
                .Create("(u)-[:POSTED_BY]->(p)")
                .ExecuteWithoutResultsAsync().Wait();
        }
    }
}
