using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Helpers;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Shared.Interfaces.IDTOs;
using MyLibrary.Views.Pages.Books.EditBook;
using Namotion.Reflection;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using static MyLibrary.Helpers.BookFilterTypes;

namespace MyLibrary.ViewModels
{
    public partial class BookViewModel : ObservableObject
    {
        private readonly IBookService _bookService;
        private readonly INotificationService _notificationService;
        private readonly INavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string _filterText = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilterButtonText))]
        private FilterType _filter = FilterType.Title;

        public string FilterButtonText => Filter.ToString();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilteredBooks))]
        private ObservableCollection<IBookDTO> _books = [];

        [ObservableProperty]
        private ObservableCollection<IBookDTO> _filteredBooks = [];

        public BookViewModel(
            IBookService bookService,
            INotificationService notificationService,
            INavigationService navigationService,
            IServiceProvider serviceProvider)
        {
            _bookService = bookService;
            _notificationService = notificationService;
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;

            Books.CollectionChanged += (s, e) => ApplyFilter();
        }

        public async Task LoadBooksAsync()
        {
            var result = await _bookService.GetAllBooksAsync() as BookTaskResult;
            if (result is null || !result.Succeeded)
            {
                _notificationService.ShowError(title: Strings.CustomDialog_Title_Fail, message: result?.Message ?? Strings.Errors_Api_NullResponse);
                return;
            }
            
            if (result.Books is null || result.Books.Count == 0)
            {
                _notificationService.ShowInfo(title: Strings.CustomDialog_Title_Fail, message: "No books found !");
                return;
            }

            Books = new ObservableCollection<IBookDTO>(result.Books.DistinctBy(b => b.ISBN).OrderBy(b => b.Title));
            FilteredBooks = new ObservableCollection<IBookDTO>(Books);
        }

        // Filtering 
        [RelayCommand]
        public void ChangeFilter()
        {
            Filter = (FilterType)(((int)Filter + 1) % Enum.GetValues<FilterType>().Length);
        }

        partial void OnFilterTextChanged(string value)
        {
            ApplyFilter();
        }

        // Open book details window.
        [RelayCommand]
        public async Task OpenBookDetails(IBookDTO? selectedBook)
        {
            if (selectedBook is null) return;

            var scope = _serviceProvider.CreateScope();
            var vm = scope.ServiceProvider.GetRequiredService<EditBookViewModel>();
            vm.Book = selectedBook;

            var dialog = new EditBookView(vm);
            if (dialog.ShowDialog() == true)
            {
                await LoadBooksAsync();
            }
        }

        private void ApplyFilter()
        {
            FilteredBooks = Filter switch
            {
                FilterType.Title => new ObservableCollection<IBookDTO>(
                    Books.Where(b => b.Title.Contains(FilterText, StringComparison.OrdinalIgnoreCase))),

                FilterType.Author => new ObservableCollection<IBookDTO>(
                    Books.Where(b => b.Author.Contains(FilterText, StringComparison.OrdinalIgnoreCase))),

                FilterType.Genre => new ObservableCollection<IBookDTO>(
                    Books.Where(b => b.Genre.Contains(FilterText, StringComparison.OrdinalIgnoreCase))),

                FilterType.ISBN => new ObservableCollection<IBookDTO>(
                    Books.Where(b => b.ISBN.Contains(FilterText, StringComparison.OrdinalIgnoreCase))),

                _ => Books
            };
        }
    }
}
