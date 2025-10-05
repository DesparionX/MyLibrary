using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyLibrary.Configs;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyLibrary.Services.Api
{
    public class BookService : IBookService
    {
        private readonly ApiSettings _apiSettings;
        private readonly HttpClient _httpClient;
        private readonly INotificationService _notificationService;

        public BookService(IOptions<ApiSettings> apiSettings, IHttpClientFactory httpClientFactory, INotificationService notificationService)
        {
            _apiSettings = apiSettings.Value;
            _httpClient = httpClientFactory.CreateClient("LibStore");
            _notificationService = notificationService;
        }

        public Task<ITaskResult> AddBookAsync(INewBook<BookDTO> newBookDto)
        {
            throw new NotImplementedException();
        }

        public Task<ITaskResult> DeleteBookAsync<T>(T id) where T : IEquatable<T>
        {
            throw new NotImplementedException();
        }

        public async Task<ITaskResult> FindBookByIdAsync(string bookId)
        {
            try
            {
                var requestUrl = _apiSettings.BaseUrl + _apiSettings.Controllers.Books.FindById;
                var param = ($"/{Uri.EscapeDataString(bookId) ?? ""}");
                var response = await _httpClient.GetFromJsonAsync<BookTaskResult>(requestUrl + param);

                if (response is null)
                {
                    _notificationService.ShowError(title: Strings.Error, message: Strings.Errors_Api_NullResponse);
                    return new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status500InternalServerError, message: Strings.Errors_Api_NullResponse);
                }

                return response;
            }
            catch(Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: $"{Strings.BookService_Errors_ErrorFetchingBookByID} \n {err.Message}");
                return new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status500InternalServerError, message: err.Message);
            }
        }

        public async Task<ITaskResult> FindBookByISBNAsync(string isbn, bool? isAvailable)
        {
            try
            {
                var requestUrl = _apiSettings.BaseUrl + _apiSettings.Controllers.Books.FindByISBN;
                var param = ($"/{Uri.EscapeDataString(isbn) ?? ""}+{isAvailable}");
                var response = await _httpClient.GetFromJsonAsync<BookTaskResult>(requestUrl + param);

                if (response is null)
                {
                    _notificationService.ShowError(title: Strings.Error, message: Strings.Errors_Api_NullResponse);
                    return new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status500InternalServerError, message: Strings.Errors_Api_NullResponse);
                }

                return response;
            }
            catch (Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: $"{Strings.BookService_Errors_ErrorFetchingBookByISBN} \n {err.Message}");
                return new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status500InternalServerError, message: err.Message);
            }
        }

        public Task<ITaskResult> GetAllBooksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ITaskResult> UpdateBookAsync(IBookDTO bookDto)
        {
            throw new NotImplementedException();
        }
    }
}
