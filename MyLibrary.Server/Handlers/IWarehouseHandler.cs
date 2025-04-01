using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IWarehouseHandler<IWarehouse>
    {
        public Task<ITaskResult> GetStockAsync(string isbn);
        public Task<ITaskResult> GetAllStocksAsync();
        public Task<ITaskResult> CreateStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> AddStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> RemoveStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> DeleteStockAsync(int id);
    }
}
