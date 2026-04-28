namespace CsvProcessor.Shared.Settings;

public class S3Settings
{
    public const string SectionName = "S3";

    public string? BucketName { get; set; }
}