using MongoDB.Driver;
using NoSQL.DAL.Data;
using NoSQL.DAL.Repositories.Abstract.DataBaseMongoDBNoSQL;
using NoSQL.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQL.DAL.Repositories.Concreate.DataBaseMongoDBNoSQL
{
    public class PostsRepository : IPostsRepository
    {
        protected Driver _driver;
        protected IMongoCollection<Post> _collection;
        public PostsRepository(Driver driver) 
        {
            _driver = driver;
            _collection = _driver.Posts;
        }

        public void Create(Post entity)
        {
            _collection.InsertOne(entity);
        }

        public void Replace(Post entity)
        {
            _collection.ReplaceOne(p => p.Id == entity.Id, entity);
        }

        public void Delete(Post entity)
        {
            _collection.DeleteOne(p => p.Id == entity.Id);
        }

        public IMongoCollection<Post> GetCollection()
        {
            return _collection;
        }
    }
}
