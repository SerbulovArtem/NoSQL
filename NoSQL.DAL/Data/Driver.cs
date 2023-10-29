using NoSQL.Mongo.DTO.Models;
using NoSQL.Mongo.DAL.Data;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Net;

namespace NoSQL.Mongo.DAL.Data;


public class Driver
{
    private int _type;
    private MongoClient _client;
    private IMongoDatabase _database;
    public Driver(int type)
    {
        _type = type;
        _client = new MongoClient(Startup.GetConnectionStringsServer());
        _database = _client.GetDatabase(Startup.GetConnectionStringsDatabase(_type));
    }

    public IMongoCollection<User> Users {  
        get 
        { 
            return _database.GetCollection<User>("Users");
        }
    }

    public IMongoCollection<Post> Posts
    {
        get
        {
            return _database.GetCollection<Post>("Posts");
        }
    }
}