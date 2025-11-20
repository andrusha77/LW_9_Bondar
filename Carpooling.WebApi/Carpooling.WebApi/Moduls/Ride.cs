using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Carpooling.WebApi.Models;
using System.ComponentModel.DataAnnotations;

public enum RideStatus { Planned, Active, Completed, Canceled, Booked }

public class Ride
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [Required]
    public int DriverUserId { get; set; }
    [Required]
    public string From { get; set; } = string.Empty;
    [Required]
    public string To { get; set; } = string.Empty;
    [Required]
    public DateTime DepartureTime { get; set; }
    [Required]
    public int SeatsTotal { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public RideStatus Status { get; set; } = RideStatus.Planned;
    [Required]
    public string statustirg { get => Status.ToString();
    
    
    
    }
}
