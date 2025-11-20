namespace Carpooling.WebApi.Moduls
{
    // Add this class definition above or below your AuthController class
    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime TokenExpiryTime { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
