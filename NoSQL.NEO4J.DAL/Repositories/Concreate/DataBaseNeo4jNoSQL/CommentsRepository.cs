using Neo4jClient;
using NoSQL.Neo4j.DAL.Data;
using NoSQL.Neo4j.DAL.Repositories.Abstract.DataBaseNeo4jNoSQL;
using NoSQL.Neo4j.DTO.Models;

namespace NoSQL.Neo4j.DAL.Repositories.Concreate.DataBaseNeo4jNoSQL
{
    public class CommentsRepository : ICommentsRepository
    {
        protected Driver _driver;
        public CommentsRepository(Driver driver) 
        {
            _driver = driver;
        }

        public Driver Driver { get { return _driver; } }

        public void Create(Comment entity)
        {
            _driver.GraphClient.Cypher
                .Create("(c:Comment $Comment)")
                .WithParam("Comment", entity)
                .ExecuteWithoutResultsAsync().Wait();
        }

        public void Delete(Comment entity)
        {
            _driver.GraphClient.Cypher
                .Match("(c:Comment {commentid: $CommentId})")
                .WithParam("CommentId", entity.CommentId)
                .DetachDelete("c")
                .ExecuteWithoutResultsAsync().Wait();
        }
    }
}
