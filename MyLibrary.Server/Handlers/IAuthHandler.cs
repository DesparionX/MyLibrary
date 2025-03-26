using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;
using System.Security.Claims;
using System.Security.Principal;

namespace MyLibrary.Server.Handlers
{
    public interface IAuthHandler
    {
        Task<ITaskResult> LoginUserAsync(ILoginRequest request);
        Task<ITaskResult> LogOutAsync(IIdentity userIdentity);
        Task<ITaskResult> GetIdentityAsync(IIdentity userIdentity);
    }
}
