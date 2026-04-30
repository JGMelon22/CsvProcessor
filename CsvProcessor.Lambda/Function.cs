using System.Text.Json;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using CsvProcessor.Lambda.Services.Interfaces;
using CsvProcessor.Shared.DTOs;
using CsvProcessor.Shared.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CsvProcessor.Lambda;

public class Function
{
    private readonly IS3DownloadService _s3DownloadService;
    private readonly ICsvReaderService _csvReaderService;
    private readonly IMongoDbService _mongoDbService;

    public Function()
    {
    }

    public Function(IS3DownloadService s3DownloadService, ICsvReaderService csvReaderService,
        IMongoDbService mongoDbService)
    {
        _s3DownloadService = s3DownloadService;
        _csvReaderService = csvReaderService;
        _mongoDbService = mongoDbService;
    }

    [LambdaFunction]
    public async Task FunctionHandler(
        SQSEvent sqsEvent,
        ILambdaContext context)
    {
        foreach (var message in sqsEvent.Records)
        {
            context.Logger.LogInformation("Processing message {MessageId}", message.MessageId);

            var payload = JsonSerializer.Deserialize<CsvUploadPayload>(message.Body);

            if (payload is null)
            {
                context.Logger.LogError("Invalid payload at message {MessageId}", message.MessageId);
                continue;
            }

            // Download CSC from RustFS
            Stream stream = await _s3DownloadService.DownloadAsync(payload.S3Key);

            // Read and process registers using CsvHelper
            IEnumerable<Product> products = _csvReaderService.ReadRecords(stream);

            // Saves at MongoDb
            await _mongoDbService.InsertManyAsync(products);

            context.Logger.LogInformation("Message {MessageId} processed. {Total} of registers saved.",
                message.MessageId,
                payload.TotalRecords);
        }
    }
}