using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Resources.Languages;
using MyLibrary.Views.Pages;
using MyLibrary.Views.Pages.Borrow;
using MyLibrary.Views.Pages.Return;
using MyLibrary.Views.Pages.Sell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyLibrary.Services.Api
{
    public partial class NavigationService : ObservableObject, INavigationService
    {
        private Window? _currentWindow;
        private Window? _lastWindow;

        [ObservableProperty]
        private object _currentView = new();

        private readonly IServiceProvider _serviceProvider;
        private readonly INotificationService _notificationService;

        public NavigationService(IServiceProvider serviceProvider, INotificationService notificationService)
        {
            _serviceProvider = serviceProvider;
            _notificationService = notificationService;
        }
        public void NavigateToView(string viewName)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var serviceProvider = scope.ServiceProvider;
                CurrentView = viewName.ToLower() switch
                {
                    "home" => serviceProvider.GetRequiredService<HomeView>(),
                    "borrow" => serviceProvider.GetRequiredService<BorrowView>(),
                    "sell" => serviceProvider.GetRequiredService<SellView>(),
                    "return" => serviceProvider.GetRequiredService<ReturnView>(),
                    _ => serviceProvider.GetRequiredService<NotFoundView>()
                };
            }
            catch(Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: Strings.Errors_NavigatingToView + err.Message);
            }
        }
        public void BackToHomeView()
        {
            try
            {
                CurrentView = _serviceProvider.GetRequiredService<HomeView>();
            }
            catch (Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: Strings.Errors_NavigatingToView + err.Message);
            }
        }
        public void NavigateTo<TWindow>() where TWindow : Window
        {
            try
            {
                _lastWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

                var window = _serviceProvider.GetRequiredService<TWindow>();
                _currentWindow = window;

                window.Show();
                _lastWindow?.Close();
            }
            catch (Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: Strings.Errors_NavigatingToWindow + err.Message);
            }
        }
        public void CloseCurrentWindow()
        {
            _currentWindow?.Close();
        }
        public void CloseApp()
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: Strings.Errors_ClosingApp + err.Message);
            }
        }
    }
}
