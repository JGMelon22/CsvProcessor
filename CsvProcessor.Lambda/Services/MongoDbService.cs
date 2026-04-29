using CsvProcessor.Lambda.Services.Interfaces;
using CsvProcessor.Shared.Models;
using CsvProcessor.Shared.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CsvProcessor.Lambda.Services;

public class MongoDbService(IMongoClient mongoClient, IOptions<MongoDbSettings> options) : IMongoDbService
{
    private readonly IMongoCollection<Product> _collection = mongoClient
        .GetDatabase(options.Value.DatabaseName)
        .GetCollection<Product>(options.Value.CollectionName);

    public async Task InsertMany(IEnumerable<Product> products, CancellationToken cancellationToken = default)
    {
        await _collection.InsertManyAsync(products, cancellationToken: cancellationToken);
    }
}