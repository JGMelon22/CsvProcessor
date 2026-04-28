namespace CsvProcessor.Shared.Settings;

public class AwsSettings
{
    public const string SectionName = "AWS";

    public string? ServiceUrl { get; set; }
    public string? AuthenticationRegion { get; set; }
    public string? AccessKey { get; set; }
    public string? SecretKey { get; set; }
}