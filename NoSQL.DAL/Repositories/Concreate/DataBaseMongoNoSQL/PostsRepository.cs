using MongoDB.Driver;
using NoSQL.Mongo.DAL.Data;
using NoSQL.Mongo.DAL.Repositories.Abstract.DataBaseMongoNoSQL;
using NoSQL.Mongo.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQL.Mongo.DAL.Repositories.Concreate.DataBaseMongoNoSQL
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
