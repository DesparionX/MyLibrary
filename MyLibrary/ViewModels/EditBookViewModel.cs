using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Shared.Interfaces.IDTOs;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyLibrary.ViewModels
{
    public partial class EditBookViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly IBookService _bookService;
        private readonly IFileService _fileService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(AvailableCopies))]
        private IBookDTO? _book;

        [ObservableProperty]
        private bool _editorMode = false;

        [ObservableProperty]
        private int _availableCopies = 0;

        public EditBookViewModel(
            INotificationService notificationService,
            IBookService bookService,
            IFileService fileService)
        {
            _notificationService = notificationService;
            _bookService = bookService;
            _fileService = fileService;
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (!EditorMode) return false;
            if (Book is null) return false;

            var result = await _bookService.UpdateBookAsync(Book);

            if (!result.Succeeded)
            {
                _notificationService.ShowError(title: Strings.CustomDialog_Title_Fail, message: result.Message!);
                return false;
            }

            _notificationService.ShowSuccess(title: Strings.CustomDialog_Title_Success, message: Strings.BookService_SuccessfullyUpdated);
            return true;
        }

        partial void OnBookChanged(IBookDTO? value)
        {
            Book!.Picture = _fileService.GetImageUrl(value!.Picture);
            _ = UpdateBookAvailability(value);
        }

        private async Task UpdateBookAvailability(IBookDTO? book)
        {
            AvailableCopies = await GetAvailableCopiesAsync(book!.ISBN);
        }

        private async Task<int> GetAvailableCopiesAsync(string isbn)
        {
            var result = await _bookService.GetAllBooksAsync() as BookTaskResult;

            if (result is not null && result.Books is not null)
                return result.Books.Count(b => b.ISBN == isbn && b.IsAvailable);

            return 0;
        }
    }
}
