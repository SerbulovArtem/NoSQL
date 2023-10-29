using Neo4jClient;
using NoSQL.Neo4j.DAL.Data;
using NoSQL.Neo4j.DAL.Repositories.Abstract.DataBaseNeo4jNoSQL;
using NoSQL.Neo4j.DTO.Models;

namespace NoSQL.Neo4j.DAL.Repositories.Concreate.DataBaseNeo4jNoSQL
{
    public class UsersRepository : IUsersRepository
    {
        protected Driver _driver;
        public UsersRepository(Driver driver)
        {
            _driver = driver;
        }

        public Driver Driver { get { return _driver; } }

        public void Create(User entity)
        {
            _driver.GraphClient.Cypher
                .Create("(u:User $User)")
                .WithParam("User", entity)
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void Delete(User entity)
        {
            _driver.GraphClient.Cypher
                .Match("(u:User {userId: $UserId})")
                .WithParam("UserId", entity.UserId)
                .DetachDelete("u")
                .ExecuteWithoutResultsAsync().Wait();
        }
    }
}
