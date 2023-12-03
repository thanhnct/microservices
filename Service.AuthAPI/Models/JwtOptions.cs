namespace Service.AuthAPI.Models
{
    public class JwtOptions
    {
        public required string Secrect { get; set; } = string.Empty;
        public required string Issuer { get; set; } = string.Empty;
        public required string Audience { get; set; } = string.Empty;
    }
}
