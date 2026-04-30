using CsvProcessor.Shared.Models;

namespace CsvProcessor.Lambda.Services.Interfaces;

public interface IMongoDbService
{
    Task InsertManyAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default);
}