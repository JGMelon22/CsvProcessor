using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using CsvProcessor.Api.Services;
using CsvProcessor.Api.Services.Interfaces;
using CsvProcessor.Shared.Settings;
using Microsoft.Extensions.Options;

namespace CsvProcessor.Api.Extensions;

public static class IocExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IS3Service, S3Service>();
        services.AddScoped<ISqsService, SqsService>();
        services.AddScoped<ICsvReaderService, CsvReaderService>();
    }

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
                    ForcePathStyle = awsSettings.ForceStylePath
                });
        });
    }

    public static void AddSqsService(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonSQS>(sp =>
        {
            AwsSettings awsConfig = sp
                .GetRequiredService<IOptions<AwsSettings>>()
                .Value;

            SqsSettings sqsConfig = sp
                .GetRequiredService<IOptions<SqsSettings>>()
                .Value;

            return new AmazonSQSClient(
                new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey),
                new AmazonSQSConfig
                {
                    ServiceURL = sqsConfig.QueueUrl,
                    AuthenticationRegion = awsConfig.AuthenticationRegion,
                });
        });
    }
}