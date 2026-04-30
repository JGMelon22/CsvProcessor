using CsvProcessor.Lambda.Services;
using CsvProcessor.Lambda.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CsvProcessor.Lambda.Extensions;

public static class LambdaServiceExtensions
{
    public static void AddLambdaServices(this IServiceCollection services)
    {
        services.AddScoped<IS3DownloadService, S3DownloadService>();
        services.AddScoped<IMongoDbService, MongoDbService>();
        services.AddScoped<ICsvReaderService, CsvReaderService>();
    }
}