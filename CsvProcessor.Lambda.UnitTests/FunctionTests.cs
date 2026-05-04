using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using CsvProcessor.Lambda;
using CsvProcessor.Lambda.Services.Interfaces;
using CsvProcessor.Shared.DTOs;
using CsvProcessor.Shared.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CsvProcessos.Lambda.UnitTests;

[TestFixture]
public class FunctionTests
{
    private IS3DownloadService _s3DownloadService;
    private ICsvReaderService _csvReaderService;
    private IMongoDbService _mongoDbService;
    private ILambdaContext _lambdaContext;
    private Function _function;

    [SetUp]
    public void Setup()
    {
        _s3DownloadService = Substitute.For<IS3DownloadService>();
        _csvReaderService = Substitute.For<ICsvReaderService>();
        _mongoDbService = Substitute.For<IMongoDbService>();
        _lambdaContext = Substitute.For<ILambdaContext>();

        _function = new Function(_s3DownloadService, _csvReaderService, _mongoDbService);
    }

    [Test]
    public async Task Should_InsertRecords_When_ValidSqsMessageIsReceived()
    {
        // Arrange
        CsvUploadPayload payload = new("products/2026/04/30/b5321325-e1be-4408-9d98-0c28426d79b3-sample.csv",
            "sample.csv", new DateTime(2026, 04, 30), 2);
        MemoryStream stream = new();
        IEnumerable<Product> products = new List<Product> { new(), new() };

        _s3DownloadService.DownloadAsync(payload.S3Key, Arg.Any<CancellationToken>()).Returns(stream);
        _csvReaderService.ReadRecords(stream).Returns(products);

        SQSEvent sqsEvent = new()
        {
            Records = [new SQSEvent.SQSMessage { MessageId = "1", Body = JsonSerializer.Serialize(payload) }]
        };

        // Act
        await _function.FunctionHandler(sqsEvent, _lambdaContext);

        // Assert
        await _mongoDbService.Received(1).InsertManyAsync(products, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Should_ThrowException_When_S3DownloadFails()
    {
        // Arrange
        CsvUploadPayload payload = new("products/2026/04/30/b5321325-e1be-4408-9d98-0c28426d79b3-sample.csv",
            "sample.csv", new DateTime(2026, 04, 30), 2);

        _s3DownloadService.DownloadAsync(Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("RustFS unavailable"));

        SQSEvent sqsEvent = new()
        {
            Records = [new SQSEvent.SQSMessage { MessageId = "1", Body = JsonSerializer.Serialize(payload) }]
        };

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _function.FunctionHandler(sqsEvent, _lambdaContext));
    }
}