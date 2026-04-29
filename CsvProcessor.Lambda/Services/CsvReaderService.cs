using System.Globalization;
using CsvHelper;
using CsvProcessor.Lambda.Services.Interfaces;
using CsvProcessor.Shared.Mappings;
using CsvProcessor.Shared.Models;

namespace CsvProcessor.Lambda.Services;

public class CsvReaderService : ICsvReaderService
{
    public IEnumerable<Product> ReadRecords(Stream stream)
    {
        using StreamReader reader = new(stream);
        using CsvReader csv = new(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<ProductMap>();

        return csv.GetRecords<Product>().ToList();
    }
}