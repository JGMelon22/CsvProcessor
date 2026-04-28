using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using CsvProcessor.Shared.Settings;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

# region [AWS and MongoDb - Typed Configurations]

var awsConfig = builder.Configuration.GetSection(AwsSettings.SectionName).Get<AwsSettings>()!;
var sqsConfig = builder.Configuration.GetSection(SqsSettings.SectionName).Get<SqsSettings>()!;
var mongoDbConfig = builder.Configuration.GetSection(MongoDbSettings.SectionName).Get<MongoDbSettings>()!;
var s3Config = builder.Configuration.GetSection(S3Settings.SectionName).Get<S3Settings>()!;

builder.Services.AddSingleton(awsConfig);
builder.Services.AddSingleton(mongoDbConfig);
builder.Services.AddSingleton(s3Config);

# endregion

// AWS S3 pointing to RustFS
builder.Services.AddSingleton<IAmazonS3>(_ =>
    new AmazonS3Client(
        new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey),
        new AmazonS3Config
        {
            ServiceURL = awsConfig.ServiceUrl,
            AuthenticationRegion = awsConfig.AuthenticationRegion,
            ForcePathStyle = true
        }
    ));

// AWS SQS pointing to ElasticMQ
builder.Services.AddSingleton<IAmazonSQS>(_ =>
    new AmazonSQSClient(
        new BasicAWSCredentials(awsConfig.AccessKey, awsConfig.SecretKey),
        new AmazonSQSConfig
        {
            ServiceURL = sqsConfig.QueueUrl,
            AuthenticationRegion = awsConfig.AuthenticationRegion,
        }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("CSV Processor API")
            .DisableAgent();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();