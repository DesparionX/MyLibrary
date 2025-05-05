using Microsoft.Extensions.Options;
using MyLibrary.Configs;
using MyLibrary.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyLibrary.Services.Api
{
    public class ApiTestService : IApiTestService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly INotificationService _notificationService;

        public ApiTestService(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings, INotificationService notificationService)
        {
            _httpClient = httpClientFactory.CreateClient("LibStore");
            _apiSettings = apiSettings.Value;
            _notificationService = notificationService;
        }
        public async Task<bool> IsApiOnline()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiSettings.BaseUrl + _apiSettings.Controllers.HealthChecks.ApiStatus);
                return response.IsSuccessStatusCode;
            }
            catch (Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: err.Message);
                return false;
            }
        }
        public async Task<bool> IsDbOnline()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiSettings.BaseUrl + _apiSettings.Controllers.HealthChecks.ApiUI);
                return response.IsSuccessStatusCode;
            }
            catch(Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: err.Message);
                return false;
            }
        }
    }
}
