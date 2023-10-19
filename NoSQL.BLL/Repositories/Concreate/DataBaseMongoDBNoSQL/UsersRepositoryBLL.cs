using MongoDB.Driver;
using NoSQL.DAL.Data;
using NoSQL.DAL.Repositories.Abstract.DataBaseMongoDBNoSQL;
using NoSQL.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoSQL.DAL.Repositories.Concreate.DataBaseMongoDBNoSQL;

namespace NoSQL.BLL.Repositories.Concreate.DataBaseMongoDBNoSQL
{
    public class UsersRepositoryBLL : UsersRepository
    {
        public UsersRepositoryBLL(Driver driver) : base(driver)
        {

        }
    }
}
