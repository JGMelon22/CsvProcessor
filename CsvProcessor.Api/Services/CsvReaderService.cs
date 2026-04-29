using System.Globalization;
using CsvHelper;
using CsvProcessor.Api.Services.Interfaces;
using CsvProcessor.Shared.Mappings;
using CsvProcessor.Shared.Models;

namespace CsvProcessor.Api.Services;

public class CsvReaderService : ICsvReaderService
{
    public int CountRecords(Stream stream)
    {
       using StreamReader reader = new(stream, leaveOpen: true);
       using CsvReader csv = new(reader, CultureInfo.InvariantCulture);

       csv.Context.RegisterClassMap<ProductMap>();
       
       return csv.GetRecords<Product>().Count();
    }
}