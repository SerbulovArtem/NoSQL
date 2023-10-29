using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace NoSQL.Mongo.DAL.Data
{
    public class Startup
    {
        public static string GetConnectionStringsServer()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("D:\\University\\NoSQL\\NoSQL\\NoSQL.DAL\\Data\\appsettings.json");
            var configuration = builder.Build();
            string server = "SocialNetworkServer";
            var connString = configuration.GetSection("ConnectionStringsServer:" + server).Get<string>();
            return connString;
        }

        public static string GetConnectionStringsDatabase(int type)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("D:\\University\\NoSQL\\NoSQL\\NoSQL.DAL\\Data\\appsettings.json");
            var configuration = builder.Build();
            string database = "";
            if (type == 1)
            {
                database = "SocialNetworkDatabase";
            }
            else if (type == 0)
            {
                database = "SocialNetworkTestDatabase";
            }
            var connString = configuration.GetSection("ConnectionStringsDatabase:" + database).Get<string>();
            return connString;
        }
    }
}
