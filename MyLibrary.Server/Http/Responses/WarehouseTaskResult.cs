using MyLibrary.Server.Data.DTOs;

namespace MyLibrary.Server.Http.Responses
{
    public class WarehouseTaskResult : ITaskResult
    {
        public string? Message { get; }

        public bool Succeeded { get; }

        public int StatusCode { get; }
        public IWarehouseDTO? StockDTO { get; }
        public ICollection<IWarehouseDTO>? StockDTOs { get; set; }

        public WarehouseTaskResult(bool succeeded, int statusCode, string? message = "", IWarehouseDTO? stockDto = null, ICollection<IWarehouseDTO>? stockDtos = null)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = message;
            StockDTO = stockDto;
            StockDTOs = stockDtos;
        }
    }
}
