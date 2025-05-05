using MyLibrary.Server.Data.DTOs;

namespace MyLibrary.Server.Http.Requests
{
    public interface ILoginRequest
    {
        string? Email { get; }
        string? Password { get; }
        bool? RememberMe { get; }
    }
}
