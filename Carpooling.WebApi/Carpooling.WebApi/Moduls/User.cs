using System.ComponentModel.DataAnnotations;

namespace Carpooling.WebApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Carpooling.WebApi.Enum;
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required, MinLength(2)]
    public string Name { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^[+0-9 ()-]{7,20}$", ErrorMessage = "Phone format is invalid")]

    public string Phone { get; set; } = string.Empty;
    
    [BsonElement("passwordHash")]
    public string password { get; set; } = string.Empty;

    [BsonElement("refreshToken")]
    public string? RefreshToken { get; set; }

    [BsonElement("refreshTokenExpiryTime")]
    public DateTime? RefreshTokenExpiryTime { get; set; }

    [BsonElement("role")]
    public Roles Role { get; set; } = Roles.User;
}
