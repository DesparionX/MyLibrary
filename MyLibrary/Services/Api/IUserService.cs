using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.Api
{
    public interface IUserService
    {
        Task<ITaskResult> LogInAsync(ILoginRequest request);
        Task<ITaskResult> RegisterUserAsync(INewUser newUser);
        Task<ITaskResult> GetUserIdentityAsync();
    }
}
