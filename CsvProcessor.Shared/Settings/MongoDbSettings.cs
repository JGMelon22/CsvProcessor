namespace CsvProcessor.Shared.Settings;

public class MongoDbSettings
{
    public const string SectionName = "MongoDB";

    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? CollectionName { get; set; }
}