using CommunityToolkit.Mvvm.ComponentModel;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Services.Api;
using MyLibrary.Shared.Interfaces.IDTOs;
using MyLibrary.Views;
using System.Windows;

namespace MyLibrary.ViewModels
{
    public partial class AddItemToReceiptViewModel : ObservableObject
    {
        private readonly IBookService _bookService;

        [ObservableProperty] private Window? _dialog;
        [ObservableProperty] private string _isbn = string.Empty;
        [ObservableProperty] private string _receiptErrors = string.Empty;
        public Visibility ErrorsVisibility => 
            string.IsNullOrWhiteSpace(ReceiptErrors) ? Visibility.Visible : Visibility.Collapsed;

        public IBookDTO? Book { get; set; }

        public AddItemToReceiptViewModel(IBookService bookService)
        {
            _bookService = bookService;
        }
        
        public async Task Confirm()
        {
            if (!IsValidISBN())
                return;

            Book = await GetBook();

            if (Book is not null)
            {
                Dialog?.DialogResult = true;
                Dialog?.Close();
            }
        }

        public void Cancel()
        {
            Book = null;
            Dialog?.DialogResult = false;
            Dialog?.Close();
        }

        // API Call to get the book by ISBN
        private async Task<IBookDTO?> GetBook()
        {
            try
            {
                var response = await _bookService.FindBookByISBNAsync(Isbn) as BookTaskResult;
                if (response!.Succeeded)
                    return response.Book;
                else
                {
                    // Add proper 404 Message.
                    ReceiptErrors += Strings.BookService_Errors_ErrorFetchingBookByISBN + Environment.NewLine;
                    return null;
                }
            }
            catch (Exception err)
            {
                ReceiptErrors += err.Message + Environment.NewLine;
                return null;
            }
        }

        // Validations
        public void ValidateISBN()
        {
            ReceiptErrors = string.Empty;

            if (string.IsNullOrWhiteSpace(Isbn))
            {
                ReceiptErrors += "ISBN cannot be empty." + Environment.NewLine;
            }
        }

        private bool IsValidISBN() => string.IsNullOrWhiteSpace(ReceiptErrors);
    }
}
