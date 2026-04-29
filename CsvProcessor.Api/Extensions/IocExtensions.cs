using CsvProcessor.Api.Services;
using CsvProcessor.Api.Services.Interfaces;

namespace CsvProcessor.Api.Extensions;

public static class IocExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IS3Service, S3Service>();
        services.AddScoped<ISqsService, SqsService>();
        services.AddScoped<ICsvReaderService, CsvReaderService>();
    }
}