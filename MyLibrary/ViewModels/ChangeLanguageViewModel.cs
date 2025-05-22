using CommunityToolkit.Mvvm.ComponentModel;
using MyLibrary.Services;
using MyLibrary.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.ViewModels
{
    public partial class ChangeLanguageViewModel : ObservableObject
    {
        private readonly ILanguageService _languageService;
        private readonly INotificationService _notificationService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string _currentLanguage = Thread.CurrentThread.CurrentCulture.NativeName;

        public ChangeLanguageViewModel(ILanguageService languageService, INotificationService notificationService, INavigationService navigationService)
        {
            _languageService = languageService;
            _notificationService = notificationService;
            _navigationService = navigationService;
        }



        public void ChangeLanguage(string newLanguage)
        {
            if (string.Equals(CurrentLanguage, newLanguage, StringComparison.InvariantCultureIgnoreCase))
            {
                _notificationService.ShowWarning(title: "Language Change", message: "The selected language is already set.");
                _navigationService.NavigateTo<LoadingScreen>();
            }
            else
            {
                try
                {
                    _languageService.SetLanguage(newLanguage);
                    _navigationService.NavigateTo<LoadingScreen>();
                }
                catch(Exception err)
                {
                    _notificationService.ShowError(title: "Language Change Error", message: $"Error changing language: {err.Message}");
                }
            }
        }
    }
}
