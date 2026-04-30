using Amazon.Lambda.Annotations;
using CsvProcessor.Lambda.Extensions;
using CsvProcessor.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CsvProcessor.Lambda;

[LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            // .AddEnvironmentVariables() // Would use only if configured extra variables at AWS Lambda UI
            .Build();

        services.Configure<AwsSettings>(
            configuration.GetSection(AwsSettings.SectionName));
        services.Configure<MongoDbSettings>(
            configuration.GetSection(MongoDbSettings.SectionName));
        services.Configure<S3Settings>(
            configuration.GetSection(S3Settings.SectionName));

        services.AddAwsServices();
        services.AddMongoDb();
        services.AddLambdaServices();
    }
}