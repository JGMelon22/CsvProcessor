using Amazon.S3;
using Amazon.S3.Model;
using CsvProcessor.Api.Services.Interfaces;
using CsvProcessor.Shared.Settings;
using Microsoft.Extensions.Options;

namespace CsvProcessor.Api.Services;

public class S3Service(IAmazonS3 s3Client, IOptions<S3Settings> options) : IS3Service
{
    private readonly S3Settings _s3Client = options.Value;

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType,
        CancellationToken cancellationToken = default)
    {
        string s3Key = $"products/{DateTime.UtcNow:yyyy/MM/dd}/{Guid.NewGuid()}-{fileName}";

        PutObjectRequest request = new()
        {
            BucketName = _s3Client.BucketName,
            Key = s3Key,
            InputStream = stream,
            ContentBody = contentType
        };

        await s3Client.PutObjectAsync(request, cancellationToken);

        return s3Key;
    }
}