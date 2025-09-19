using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyLibrary.Configs;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;
using System.Net.Http;
using System.Net.Http.Json;

namespace MyLibrary.Services.Api
{
    public class OperationService : IOperationService
    {
        private readonly ApiService _apiService;

        public OperationService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public Task<ITaskResult?> GetOperationHistoryAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ITaskResult?> PerformOperation(IOperationDTO? operationDto)
        {
            try
            {
                var url = _apiService.ApiSettings.BaseUrl + _apiService.ApiSettings.Controllers.Operations.PerformOperation;
                var response = await _apiService.HttpClient.PostAsJsonAsync(url, operationDto);

                return await response.Content.ReadFromJsonAsync<OperationTaskResult>();
            }
            catch (Exception err)
            {
                _apiService.NotificationService.ShowError(title: "Error", message: $"Error performing operation. \n {err.Message}");
                return new OperationTaskResult(succeeded: false, statusCode: StatusCodes.Status500InternalServerError, message: err.Message);
            }
        }
    }
}
