using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Http.Responses
{
    public class UserTaskResult : ITaskResult
    {
        public string? Message { get; }

        public bool Succeeded { get; }

        public int StatusCode { get; }

        public ICollection<IUserDTO>? Users { get; }
        public IUserDTO? User { get; }

        public UserTaskResult(bool succeeded, int statusCode, string? message, ICollection<IUserDTO>? users = null, IUserDTO? user = null)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = message;
            Users = users;
            User = user;
        }
    }
}
