using CsvProcessor.Lambda.Services.Interfaces;
using CsvProcessor.Shared.Models;
using CsvProcessor.Shared.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CsvProcessor.Lambda.Services;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoCollection<Product> _collection;

    public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        MongoClient mongoClient = new(
            mongoDbSettings.Value.ConnectionString);

        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(
            mongoDbSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<Product>(
            mongoDbSettings.Value.CollectionName
        );
    }
    // private readonly IMongoCollection<Product> _collection = mongoClient
    //     .GetDatabase(options.Value.DatabaseName)
    //     .GetCollection<Product>(options.Value.CollectionName);

    public async Task InsertManyAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default)
    {
        await _collection.InsertManyAsync(products, cancellationToken: cancellationToken);
    }
}