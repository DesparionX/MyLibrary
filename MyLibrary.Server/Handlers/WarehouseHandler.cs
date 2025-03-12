using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public class WarehouseHandler : IWarehouseHandler<Warehouse>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WarehouseHandler> _logger;
        private readonly IMapper _mapper;

        public WarehouseHandler(AppDbContext context, ILogger<WarehouseHandler> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public Task<ITaskResponse> CreateStockAsync<TId>(IWarehouseDTO<TId> warehouseDTO) where TId : IEquatable<TId>
        {
            throw new NotImplementedException();
        }

        public Task<ITaskResponse> CreateStockAsync(IWarehouseDTO warehouseDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ITaskResponse> DeleteStockAsync<TId>(TId id)
        {
            try
            {
                var itemToDelete = await _context.Warehouse
                    .Where(w => w.ISBN.Equals(id))
                    .ExecuteDeleteAsync();
                if (itemToDelete > 0)
                {
                    return new WarehouseTaskResponse(succeeded: true, message: "Item was deleted.", statusCode: StatusCodes.Status200OK);
                }
                return new WarehouseTaskResponse(succeeded: false, message: "Item was not found.", statusCode: StatusCodes.Status404NotFound);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResponse(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResponse> AddStockAsync<TId>(IWarehouseDTO<TId> warehouseDTO) where TId : IEquatable<TId>
        {
            // TODO: Implement this method later.
            return new WarehouseTaskResponse(succeeded: false, message: "Not implemented yet.", statusCode: StatusCodes.Status501NotImplemented);
        }

        public async Task<ITaskResponse> AddStockAsync(IWarehouseDTO warehouseDTO)
        {
            try
            {
                var itemToUpdate = await _context.Warehouse
                    .Where(w => w.ISBN.Equals(warehouseDTO.ISBN))
                    .ExecuteUpdateAsync(w => w.SetProperty(w => w.Quantity, + warehouseDTO.Quantity));
                if (itemToUpdate > 0)
                {
                    return new WarehouseTaskResponse(succeeded: false, message: "0 items were added.", statusCode: StatusCodes.Status304NotModified);
                }
                return new WarehouseTaskResponse(succeeded: true, message: $"{warehouseDTO.Quantity} items were added.", statusCode: StatusCodes.Status200OK);
            }
            catch(Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResponse(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public Task<ITaskResponse> RemoveStockAsync<TId>(IWarehouseDTO<TId> warehouseDTO) where TId : IEquatable<TId>
        {
            throw new NotImplementedException();
        }

        public async Task<ITaskResponse> RemoveStockAsync(IWarehouseDTO warehouseDTO)
        {
            try
            {
                var itemInDb = await _context.Warehouse.Where(w => w.ISBN.Equals(warehouseDTO.ISBN))
                    .FirstOrDefaultAsync();

                if(itemInDb!.Quantity < warehouseDTO.Quantity)
                {
                    return new WarehouseTaskResponse(succeeded: false, message: "Not enough items in stock.", statusCode: StatusCodes.Status400BadRequest);
                }

                var itemToUpdate = await _context.Warehouse
                    .Where(w => w.ISBN.Equals(warehouseDTO.ISBN))
                    .ExecuteUpdateAsync(w => w.SetProperty(w => w.Quantity, - warehouseDTO.Quantity));
                if (itemToUpdate > 0)
                {
                    return new WarehouseTaskResponse(succeeded: false, message: "0 items were removed.", statusCode: StatusCodes.Status304NotModified);
                }
                return new WarehouseTaskResponse(succeeded: true, message: $"{warehouseDTO.Quantity} items were removed.", statusCode: StatusCodes.Status200OK);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResponse(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}
