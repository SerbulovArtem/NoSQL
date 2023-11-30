using System;
using System.Net;

using ServiceStack.Redis;

namespace NoSQL.Redis.DAL.Data;


public class Driver
{
    private RedisClient _database;
    public Driver()
    {
        string host = Startup.GetConnectionStringsServer();
        int port = Startup.GetConnectionStringsServerPort();
        _database = new RedisClient(host, port);
    } 

    public RedisClient Client
    {  
        get 
        { 
            return _database;
        }
    }
}