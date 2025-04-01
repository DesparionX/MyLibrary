using MyLibrary.Server.Data.DTOs;

namespace MyLibrary.Server.Http.Responses
{
    public class WarehouseTaskResult : ITaskResult
    {
        public string? Message { get; }

        public bool Succeeded { get; }

        public int StatusCode { get; }
        public WarehouseDTO? StockDTO { get; }
        public ICollection<WarehouseDTO>? StockDTOs { get; set; }

        public WarehouseTaskResult(bool succeeded, int statusCode, string? message = "", WarehouseDTO? stockDto = null, ICollection<WarehouseDTO>? stockDtos = null)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = message;
            StockDTO = stockDto;
            StockDTOs = stockDtos;
        }
    }
}
