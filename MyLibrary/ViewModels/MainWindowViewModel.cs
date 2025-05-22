using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Services;
using MyLibrary.Views;
using MyLibrary.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyLibrary.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private UserDTO _user = new();

        [ObservableProperty]
        private object _currentView = new();

        public ICommand NavigateToViewCommand => new RelayCommand<string>(NavigateToView);

        public MainWindowViewModel(IAuthService authService, INavigationService navigationService)
        {
            _authService = authService;
            if (_authService.IsAuthenticated())
            {
                _user = _authService.GetUser() as UserDTO;
            }

            _navigationService = navigationService;
            NavigateToView("home");
        }
        private void NavigateToView(string viewName)
        {
            CurrentView = viewName.ToLower() switch
            {
                "home" => new HomeView(),
                _ => new NotFoundView()
            };
        }
    }
}
