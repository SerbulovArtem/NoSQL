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
    public class UsersRepository : IUsersRepository
    {
        protected Driver _driver;
        protected IMongoCollection<User> _collection;
        public UsersRepository(Driver driver)
        {
            _driver = driver;
            _collection = _driver.Users;
        }

        public void Create(User entity)
        {
            _collection.InsertOne(entity);
        }

        public void Replace(User entity)
        {
            _collection.ReplaceOne(p => p.Id == entity.Id, entity);
        }

        public void Delete(User entity)
        {
            _collection.DeleteOne(p => p.Id == entity.Id);
        }

        public IMongoCollection<User> GetCollection()
        {
            return _collection;
        }
    }
}
