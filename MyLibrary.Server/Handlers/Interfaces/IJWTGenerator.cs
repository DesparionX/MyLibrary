using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Handlers.Interfaces
{
    public interface IJWTGenerator
    {
        Task<string> GenerateJWT(IUser<string> user);
    }
}
