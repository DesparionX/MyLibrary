using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MyLibrary.Helpers;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Helpers;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.Shared.Interfaces.IDTOs;
using MyLibrary.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace MyLibrary.ViewModels
{
    public partial class SellViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly INavigationService _navigationService;
        private readonly IValidationService _validationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileService _fileService;
        private readonly IOperationService _operationService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private IOperationDTO _operation = new OperationDTO();

        [ObservableProperty]
        private ObservableCollection<IOrder> _orders = [];

        [ObservableProperty] 
        private ObservableCollection<IBookDTO?> _booksFromReceipt = [];

        [ObservableProperty] 
        private IBookDTO? _selectedBookFromReceipt;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FinishVisibility))]
        private ObservableCollection<DisplayOrder> _displayOrders = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ErrorsVisibility))]
        private string _receiptErrors = string.Empty;

        public Visibility ErrorsVisibility =>
            string.IsNullOrWhiteSpace(ReceiptErrors) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility FinishVisibility =>
            DisplayOrders.Count == 0 ? Visibility.Collapsed : Visibility.Visible;

        public SellViewModel(INotificationService notificationService,
            INavigationService navigationService,
            IAuthService authService,
            IFileService fileService,
            IServiceProvider serviceProvider,
            IOperationService operationService,
            IValidationService validationService)
        {
            _notificationService = notificationService;
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            _authService = authService;
            _fileService = fileService;
            _operationService = operationService;
            _validationService = validationService;

            
            DisplayOrders.CollectionChanged += (s, e) => OnPropertyChanged(nameof(FinishVisibility));
        }

        [RelayCommand]
        public async Task SellAsync()
        {
            if(Orders is null || Orders.Count == 0)
            {
                ReceiptErrors += Strings.Errors_Receipt_ZeroOrders + Environment.NewLine;
                return;
            }

            InitializeReceipt();
            var result = await _operationService.PerformOperation(Operation);
            if (!result.Succeeded)
            {
                ReceiptErrors += $"{result.Message}{Environment.NewLine}";
            }

            // Implement Print Receipt functionality.
            _notificationService.ShowSuccess(title: Strings.CustomDialog_Title_Success, message: result.Message!);
            _navigationService.BackToHomeView();
        }

        // Takes a BookDTO, creates an Order from it and adds it to the Receipt if valid.
        public async Task AddItemToReceipt(IBookDTO? book)
        {
            var order = CreateOrder(book);

            if(await IsValidOrder(order!))
            {
                Orders.Add(order!);
                DisplayOrders.Add(new DisplayOrder(order!));
                BooksFromReceipt.Add(book!);
            }
        }

        [RelayCommand]
        public void DeleteItemFromReceipt(string? itemId)
        {
            // Remove the item from all relevant collections.
            DisplayOrders.Remove(DisplayOrders.FirstOrDefault(i => i.ItemId!.Equals(itemId))!);
            Orders.Remove(Orders.FirstOrDefault(i => i.ItemId!.Equals(itemId))!);
            BooksFromReceipt.Remove(BooksFromReceipt.FirstOrDefault(i => i!.Id.ToString().Equals(itemId))!);
            SelectedBookFromReceipt = null;
        }

        // Sets the SelectedBookFromReceipt property based on the provided ISBN.
        public void SelectBookFromReceipt(string isbn)
        {
            SelectedBookFromReceipt = BooksFromReceipt.FirstOrDefault(b => b?.ISBN == isbn);
            SelectedBookFromReceipt?.Picture = _fileService.GetImageUrl(SelectedBookFromReceipt.Picture);
        }

        // Opens a dialog and retrieves a BookDTO from it, then adds it to the Receipt.
        public async Task OpenAddItemDialog()
        {
            var scope = _serviceProvider.CreateScope();
            var addItemWindow = scope.ServiceProvider.GetRequiredService<AddItemToReceiptWindow>();
            
            if (addItemWindow.ShowDialog() is true)
            {
                var model = addItemWindow.DataContext as AddItemToReceiptViewModel;
                await AddItemToReceipt(model?.Book);
            }
        }

        // Initializes the receipt with default values.
        private void InitializeReceipt()
        {
            Operation.OperationName = nameof(StockOperations.OperationType.Sell);
            Operation.OrderList = Orders;
            Operation.UserId = _authService.GetUser()!.Id;
            Operation.UserName = _authService.GetUser()!.UserName;
            Operation.UserRole = _authService.GetUser()!.UserName; // Adding UserName until UserRole is implemented.
            Operation.OperationDate = DateTime.Now;
            Operation.TotalPrice = Orders.Sum(o => o.Price * o.Quantity);
        }

        // Recieves BookDTO and creates an Order from it.
        private IOrder? CreateOrder(IBookDTO? book)
        {
            return new Order
            {
                ItemId = book?.Id.ToString(),
                ItemISBN = book?.ISBN,
                ItemName = book?.Title,
                Price = book?.BasePrice,
                Quantity = 1
            };
        }

        // Validates the given order before adding it to the receipt.
        private async Task<bool> IsValidOrder(IOrder order)
        {
            var orderCheck = await _validationService.ValidateOrder(order);
            if (!orderCheck.IsValid)
            {
                orderCheck.Errors!.ToList().ForEach(error => ReceiptErrors += error + Environment.NewLine);
            }

            return orderCheck.IsValid;
        }
    }
}
