using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IWarehouseHandler<IWarehouse>
    {
        public Task<ITaskResult> CreateStockAsync<TId>(IWarehouseDTO<TId> warehouseDTO) where TId : IEquatable<TId>;
        public Task<ITaskResult> CreateStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> AddStockAsync<TId>(IWarehouseDTO<TId> warehouseDTO) where TId : IEquatable<TId>;
        public Task<ITaskResult> AddStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> RemoveStockAsync<TId>(IWarehouseDTO<TId> warehouseDTO) where TId : IEquatable<TId>;
        public Task<ITaskResult> RemoveStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResult> DeleteStockAsync<TId>(TId id);

    }
}
