using Neo4jClient;
using NoSQL.Neo4j.DAL.Data;
using NoSQL.Neo4j.DAL.Repositories.Abstract.DataBaseNeo4jNoSQL;
using NoSQL.Neo4j.DTO.Models;

namespace NoSQL.Neo4j.DAL.Repositories.Concreate.DataBaseNeo4jNoSQL
{
    public class PostsRepository : IPostsRepository
    {
        protected Driver _driver;
        public PostsRepository(Driver driver) 
        {
            _driver = driver;
        }

        public Driver Driver { get { return _driver; } }

        public void Create(Post entity)
        {
            _driver.GraphClient.Cypher
                .Create("(p:Post $Post)")
                .WithParam("Post", entity)
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void Delete(Post entity)
        {
            _driver.GraphClient.Cypher
                .Match("(p:Post {postid: $PostId})")
                .WithParam("PostId", entity.PostId)
                .DetachDelete("p")
                .ExecuteWithoutResultsAsync().Wait();
        }
    }
}
