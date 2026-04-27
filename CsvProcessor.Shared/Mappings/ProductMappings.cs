using CsvHelper.Configuration;
using CsvProcessor.Shared.Models;

namespace CsvProcessor.Shared.Mappings;

public sealed class ProductMappings : ClassMap<Product>
{
    public ProductMappings()
    {
        Map(p => p.Id).Name("Id");
        Map(p => p.Name).Name("Name");
        Map(p=>p.Price).Name("Price");
        Map(p=>p.Amount).Name("Amount");
        Map(p=>p.Category).Name("Category");
        Map(p=>p.CreatedAt).Name("CreatedAt");
    }
}