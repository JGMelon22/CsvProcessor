using CsvProcessor.Shared.Models;

namespace CsvProcessor.Lambda.Services.Interfaces;

public interface ICsvReaderService
{
    IEnumerable<Product> ReadRecords(Stream stream);
}