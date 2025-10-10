using CommunityToolkit.Mvvm.ComponentModel;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.ViewModels
{
    public partial class EditBookViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly INavigationService _navigationService;
        private readonly IBookService _bookService;

        [ObservableProperty]
        private IBookDTO? _book;

        public EditBookViewModel(
            INavigationService navigationService,
            INotificationService notificationService,
            IBookService bookService)
        {
            _navigationService = navigationService;
            _notificationService = notificationService;
            _bookService = bookService;
        }
    }
}
