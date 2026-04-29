using Amazon.S3;
using Amazon.S3.Model;
using CsvProcessor.Lambda.Services.Interfaces;
using CsvProcessor.Shared.Settings;
using Microsoft.Extensions.Options;

namespace CsvProcessor.Lambda.Services;

public class S3DownloadService(IAmazonS3 s3Client, IOptions<S3Settings> options) : IS3DownloadService
{
    private readonly S3Settings _s3Settings = options.Value;

    public async Task<Stream> DownloadAsync(string s3Key, CancellationToken cancellationToken = default)
    {
        GetObjectRequest request = new()
        {
            BucketName = _s3Settings.BucketName,
            Key = s3Key
        };

        GetObjectResponse response = await s3Client.GetObjectAsync(request, cancellationToken);

        return response.ResponseStream;
    }
}