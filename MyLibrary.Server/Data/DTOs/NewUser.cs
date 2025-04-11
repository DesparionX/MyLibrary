using MyLibrary.Server.Data.DTOs.Interfaces;

namespace MyLibrary.Server.Data.DTOs
{
    public class NewUser : INewUser<UserDTO>
    {
        public UserDTO UserDTO { get; set; }
        public string Password { get; set; }
    }
}
