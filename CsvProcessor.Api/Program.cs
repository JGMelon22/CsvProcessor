using Amazon.Runtime;
using Amazon.S3;
using Amazon.SQS;
using CsvProcessor.Api.Extensions;
using CsvProcessor.Shared.Settings;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddServices();

# region [AWS and MongoDb - Options Configurations]

builder.Services.Configure<AwsSettings>(
    builder.Configuration.GetSection(AwsSettings.SectionName));
builder.Services.Configure<SqsSettings>(
    builder.Configuration.GetSection(SqsSettings.SectionName));
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));
builder.Services.Configure<S3Settings>(
    builder.Configuration.GetSection(S3Settings.SectionName));

#endregion

var awsConfig = builder.Configuration.GetSection(AwsSettings.SectionName).Get<AwsSettings>();
var sqsConfig = builder.Configuration.GetSection(SqsSettings.SectionName).Get<SqsSettings>();

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
        options.WithTitle("CSV Processor API");
        options.DisableAgent();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();