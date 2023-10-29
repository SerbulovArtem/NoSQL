using NoSQL.Neo4j.DTO.Models;
using NoSQL.Neo4j.DAL.Data;
using Neo4jClient.Cypher;
using Neo4jClient;
using Neo4j.Driver;
using System;
using System.Net;

namespace NoSQL.Neo4j.DAL.Data;


public class Driver
{
    private GraphClient _client;
    public Driver()
    {
        _client = new GraphClient(new Uri(Startup.GetConnectionStringsServer()), Startup.GetUserName(), Startup.GetPassword());
        _client.ConnectAsync().Wait();
    }

    public GraphClient GraphClient
    {
        get
        {
            return _client;
        }
    }
}