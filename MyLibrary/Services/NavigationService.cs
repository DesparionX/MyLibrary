using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyLibrary.Services.Api
{
    public class NavigationService : INavigationService
    {
        private Window? _currentWindow;
        private Window? _lastWindow;
        private readonly IServiceProvider _serviceProvider;
        private readonly INotificationService _notificationService;

        public NavigationService(IServiceProvider serviceProvider, INotificationService notificationService)
        {
            _serviceProvider = serviceProvider;
            _notificationService = notificationService;
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
