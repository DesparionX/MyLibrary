using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs.Interfaces
{
    public interface INewUser<TUserDTO>
    {
        public TUserDTO UserDTO { get; set; }
        public string Password { get; set; }
    }
    public interface INewUser
    {
        public UserDTO UserDTO { get; set; }
        public string Password { get; set; }
    }
}
