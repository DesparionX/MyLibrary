using MyLibrary.Resources.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services
{
    class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;
        public AuthHeaderHandler(IAuthService authService, INotificationService notificationService)
        {
            _authService = authService;
            _notificationService = notificationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (_authService.IsAuthenticated())
                {
                    var token = _authService.GetToken();
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                return await base.SendAsync(request, cancellationToken);
            }
            catch(Exception err)
            {
                _notificationService.ShowError(title: Strings.Error, message: Strings.Errors_AuthHeader_FailedSendingRequest + err.Message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(Strings.Errors_AuthHeader_FailedSendingRequest + err.Message)
                };
            }
        }
    }
}
