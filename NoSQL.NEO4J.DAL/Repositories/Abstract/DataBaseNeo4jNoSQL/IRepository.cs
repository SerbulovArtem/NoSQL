using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQL.Neo4j.DAL.Repositories.Abstract.DataBaseNeo4jNoSQL
{
    public interface IRepository<T>
    {
        void Create(T entity);

        void Delete(T entity);
    }
}
