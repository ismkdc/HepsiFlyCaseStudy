using MongoDB.Driver;

namespace HepsiFlyCaseStudy.Models;

public class MongoDBContext
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase _mongoDatabase;

    public MongoDBContext(IConfiguration configuration)
    {
        _configuration = configuration;

        var connectionString = _configuration.GetValue<string>("MongoDBConfiguration:ConnectionString");
        var db = _configuration.GetValue<string>("MongoDBConfiguration:Database");

        var client = new MongoClient(connectionString);
        _mongoDatabase = client.GetDatabase(db);
    }

    public IMongoCollection<Product> Products => _mongoDatabase.GetCollection<Product>(nameof(Products));
    public IMongoCollection<Category> Categories => _mongoDatabase.GetCollection<Category>(nameof(Categories));
}