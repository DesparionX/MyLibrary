namespace MyLibrary.Server.Configs
{
    public static class JwtConfig
    {
        public static readonly string JwtKey = Environment.GetEnvironmentVariable("JWT_KEY")!;
        public static readonly string JwtIssuer = Environment.GetEnvironmentVariable("WEB_API_URL")!;
        public static readonly string JwtAudience = Environment.GetEnvironmentVariable("CLIENT_URL")!;
    }
}
