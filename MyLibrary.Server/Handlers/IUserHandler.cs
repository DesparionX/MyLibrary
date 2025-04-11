using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.DTOs.Interfaces;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IUserHandler
    {
        Task<ITaskResult> GetUserAsync(string userId);
        Task<ITaskResult> GetAllUsers();
        Task<ITaskResult> RegisterUserAsync(INewUser<UserDTO> newUserDTO);
        Task<ITaskResult> UpdateUserAsync(IUserDTO userDTO);
        Task<ITaskResult> DeleteUserAsync(string userId);
    }
}
