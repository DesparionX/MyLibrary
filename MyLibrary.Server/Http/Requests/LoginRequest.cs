namespace MyLibrary.Server.Http.Requests
{
    public class LoginRequest : ILoginRequest
    {
        public string? Email { get; }
        public string? Password { get; }
        public bool? RememberMe { get; }

        public LoginRequest(string email, string password, bool rememberMe)
        {
            Email = email;
            Password = password;
            RememberMe = rememberMe;
        }
    }
}
