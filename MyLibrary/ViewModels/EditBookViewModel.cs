using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyLibrary.Resources.Languages;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Shared.Interfaces.IDTOs;
using System.Windows.Media;

namespace MyLibrary.ViewModels
{
    public partial class EditBookViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly INavigationService _navigationService;
        private readonly IBookService _bookService;
        private readonly IFileService _fileService;

        [ObservableProperty]
        private IBookDTO? _book;

        [ObservableProperty]
        private bool _editorMode = false;

        public EditBookViewModel(
            INavigationService navigationService,
            INotificationService notificationService,
            IBookService bookService,
            IFileService fileService)
        {
            _navigationService = navigationService;
            _notificationService = notificationService;
            _bookService = bookService;
            _fileService = fileService;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return true;
        }

        public void FetchBookPicture()
        {
            Book!.Picture = _fileService.GetImageUrl(Book.Picture);
        }
    }
}
