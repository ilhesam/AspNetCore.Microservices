namespace Hotel.Services.Identity.Core.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public double AccessTokenExpirationInMinutes { get; set; }
        public double RefreshTokenExpirationInMinutes { get; set; }
    }
}