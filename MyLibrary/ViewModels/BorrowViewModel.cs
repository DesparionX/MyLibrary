using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Helpers;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Shared.Interfaces.IDTOs;
using System.Windows;

namespace MyLibrary.ViewModels
{
    public partial class BorrowViewModel : ObservableObject
    {
        private readonly IBookService _bookService;
        private readonly IBorrowService _borrowService;
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IOperationService _operationService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        private string _bookId = string.Empty;

        [ObservableProperty]
        private string _borrowerId = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BookIdErrorsVisibility))]
        private string _bookIdErrors = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorrowerIdErrorsVisibility))]
        private string _borrowerIdErrors = string.Empty;

        [ObservableProperty]
        private IOperationDTO _operation = new OperationDTO();

        public Visibility BookIdErrorsVisibility 
            => string.IsNullOrWhiteSpace(BookIdErrors) ? Visibility.Collapsed : Visibility.Visible;

        public Visibility BorrowerIdErrorsVisibility
            => string.IsNullOrWhiteSpace(BorrowerIdErrors) ? Visibility.Collapsed : Visibility.Visible;


        public BorrowViewModel(
            IBookService bookService,
            IBorrowService borrowService,
            IAuthService authService,
            INavigationService navigationService,
            IOperationService operationService,
            INotificationService notificationService)
        {
            _bookService = bookService;
            _borrowService = borrowService;
            _authService = authService;
            _navigationService = navigationService;
            _operationService = operationService;
            _notificationService = notificationService;
        }

        [RelayCommand]
        public async Task BorrowAsync()
        {
            if (!IsValidBorrow())
                return;

            await FillOperation();

            if (Operation.OrderList is null || Operation.OrderList.Count == 0)
            {
                _notificationService.ShowError(title: Strings.CustomDialog_Title_Fail, message: Strings.Borrows_Missing_Book);
                return;
            }

            var result = await _operationService.PerformOperation(Operation) as OperationTaskResult;

            if (result == null || !result.Succeeded)
            {
                _notificationService.ShowError(title: Strings.CustomDialog_Title_Fail, message: Strings.Borrows_Failed_To_Borrow);
                return;
            }

            _notificationService.ShowSuccess(title: Strings.CustomDialog_Title_Fail, message: Strings.Borrows_Successfully_Borrowed);
            _navigationService.BackToHomeView();
        }

        // Fill operation
        private async Task FillOperation()
        {
            Operation.OperationName = nameof(StockOperations.OperationType.Borrow);
            Operation.UserId = _authService.GetUser()!.Id;
            Operation.UserName = _authService.GetUser()!.UserName;
            Operation.UserRole = _authService.GetUser()!.UserName; // Adding UserName until UserRole is implemented.
            Operation.TotalPrice = 0;
            Operation.OrderList = new List<IOrder>
            {
                await CreateOrderAsync(BookId)
            };
            Operation.OperationDate = DateTime.Now;
            Operation.BorrowerId = BorrowerId;


        }

        // Fetch the book from DB
        private async Task<IOrder> CreateOrderAsync(string bookId)
        {
            var book = (await _bookService.FindBookByIdAsync(bookId) as BookTaskResult)!.Book!;
            var order = new Order
            {
                ItemId = book.Id.ToString(),
                ItemName = book.Title,
                ItemISBN = book.ISBN,
                Price = 0,
                Quantity = 1
            };

            return order;
        }

        // Validations
        partial void OnBookIdChanged(string value)
        {
            BookIdErrors = string.Empty;
            BookIdErrors += ValidateId(value);
        }

        partial void OnBorrowerIdChanged(string value)
        {
            BorrowerIdErrors = string.Empty;
            BorrowerIdErrors += ValidateId(value);
        }

        private string ValidateId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Strings.Borrows_Validations_EmptyID + Environment.NewLine;

            if (!Guid.TryParse(id, out var guid) && guid == Guid.Empty)
                return Strings.Borrows_Validations_InvalidID + Environment.NewLine;

            return string.Empty;
        }

        private bool IsValidBorrow()
        {
            BookIdErrors = string.Empty;
            BookIdErrors += ValidateId(BookId);

            BorrowerIdErrors = string.Empty;
            BorrowerIdErrors += ValidateId(BorrowerId);

            return string.IsNullOrEmpty(BorrowerIdErrors) && string.IsNullOrEmpty(BookIdErrors);
        }
    }
}
