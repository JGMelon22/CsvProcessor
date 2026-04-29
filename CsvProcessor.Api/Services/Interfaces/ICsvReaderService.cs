namespace CsvProcessor.Api.Services.Interfaces;

public interface ICsvReaderService
{
    int CountRecords(Stream stream);
}