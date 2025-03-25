using MyLibrary.Server.Data.DTOs;

namespace MyLibrary.Server.Http.Responses
{
    public class AuthResult : ITaskResult
    {
        public string? Message { get; }

        public bool Succeeded { get; }

        public int StatusCode { get; }

        public IUserDTO? User { get; }
        public string? Token { get; set; }

        public AuthResult(bool succeeded, int statusCode, string? message = null, string? token = null, IUserDTO? user = null)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = message;
            Token = token;
            User = user;
        }
    }
}
