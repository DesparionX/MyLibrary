namespace MyLibrary.Server.Data.DTOs
{
    public class NewUser : INewUser
    {
        public IUserDTO UserDTO { get; set; }
        public string Password { get; set; }
    }
}
