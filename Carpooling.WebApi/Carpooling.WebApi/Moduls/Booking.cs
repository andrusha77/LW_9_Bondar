using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Carpooling.WebApi.Models;

public class Booking
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public int RideId { get; set; }
    public int PassengerUserId { get; set; }
    public int Seats { get; set; }
    public DateTime CreatedAt { get; set; }
}
