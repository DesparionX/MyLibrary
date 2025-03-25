namespace MyLibrary.Server.Configs
{
    public static class DbConfig
    {
        public static readonly string ConnectionString = $"Server={Environment.GetEnvironmentVariable("DB_SERVER")};" +
                                                         $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                                                         $"User Id={Environment.GetEnvironmentVariable("DB_USER")};" +
                                                         $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                                                         $"Trusted_Connection=True;" +
                                                         $"TrustServerCertificate=True;";
    }
}
