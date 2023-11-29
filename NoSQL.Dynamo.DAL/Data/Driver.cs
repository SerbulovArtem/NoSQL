using System;
using System.Net;

using NoSQL.Dynamo.DAL.Data;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;

namespace NoSQL.Dynamo.DAL.Data;


public class Driver
{
    private AmazonDynamoDBConfig _dBConfig;
    private AmazonDynamoDBClient _database;
    public Driver()
    {
        _dBConfig = new AmazonDynamoDBConfig();
        _dBConfig.ServiceURL = Startup.GetConnectionStringsServer();
        _database = new AmazonDynamoDBClient(_dBConfig);
    }

    public AmazonDynamoDBClient Client{  
        get 
        { 
            return _database;
        }
    }
}