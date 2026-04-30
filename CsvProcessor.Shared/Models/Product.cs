using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CsvProcessor.Shared.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? MongoId { get; set; }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}