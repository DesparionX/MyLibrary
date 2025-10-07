using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Helpers;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Shared.DTOs;
using MyLibrary.Shared.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyLibrary.ViewModels
{
    public partial class ReturnViewModel : ObservableObject
    {
        private readonly IBookService _bookService;
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IOperationService _operationService;
        private readonly IBorrowService _borrowService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        private string _bookId = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ErrorsVisibility))]
        private string _errors = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BookDetailsVisibility))]
        private IBookDTO? _borrowedBook = null;

        [ObservableProperty]
        private IOperationDTO _operation = new OperationDTO();

        [ObservableProperty]
        private string _borrowerId = string.Empty;

        public Visibility BookDetailsVisibility => BorrowedBook == null ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ErrorsVisibility => string.IsNullOrWhiteSpace(Errors) ? Visibility.Collapsed : Visibility.Visible;

        public ReturnViewModel(IBookService bookService, IBorrowService borrowService, INavigationService navigationService, IAuthService authService, IOperationService operationService, INotificationService notificationService)
        {
            _bookService = bookService;
            _borrowService = borrowService;
            _authService = authService;
            _navigationService = navigationService;
            _operationService = operationService;
            _notificationService = notificationService;
            BorrowedBook = null;
        }

        [RelayCommand]
        public async Task FindBookAsync(string bookId)
        {
            if(!IsValidBookId(bookId))
            {
                Errors += Strings.Validators_BookId_Invalid + Environment.NewLine;
                BorrowedBook = null;
                return;
            }

            var result = await _bookService.FindBookByIdAsync(bookId) as BookTaskResult;
            if (result is null)
            {
                Errors += Strings.Errors_Api_NullResponse + Environment.NewLine;
                return;
            }

            if (!result.Succeeded)
            {
                Errors += Strings.BookService_Errors_BookDoesntExist + Environment.NewLine;
                return;
            }

            BorrowerId = await GetBorrowerIdAsync(bookId);

            if (string.IsNullOrWhiteSpace(BorrowerId))
                return;

            BorrowedBook = result.Book;
        }

        [RelayCommand]
        public async Task ReturnBookAsync()
        {
            if (!string.IsNullOrWhiteSpace(Errors))
                return;

            if (BorrowedBook is null)
                return;

            FillOperation();

            var result = await _operationService.PerformOperation(Operation) as OperationTaskResult;
            if (result is null)
            {
                Errors += Strings.Errors_Api_NullResponse + Environment.NewLine;
                return;
            }

            if (!result.Succeeded)
            {
                Errors += result.Message + Environment.NewLine;
                return;
            }

            _notificationService.ShowSuccess(title: Strings.CustomDialog_Title_Success, message: Strings.Return_Book_Returned_Successfully);
            _navigationService.BackToHomeView();
        }

        // BookID field validation
        // Triggers validation when the BorrowedId property changes.
        partial void OnBookIdChanged(string value)
        {
            ValidateBookId(value);
        }

        private void ValidateBookId(string bookId)
        {
            if (IsValidBookId(bookId))
            {
                Errors = string.Empty;
            }
            else
            {
                Errors += Strings.Validators_BookId_Invalid + Environment.NewLine;
                BorrowedBook = null;
            }
        }

        private bool IsValidBookId(string bookId)
        {
            Errors = string.Empty;

            return Guid.TryParse(bookId, out var id) && id != Guid.Empty;
        }

        // Retrieve borrower Id.
        private async Task<string> GetBorrowerIdAsync(string bookId)
        {
            var result = await _borrowService.GetBooksBorrowsAsync(bookId, isReturned: false) as BorrowTaskResult;
            if (result is null)
            {
                Errors += Strings.Errors_Api_NullResponse + Environment.NewLine;
                return string.Empty;
            }

            if (!result.Succeeded)
            {
                Errors += Strings.Errors_Book_Already_Returned + Environment.NewLine;
                return string.Empty;
            }

            var borrow = result.BorrowDtos?.FirstOrDefault() as BorrowDTO;

            return borrow!.UserId;
        }

        // Order filling
        private void FillOperation()
        {
            Operation.OperationName = nameof(StockOperations.OperationType.Return);
            Operation.UserId = _authService.GetUser()!.Id;
            Operation.UserName = _authService.GetUser()!.UserName;
            Operation.UserRole = _authService.GetUser()!.UserName; // Adding UserName until UserRole is implemented.
            Operation.TotalPrice = 0;
            Operation.OrderList = new List<IOrder>
            {
                new Order
                {
                    ItemId = BorrowedBook!.Id.ToString(),
                    ItemName = BorrowedBook.Title,
                    ItemISBN = BorrowedBook.ISBN,
                    Price = 0,
                    Quantity = 1
                }
            };
            Operation.OperationDate = DateTime.Now;
            Operation.BorrowerId = BorrowerId;
        }
    }
}
