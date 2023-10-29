using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace NoSQL.Neo4j.DAL.Data
{
    public class Startup
    {
        public static string GetConnectionStringsServer()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("D:\\University\\NoSQL\\NoSQL\\NoSQL.NEO4J.DAL\\Data\\appsettings.json");
            var configuration = builder.Build();
            string server = "SocialNetworkServer";
            var connString = configuration.GetSection("ConnectionStringsServer:" + server).Get<string>();
            return connString;
        }

        public static string GetUserName()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("D:\\University\\NoSQL\\NoSQL\\NoSQL.NEO4J.DAL\\Data\\appsettings.json");
            var configuration = builder.Build();
            string usernameSection = "UserName";
            var userName = configuration.GetSection("UserNameServer:" + usernameSection).Get<string>();
            return userName;
        }

        public static string GetPassword()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("D:\\University\\NoSQL\\NoSQL\\NoSQL.NEO4J.DAL\\Data\\appsettings.json");
            var configuration = builder.Build();
            string passwordSection = "Password";
            var password = configuration.GetSection("PasswordServer:" + passwordSection).Get<string>();
            return password;
        }
    }
}
