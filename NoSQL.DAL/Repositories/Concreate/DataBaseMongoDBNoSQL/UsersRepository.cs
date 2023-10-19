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
