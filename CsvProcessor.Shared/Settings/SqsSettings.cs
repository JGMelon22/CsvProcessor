namespace CsvProcessor.Shared.Settings;

public class SqsSettings
{
    public const string SectionName = "SQS";

    public string? QueueUrl { get; set; }
}