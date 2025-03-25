namespace MyLibrary.Server.Data.DTOs
{
    public interface INewUser
    {
        public IUserDTO UserDTO { get; set; }
        public string Password { get; set; }
    }
}
