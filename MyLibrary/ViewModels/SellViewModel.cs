using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;
using MyLibrary.Configs;
using MyLibrary.Helpers;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Helpers;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.ViewModels
{
    public partial class SellViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly INavigationService _navigationService;
        private readonly IValidationService _validationService;
        private readonly IBookService _bookService;
        private readonly IAuthService _authService;
        private readonly ApiSettings _apiSettings;

        [ObservableProperty]
        private IOperationDTO _operation = new OperationDTO();

        [ObservableProperty]
        private ICollection<IOrder> _orders = [];

        [ObservableProperty]
        private ObservableCollection<BookDTO?> _booksFromReceipt = [];

        [ObservableProperty]
        private BookDTO? _selectedBookFromReceipt;

        [ObservableProperty]
        private ObservableCollection<DisplayOrder> _displayOrders = [];

        [ObservableProperty]
        private List<string> _receiptErrors = [];

        public SellViewModel(INotificationService notificationService,
            INavigationService navigationService,
            IAuthService authService,
            IBookService bookService,
            IOptions<ApiSettings> apiSettings,
            IValidationService validationService)
        {
            _notificationService = notificationService;
            _navigationService = navigationService;
            _authService = authService;
            _bookService = bookService;
            _apiSettings = apiSettings.Value;
            _validationService = validationService;

            InitializeOrder();
            AddFakeOrders();
            FakeReceiptBooksAdd();
        }

        public async Task<bool> SellAsync(IOperationDTO operation)
        {
            Operation.OperationDate = DateTime.Now;
            return true;
        }
        public async Task AddOrder(IOrder order)
        {
            if(await IsValidOrder(order))
            {
                Orders.Add(order);
                DisplayOrders.Add(new DisplayOrder(order));
                await AddBookFromOrder(order.ItemISBN!);
            }
        }
        public void SelectBookFromReceipt(string isbn)
        {
            SelectedBookFromReceipt = BooksFromReceipt.FirstOrDefault(b => b?.ISBN == isbn);
        }

        private async Task<BookDTO?> GetBookByISBN(string isbn)
        {
            try
            {
                var response = await _bookService.FindBookByISBNAsync(isbn) as BookTaskResult;
                if (response!.Succeeded)
                    return response.Book;
                else
                {
                    ReceiptErrors.Add(response.Message!);
                    return null;
                }
            }
            catch (Exception err)
            {
                ReceiptErrors.Add(err.Message);
                return null;
            }
        }

        private async Task<bool> IsValidOrder(IOrder order)
        {
            var orderCheck = await _validationService.ValidateOrder(order);
            if (!orderCheck.IsValid)
            {
                orderCheck.Errors!.ToList().ForEach(error => ReceiptErrors.Add(error));
            }

            return orderCheck.IsValid;
        }

        private async Task AddBookFromOrder(string isbn)
        {
            var book = await GetBookByISBN(isbn);
            if (book != null)
            {
                BooksFromReceipt.Add(book);
            }
            else
            {
                ReceiptErrors.Add("Book not found with ISBN: " + isbn);
            }
        }

        private void InitializeOrder()
        {
            Operation.OperationName = nameof(StockOperations.OperationType.Sell);
            Operation.OrderList = Orders;
            Operation.UserId = _authService.GetUser().Id;
            Operation.UserName = _authService.GetUser().UserName;
            Operation.UserRole = _authService.GetUser().UserName; // Assing UserName until UserRole is implemented.
        }

        private void AddFakeOrders()
        {
            DisplayOrders.Add(new DisplayOrder(
                new Order { ItemISBN = "12345678", ItemId = "ff8ca4e3-0026-4399-8eac-1236e7cc1422", ItemName = "BEST BOOK", Quantity = 1, Price = 56.00m }));
            DisplayOrders.Add(new DisplayOrder(
                new Order { ItemISBN = "12345678", ItemId = "ffe07648-78e4-4e33-8e9d-5d28a7219bfa", ItemName = "BEST BOOK", Quantity = 1, Price = 56.00m }));
            DisplayOrders.Add(new DisplayOrder(
                new Order { ItemISBN = "12345678", ItemId = "abededc0-f902-4c51-9439-6ce8b76fe29c", ItemName = "BEST BOOK", Quantity = 1, Price = 56.00m }));
            DisplayOrders.Add(new DisplayOrder(
                new Order { ItemISBN = "53433 333 22", ItemId = "a2c9e362-5a08-4ebf-b604-0965e97057c8", ItemName = "IT: Chapter TWO", Quantity = 1, Price = 45.00m }));
            DisplayOrders.Add(new DisplayOrder(
                new Order { ItemISBN = "53433 333 22", ItemId = "c6d97756-c844-490f-a14d-3c70d271c092", ItemName = "IT: Chapter TWO", Quantity = 1, Price = 45.00m }));
            DisplayOrders.Add(new DisplayOrder(
                new Order { ItemISBN = "53433 333 22", ItemId = "d055ecdd-2087-4362-8250-570de80bc927", ItemName = "IT: Chapter TWO", Quantity = 1, Price = 45.00m }));
        }
        private async Task FakeReceiptBooksAdd()
        {
            foreach(var order in DisplayOrders)
            {
                await AddBookFromOrder(order.ItemISBN!);
            }
        }
    }
}
