using CommunityToolkit.Mvvm.ComponentModel;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services
{
    public partial class AuthService : ObservableObject, IAuthService
    {
        [ObservableProperty]
        private string _token = string.Empty;

        [ObservableProperty]
        private UserDTO? _user;

        private readonly INotificationService _notificationService;

        public AuthService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<bool> AuthenticateAsync(IUserDTO user, string jwtToken)
        {
            try
            {
                await Task.Run(() =>
                {
                    Token = jwtToken;
                    User = (UserDTO)user;
                });
                return true;
            }
            catch (Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: err.Message);
                return false;
            }
        }

        public async Task<bool> ClearSesionAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    Token = string.Empty;
                    User = null;
                    return true;
                });
                _notificationService.ShowError(title: Strings.Error, message: Strings.Errors_AuthService_ClearingUserIdentity);
                return false;
            }
            catch (Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: err.Message);
                return false;
            }
        }

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(Token) && User != null;
        }

        public string GetToken()
        {
            return Token;
        }
        public IUserDTO? GetUser()
        {
            return User;
        }
    }
}
