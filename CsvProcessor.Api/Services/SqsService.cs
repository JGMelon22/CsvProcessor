using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using CsvProcessor.Api.Services.Interfaces;
using CsvProcessor.Shared.DTOs;
using CsvProcessor.Shared.Settings;
using Microsoft.Extensions.Options;

namespace CsvProcessor.Api.Services;

public class SqsService(IAmazonSQS sqsClient, IOptions<SqsSettings> options) : ISqsService
{
    private readonly SqsSettings _settings = options.Value;

    public async Task PublishAsync(CsvUploadPayload payload, CancellationToken cancellationToken = default)
    {
        SendMessageRequest request = new()
        {
            QueueUrl = _settings.QueueUrl,
            MessageBody = JsonSerializer.Serialize(payload)
        };

        await sqsClient.SendMessageAsync(request, cancellationToken);
    }
}