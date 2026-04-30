using CsvProcessor.Api.Services.Interfaces;
using CsvProcessor.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CsvProcessor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CsvController(
    IS3Service s3Service,
    ISqsService sqsService,
    ICsvReaderService csvReaderService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("No file was sent.");

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Only CSV files are allowed.");

        using MemoryStream memoryStream = new();
        await file.CopyToAsync(memoryStream, cancellationToken);

        memoryStream.Position = 0;
        var totalRecords = csvReaderService.CountRecords(memoryStream);

        memoryStream.Position = 0;
        var s3Key = await s3Service.UploadAsync(
            memoryStream,
            file.FileName,
            file.ContentType,
            cancellationToken);

        CsvUploadPayload payload = new(
            S3Key: s3Key,
            FileName: file.FileName,
            UploadedAt: DateTime.UtcNow,
            TotalRecords: totalRecords
        );

        await sqsService.PublishAsync(payload, cancellationToken);

        return Ok(payload);
    }
}