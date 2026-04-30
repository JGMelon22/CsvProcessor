using Amazon.Runtime;
using Amazon.S3;
using CsvProcessor.Shared.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CsvProcessor.Lambda.Extensions;

public static class IocExtensions
{
    public static void AddAwsServices(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var aws = sp.GetRequiredService<IOptions<AwsSettings>>().Value;

            return new AmazonS3Client(
                new BasicAWSCredentials(aws.AccessKey, aws.SecretKey),
                new AmazonS3Config
                {
                    ServiceURL = aws.ServiceUrl,
                    AuthenticationRegion = aws.AuthenticationRegion,
                    ForcePathStyle = true
                }
            );
        });
    }

    public static void AddMongoDb(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });
    }
}