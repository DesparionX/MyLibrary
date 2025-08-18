using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services
{
    public interface IAuthService
    {
        bool IsAuthenticated();
        Task<bool> AuthenticateAsync(IUserDTO user, string jwtToken);
        Task<bool> ClearSesionAsync();
        string GetToken();
        IUserDTO? GetUser();
    }
}
