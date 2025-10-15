using Microsoft.AspNetCore.Http;
using MyLibrary.Resources.Languages;
using MyLibrary.Server.Http.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.Api
{
    public class WarehouseService : IWarehouseService
    {
        private readonly ApiService _apiService;

        public WarehouseService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ITaskResult> GetStockAsync(string isbn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(isbn))
                    throw new ArgumentException(Strings.WarehouseService_Errors_EmptyISBN, nameof(isbn));

                var url = _apiService.ApiSettings.BaseUrl
                    + _apiService.ApiSettings.Controllers.Warehouse.GetStock;

                var param = ($"/{Uri.EscapeDataString(isbn) ?? ""}");

                var response = await _apiService.HttpClient.GetFromJsonAsync<WarehouseTaskResult>(url + param) 
                    ?? throw new ArgumentException(Strings.Errors_Api_NullResponse);

                return response;
            }
            catch (ArgumentException err)
            {
                _apiService.NotificationService.ShowError(title: Strings.Error, message: err.Message);
                return new WarehouseTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: err.Message);
            }
            catch (Exception)
            {
                _apiService.NotificationService.ShowError(title: Strings.Error, message: Strings.WarehouseService_Errors_NotFound);
                return new WarehouseTaskResult(succeeded: false, statusCode: StatusCodes.Status500InternalServerError, message: Strings.WarehouseService_Errors_NotFound);
            }
        }
    }
}
