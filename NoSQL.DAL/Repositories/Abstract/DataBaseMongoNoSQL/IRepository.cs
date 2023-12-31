﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace NoSQL.Mongo.DAL.Repositories.Abstract.DataBaseMongoNoSQL
{
    public interface IRepository<T>
    {
        void Create(T entity);

        void Delete(T entity);

        void Replace(T entity);

        IMongoCollection<T> GetCollection();
    }
}
