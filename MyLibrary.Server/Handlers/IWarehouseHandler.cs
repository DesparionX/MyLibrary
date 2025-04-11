using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLibrary.Server.Data.DTOs.Interfaces;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IWarehouseHandler<IWarehouse>
    {
        public Task<ITaskResult> GetStockAsync(string isbn);
        public Task<ITaskResult> GetAllStocksAsync();
        public Task<ITaskResult> CreateStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> AddStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> AddStocksAsync(ICollection<IWarehouseDTO> warehouseDTOs);
        public Task<ITaskResult> UpdateStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> RemoveStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> RemoveStocksAsync(ICollection<IWarehouseDTO> warehouseDTOs);
        public Task<ITaskResult> DeleteStockAsync(int id);
    }
}
