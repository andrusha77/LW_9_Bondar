using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Carpooling.WebApi.Models;

public class Vehicle
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [Required]
    public int OwnerUserId { get; set; }
    [Required]

    public string Make { get; set; } = string.Empty;
    [Required]
    public string Model { get; set; } = string.Empty;
    [Required]
    public string PlateNumber { get; set; } = string.Empty;
}
