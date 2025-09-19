using Microsoft.Extensions.Options;
using MyLibrary.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.Api
{
    public class ApiService(IOptions<ApiSettings> apiSettings, IHttpClientFactory httpClientFactory, INotificationService notificationService)
    {
        public ApiSettings ApiSettings { get; } = apiSettings.Value;
        public HttpClient HttpClient { get; } = httpClientFactory.CreateClient("LibStore");
        public INotificationService NotificationService { get; } = notificationService;
    }
}
