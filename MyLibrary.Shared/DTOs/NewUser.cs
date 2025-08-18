using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Data.DTOs
{
    public class NewUser : INewUser<UserDTO>
    {
        public required UserDTO UserDTO { get; set; }
        public required string Password { get; set; }
    }
}
