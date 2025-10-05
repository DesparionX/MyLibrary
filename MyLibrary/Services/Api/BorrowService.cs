using Microsoft.AspNetCore.Http;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Http.Responses;
using System.Net.Http.Json;

namespace MyLibrary.Services.Api
{
    public class BorrowService : IBorrowService
    {
        private readonly ApiService _apiService;

        public BorrowService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public Task<ITaskResult> GetBorrowHistoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ITaskResult> GetUserBorrowsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ITaskResult> GetBooksBorrowsAsync(string bookId, bool? isReturned = null)
        {
            try
            {
                var url = _apiService.ApiSettings.BaseUrl + _apiService.ApiSettings.Controllers.Borrows.GetBooksBorrows;
                var param = ($"/{Uri.EscapeDataString(bookId) ?? ""}+{isReturned}");

                var response = await _apiService.HttpClient.GetFromJsonAsync<BorrowTaskResult>(url + param);

                if (response is null)
                {
                    _apiService.NotificationService.ShowError(title: Strings.CustomDialog_Title_Fail, message: Strings.Errors_Api_NullResponse);
                    return new BorrowTaskResult(succeeded: false, statusCode: StatusCodes.Status500InternalServerError, message: Strings.Errors_Api_NullResponse);
                }

                return response;
            }
            catch (Exception err)
            {
                _apiService.NotificationService.ShowError(title: Strings.CustomDialog_Title_Fail, message: Strings.Errors_Borrows_FailedToGetBorrow);
                return new BorrowTaskResult(succeeded: false, statusCode: StatusCodes.Status500InternalServerError, message: Strings.Errors_Borrows_FailedToGetBorrow);
            }
        }

        public Task<ITaskResult> BorrowBookAsync(string bookId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ITaskResult> ReturnBookAsync(string bookId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
