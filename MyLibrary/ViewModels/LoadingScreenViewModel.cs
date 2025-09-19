using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyLibrary.Resources.Languages;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MyLibrary.ViewModels
{
    public partial class LoadingScreenViewModel : ObservableObject
    {
        private readonly IApiTestService _apiTestService;
        private readonly INavigationService _navigationService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        private string statusMessage = Strings.Splash_Loading;

        public LoadingScreenViewModel(IApiTestService apiTestService, INavigationService navigationService, INotificationService notificationService)
        {
            _apiTestService = apiTestService;
            _navigationService = navigationService;
            _notificationService = notificationService;
        }

        public async Task PerformHealthChecksAsync()
        {
            if(await CheckApi() && await CheckDB())
            {
                StatusMessage = Strings.Splash_LoadingComplete;
                ShowLogIn();
            }
            else
            {
                _notificationService.ShowError(message: Strings.Splash_APIChecksFailed, title: Strings.Error);
            }
            
        }
        private async Task<bool> CheckApi()
        {
            try
            {
                StatusMessage = Strings.Splash_CheckingAPI;
                return await _apiTestService.IsApiOnline();
            }
            catch (Exception err)
            {
                _notificationService.ShowError(message: err.Message, title: Strings.Error);
                return false;
            }
        }
        private async Task<bool> CheckDB()
        {
            try
            {
                StatusMessage = Strings.Splash_CheckingDB;
                await Task.Delay(3000);
                return await _apiTestService.IsDbOnline();
            }
            catch (Exception err)
            {
                _notificationService.ShowError(message: err.Message, title: Strings.Error);
                return false;
            }
        }
        private void ShowLogIn()
        {
            _navigationService.NavigateTo<LogIn>();
        }
    }
}
