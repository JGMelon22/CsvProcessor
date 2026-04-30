using Amazon.Runtime;
using Amazon.S3;
using CsvProcessor.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CsvProcessor.Lambda.Extensions;

public static class IocExtensions
{
    public static void AddS3Service(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonS3>(sp =>
        {
            AwsSettings awsSettings = sp
                .GetRequiredService<IOptions<AwsSettings>>()
                .Value;

            return new AmazonS3Client(
                new BasicAWSCredentials(awsSettings.AccessKey, awsSettings.SecretKey),
                new AmazonS3Config
                {
                    ServiceURL = awsSettings.ServiceUrl,
                    AuthenticationRegion = awsSettings.AuthenticationRegion,
                    ForcePathStyle = true
                }
            );
        });
    }

    public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddSingleton<IMongoClient>(sp =>
        // {
        //     var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        //     return new MongoClient(settings.ConnectionString);
        // });

        services.Configure<MongoDbSettings>(
            configuration.GetSection(MongoDbSettings.SectionName));
    }
}