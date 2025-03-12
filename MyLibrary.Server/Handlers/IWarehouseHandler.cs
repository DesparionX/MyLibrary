using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IWarehouseHandler<IWarehouse>
    {
        public Task<ITaskResponse> CreateStockAsync<TId>(IWarehouseDTO<TId> warehouseDTO) where TId : IEquatable<TId>;
        public Task<ITaskResponse> CreateStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResponse> AddStockAsync<TId>(IWarehouseDTO<TId> warehouseDTO) where TId : IEquatable<TId>;
        public Task<ITaskResponse> AddStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResponse> RemoveStockAsync<TId>(IWarehouseDTO<TId> warehouseDTO) where TId : IEquatable<TId>;
        public Task<ITaskResponse> RemoveStockAsync(IWarehouseDTO warehouseDTO);
        public Task<ITaskResponse> DeleteStockAsync<TId>(TId id);

    }
}
