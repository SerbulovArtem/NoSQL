using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace NoSQL.Redis.DAL.Data
{
    public class Startup
    {
        public static string GetConnectionStringsServer()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("D:\\University\\NoSQL\\NoSQL\\NoSQL.Redis.DAL\\Data\\appsettings.json");
            var configuration = builder.Build();
            string server = "SocialNetworkServer";
            var connString = configuration.GetSection("ConnectionStringsServer:" + server).Get<string>();
            return connString;
        }

        public static int GetConnectionStringsServerPort()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("D:\\University\\NoSQL\\NoSQL\\NoSQL.Redis.DAL\\Data\\appsettings.json");
            var configuration = builder.Build();
            string server = "SocialNetworkPort";
            var connString = configuration.GetSection("ConnectionStringsServer:" + server).Get<int>();
            return connString;
        }
    }
}
