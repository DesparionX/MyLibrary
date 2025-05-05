using CommunityToolkit.Mvvm.ComponentModel;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyLibrary.ViewModels
{
    public partial class LogInViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly INavigationService _navigationService;
        private readonly INotificationService _notificationService;
        private readonly IValidationService _validationService;
        private readonly IAuthService _authService;

        // Email field.
        [ObservableProperty]
        private string _email = "Email";
        [ObservableProperty]
        private string _emailErrors = string.Empty;
        [ObservableProperty]
        private Visibility _emailErrorVisibility = Visibility.Collapsed;

        // Password field.
        [ObservableProperty]
        private string _password = string.Empty;
        [ObservableProperty]
        private string _passwordErrors = string.Empty;
        [ObservableProperty]
        private Visibility _passwordErrorVisibility = Visibility.Collapsed;

        // Remember me (OPTIONAL)
        [ObservableProperty]
        private bool _rememberMe = false;

        public LogInViewModel(IUserService userService,
            INavigationService navigationService,
            INotificationService notificationService,
            IValidationService validationService,
            IAuthService authService)
        {
            _userService = userService;
            _navigationService = navigationService;
            _notificationService = notificationService;
            _validationService = validationService;
            _authService = authService;
        }

        public async Task LogInAsync()
        {

            try
            {
                if(_validationService.ValidateEmail(Email).IsValid 
                    && _validationService.ValidatePassword(Password).IsValid)
                {
                    var result = await _userService.LogInAsync(new LoginRequest(email: Email, password: Password, rememberMe: RememberMe)) as AuthResult;
                    if(result == null)
                    {
                        _notificationService.ShowError(title: Strings.Errors_UserService_LoginFailed, message: Strings.Errors_UserService_InvalidResponse);
                        return;
                    }
                    if (!result.Succeeded)
                    {
                        _notificationService.ShowError(title: Strings.Errors_UserService_LoginFailed, message: result.Message!);
                        return;
                    }
                    await _authService.AuthenticateAsync(result.User!, result.Token!);
                    _navigationService.NavigateTo<MainWindow>();
                }
            }
            catch (Exception err)
            {
                _notificationService.ShowError(title: Strings.Errors_UserService_LoginFailed, message: err.Message);
            }
        }
        public void CheckEmail(string email)
        {
            EmailErrors = string.Empty;
            var result = _validationService.ValidateEmail(email);
            if (result.IsValid)
            {
                EmailErrors = string.Empty;
                EmailErrorVisibility = Visibility.Collapsed;
            }
            else
            {
                result.Errors?.ToList().ForEach(error =>
                {
                    EmailErrors += error + Environment.NewLine;
                    
                });
                EmailErrorVisibility = Visibility.Visible;
            }
        }
        public void CheckPassword(string password)
        {
            PasswordErrors = string.Empty;
            var result = _validationService.ValidatePassword(password);
            if (result.IsValid)
            {
                PasswordErrors = string.Empty;
                PasswordErrorVisibility = Visibility.Collapsed;
            }
            else
            {
                result.Errors?.ToList().ForEach(error =>
                {
                    PasswordErrors += error + Environment.NewLine;
                });
                PasswordErrorVisibility = Visibility.Visible;
            }
        }
    }
}
