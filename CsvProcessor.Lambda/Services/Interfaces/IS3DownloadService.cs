namespace CsvProcessor.Lambda.Services.Interfaces;

public interface IS3DownloadService
{
    Task<Stream> DownloadAsync(string s3Key, CancellationToken cancellationToken = default);
}