namespace CsvProcessor.Api.Services.Interfaces;

public interface IS3Service
{
    Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default);
}