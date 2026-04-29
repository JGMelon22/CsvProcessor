using CsvProcessor.Shared.Models;

namespace CsvProcessor.Lambda.Services.Interfaces;

public interface IMongoDbService
{
    Task InsertMany(IEnumerable<Product> products, CancellationToken cancellationToken = default);
}