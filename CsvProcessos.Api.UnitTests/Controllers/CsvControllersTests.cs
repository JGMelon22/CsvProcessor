using System.Text;
using CsvProcessor.Api.Controllers;
using CsvProcessor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace CsvProcessos.Api.UnitTests.Controllers;

[TestFixture]
public class CsvControllersTests
{
    private IS3Service _s3Service;
    private ISqsService _sqsService;
    private ICsvReaderService _csvReaderService;
    private CsvController _csvController;

    [SetUp]
    public void Setup()
    {
        _s3Service = Substitute.For<IS3Service>();
        _sqsService = Substitute.For<ISqsService>();
        _csvReaderService = Substitute.For<ICsvReaderService>();

        _csvController = new CsvController(_s3Service, _sqsService, _csvReaderService);
    }

    [Test]
    public async Task Should_ReturnOk_When_ValidCsvIsUploaded()
    {
        // Arrange
        var bytes = Encoding.UTF8.GetBytes(
            "Id,Name,Price,Amount,Category,CreatedAt\n1,Teclado Mecânico,350.90,15,Periféricos,2024-01-10");
        IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "sample.csv")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };

        _csvReaderService.CountRecords(Arg.Any<Stream>()).Returns(1);
        _s3Service.UploadAsync(Arg.Any<Stream>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns("b5321325-e1be-4408-9d98-0c28426d79b3-sample.csv");

        // Act
        var result = await _csvController.Upload(file, CancellationToken.None);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task Should_ReturnBadRequest_WhenFileIsNotCsv()
    {
        // Arrange
        byte[] bytes = Encoding.UTF8.GetBytes(
            """
            {
                "ping": "pong"
            }
            """);
        IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "sample.json")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/json"
        };

        // Act
        var result = await _csvController.Upload(file, CancellationToken.None);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
}