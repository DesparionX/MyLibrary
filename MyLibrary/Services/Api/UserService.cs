using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyLibrary.Configs;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.Api
{
    public class UserService : IUserService
    {
        private readonly ApiSettings _apiSettings;
        private readonly HttpClient _httpClient;
        private readonly INotificationService _notificationService;

        public UserService(IOptions<ApiSettings> apiSettings, IHttpClientFactory httpClientFactory, INotificationService notificationService)
        {
            _apiSettings = apiSettings.Value;
            _httpClient = httpClientFactory.CreateClient("LibStore");
            _notificationService = notificationService;
        }
        public Task<ITaskResult> GetUserIdentityAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ITaskResult> LogInAsync(ILoginRequest request)
        {
            try
            {
                var requestUrl = _apiSettings.BaseUrl + _apiSettings.Controllers.Account.Login;
                var response = await _httpClient.PostAsJsonAsync(requestUrl, request);
                return await response.Content.ReadFromJsonAsync<AuthResult>();
            }
            catch(Exception err)
            {
                _notificationService.ShowError(title: Strings.Errors_UserService_LoginFailed, message: err.Message);
                return new AuthResult(succeeded: false, statusCode: StatusCodes.Status500InternalServerError, message: err.Message);
            }
        }

        public Task<ITaskResult> RegisterUserAsync(INewUser newUser)
        {
            throw new NotImplementedException();
        }
    }
}
