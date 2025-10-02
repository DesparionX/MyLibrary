using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using MyLibrary.Configs;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Services;
using System.Windows.Input;

namespace MyLibrary.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly ApiSettings _apiSettings;

        [ObservableProperty]
        private UserDTO? _user;

        [ObservableProperty]
        private object _currentView = new();

        public ICommand NavigateToViewCommand => new RelayCommand<string>(NavigateToView!);
        public ICommand CloseApp => new RelayCommand(_navigationService.CloseApp);


        public MainWindowViewModel(IAuthService authService, INavigationService navigationService, IServiceProvider serviceProvider, IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
            _authService = authService;
            if (_authService.IsAuthenticated())
            {
                _user = (UserDTO)_authService.GetUser();
            }

            _navigationService = navigationService;
            _navigationService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_navigationService.CurrentView))
                {
                    CurrentView = _navigationService.CurrentView;
                }
            };
            NavigateToView("home");
            
        }
        public string GetUserAvatar()
        {
            return _apiSettings.BaseUrl + _apiSettings.Images + User?.Avatar;
        }
        private void NavigateToView(string viewName)
        {
            _navigationService.NavigateToView(viewName);
        }
    }
}
