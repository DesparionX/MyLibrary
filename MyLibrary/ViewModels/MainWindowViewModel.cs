using CommunityToolkit.Mvvm.ComponentModel;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private UserDTO _user = new();

        public MainWindowViewModel(IAuthService authService)
        {
            _authService = authService;
            if (_authService.IsAuthenticated())
            {
                _user = _authService.GetUser() as UserDTO;
            }
        }
    }
}
