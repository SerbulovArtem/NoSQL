using MongoDB.Driver;
using NoSQL.Mongo.DAL.Data;
using NoSQL.Mongo.DTO.Models;
using NoSQL.Mongo.DAL.Repositories.Concreate.DataBaseMongoNoSQL;
using MongoUser = NoSQL.Mongo.DTO.Models.User;
using Neo4jUser = NoSQL.Neo4j.DTO.Models.User;
using MongoIUsersRepository = NoSQL.Mongo.DAL.Repositories.Abstract.DataBaseMongoNoSQL.IUsersRepository;
using Neo4jIUsersRepository = NoSQL.Neo4j.DAL.Repositories.Abstract.DataBaseNeo4jNoSQL.IUsersRepository;
using MongoUsersRepository = NoSQL.Mongo.DAL.Repositories.Concreate.DataBaseMongoNoSQL.UsersRepository;
using Neo4jUsersRepository = NoSQL.Neo4j.DAL.Repositories.Concreate.DataBaseNeo4jNoSQL.UsersRepository;
using MongoDriver = NoSQL.Mongo.DAL.Data.Driver;
using Neo4jDriver = NoSQL.Neo4j.DAL.Data.Driver;
using NoSQL.Neo4j.DTO.Models;
using Neo4jClient.Cypher;

namespace NoSQL.BLL.Repositories.Concreate.DataBaseMongoNeo4jNoSQL
{
    public class UsersRepositoryMongoNeo4jBLL : MongoIUsersRepository, Neo4jIUsersRepository
    {
        private MongoUsersRepository _mongoUsersRepository;
        private Neo4jUsersRepository _neo4jUsersRepository;

        public UsersRepositoryMongoNeo4jBLL(MongoDriver mongoDriver, Neo4jDriver neo4jDriver)
        {
            _mongoUsersRepository = new MongoUsersRepository(mongoDriver);
            _neo4jUsersRepository = new Neo4jUsersRepository(neo4jDriver);
        }

        public void AddInterest(string ownerid, string interest)
        {
            var updateDefinition = Builders<MongoUser>.Update.Push("interests", interest);
            _mongoUsersRepository.GetCollection().UpdateOne(p => p.Id == ownerid, updateDefinition);
        }

        public void AddFriend(string ownerid, string userid)
        {
            var updateDefinition = Builders<MongoUser>.Update.Push("friends", userid);
            _mongoUsersRepository.GetCollection().UpdateOne(p => p.Id == ownerid, updateDefinition);

            _neo4jUsersRepository.Driver.GraphClient.Cypher
                .Match("(u1:User {userid: $UserId1})", "(u2:User { userid: $UserId2})")
                .WithParam("UserId1", ownerid)
                .WithParam("UserId2", userid)
                .Create("(u1)-[:HAS_FRIEND]->(u2)")
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void DeleteFriend(string ownerid, string userid)
        {
            var updateDefinition = Builders<MongoUser>.Update.Pull("friends", userid);
            _mongoUsersRepository.GetCollection().UpdateOne(p => p.Id == ownerid, updateDefinition);

            _neo4jUsersRepository.Driver.GraphClient.Cypher
                .Match("(u1:User {userid: $UserId1})-[hf:HAS_FRIEND]->(u2:User { userid: $UserId2})")
                .WithParam("UserId1", ownerid)
                .WithParam("UserId2", userid)
                .Delete("hf")
                .ExecuteWithoutResultsAsync().Wait();
        }

        public int PathLenghtToFriend(string ownerid, string userid)
        {
            var query = _neo4jUsersRepository.Driver.GraphClient.Cypher
                .Match("path = shortestPath((startNode:User {userid: $UserId1})-[*]->(endNode:User {userid: $UserId2}))")
                .WithParam("UserId1", ownerid)
                .WithParam("UserId2", userid)
                .Return(() => Return.As<int>("length(path)"));
            var result = query.ResultsAsync.Result.Single();
            return result;
        }

        public void Create(MongoUser entity)
        {
            _mongoUsersRepository.Create(entity);
        }

        public void Delete(MongoUser entity)
        {
            _mongoUsersRepository.Delete(entity);
        }

        public void Replace(MongoUser entity)
        {
            _mongoUsersRepository.Replace(entity);
        }

        public IMongoCollection<MongoUser> GetCollection()
        {
            return _mongoUsersRepository.GetCollection();
        }

        public void Create(Neo4jUser entity)
        {
            _neo4jUsersRepository.Create(entity);
        }

        public void Delete(Neo4jUser entity)
        {
            _neo4jUsersRepository.Delete(entity);
        }
    }
}
