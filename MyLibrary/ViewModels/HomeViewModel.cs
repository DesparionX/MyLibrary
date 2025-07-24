using CommunityToolkit.Mvvm.Input;
using MyLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyLibrary.ViewModels
{
    public class HomeViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand NavigateToViewCommand => new RelayCommand<string>(_navigationService.NavigateToView);
        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

    }
}
