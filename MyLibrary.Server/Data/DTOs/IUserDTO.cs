using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface IUserDTO : IDTO<string, IUser<string>>, IUser<string>
    {
    }
}
