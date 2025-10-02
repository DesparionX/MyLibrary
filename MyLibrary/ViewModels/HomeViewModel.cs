using CommunityToolkit.Mvvm.Input;
using MyLibrary.Services;
using System.Windows.Input;

namespace MyLibrary.ViewModels
{
    public class HomeViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand NavigateToViewCommand => new RelayCommand<string>(_navigationService.NavigateToView!);
        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

    }
}
