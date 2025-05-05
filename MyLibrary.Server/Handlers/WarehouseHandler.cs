using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Shared.Interfaces.IDTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Events;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;
using System.Linq.Expressions;

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

        public async Task<ITaskResult> GetStockAsync(string isbn)
        {
            try
            {
                var stock = await _context.Warehouse.Where(w => w.ISBN.Equals(isbn)).FirstOrDefaultAsync();
                if (stock == null)
                {
                    _logger.LogInformation($"[INFO] Item with ISBN: {isbn} was not found.");
                    return new WarehouseTaskResult(succeeded: false, message: "Item not found.", statusCode: StatusCodes.Status404NotFound);
                }

                var stockDTO = _mapper.Map<WarehouseDTO>(stock);
                _logger.LogInformation($"[INFO] Item with ISBN: {isbn} was found.");
                return new WarehouseTaskResult(succeeded: true, message: "Item found.", statusCode: StatusCodes.Status200OK, stockDto: stockDTO);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> GetAllStocksAsync()
        {
            try
            {
                var stocks = await _context.Warehouse.ToListAsync();
                if (stocks.Count == 0)
                {
                    _logger.LogInformation($"[INFO] No items found.");
                    return new WarehouseTaskResult(succeeded: false, message: "No items found.", statusCode: StatusCodes.Status404NotFound);
                }

                var stockDTOs = _mapper.Map<ICollection<WarehouseDTO>>(stocks);
                _logger.LogInformation($"[INFO] Items found.");
                return new WarehouseTaskResult(succeeded: true, message: "Items found.", statusCode: StatusCodes.Status200OK, stockDtos: stockDTOs);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> CreateStockAsync(IWarehouseDTO warehouseDTO)
        {
            try
            {
                if (await _context.Warehouse.AnyAsync(w => w.ISBN.Equals(warehouseDTO.ISBN)))
                {
                    return new WarehouseTaskResult(succeeded: false, message: "Item already exists.", statusCode: StatusCodes.Status409Conflict);
                }

                var itemToAdd = _mapper.Map<Warehouse>(warehouseDTO);
                await _context.Warehouse.AddAsync(itemToAdd);

                var itemAdded = await _context.SaveChangesAsync();
                if (itemAdded > 0)
                {
                    _logger.LogInformation($"[INFO] Item with ISBN: {warehouseDTO.ISBN} was created.");
                    return new WarehouseTaskResult(succeeded: true, message: "Item was added.", statusCode: StatusCodes.Status200OK);
                }

                return new WarehouseTaskResult(succeeded: false, message: "Item was not added.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> DeleteStockAsync<TId>(TId id) where TId : IEquatable<TId>
        {
            try
            {
                var itemToDelete = await _context.Warehouse
                    .Where(w => w.Id.Equals(id))
                    .ExecuteDeleteAsync();
                if (itemToDelete > 0)
                {
                    _logger.LogInformation($"[INFO] Item with ID: {id} was deleted.");
                    return new WarehouseTaskResult(succeeded: true, message: "Item was deleted.", statusCode: StatusCodes.Status200OK);
                }
                return new WarehouseTaskResult(succeeded: false, message: "Item was not found.", statusCode: StatusCodes.Status404NotFound);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> DeleteStockAsync(int id)
        {
            try
            {
                var itemToDelete = await _context.Warehouse
                    .Where(w => w.Id.Equals(id))
                    .ExecuteDeleteAsync();
                if (itemToDelete > 0)
                {
                    _logger.LogInformation($"[INFO] Item with ID: {id} was deleted.");
                    return new WarehouseTaskResult(succeeded: true, message: "Item was deleted.", statusCode: StatusCodes.Status200OK);
                }
                return new WarehouseTaskResult(succeeded: false, message: "Item was not found.", statusCode: StatusCodes.Status404NotFound);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> AddStockAsync(IWarehouseDTO warehouseDTO)
        {
            try
            {
                // Check if the stock exist, if not create it.
                if (!await _context.Warehouse.AnyAsync(w => w.ISBN.Equals(warehouseDTO.ISBN)))
                {
                    return await CreateStockAsync(warehouseDTO);
                }

                var itemToUpdate = await _context.Warehouse
                    .Where(w => w.ISBN.Equals(warehouseDTO.ISBN))
                    .ExecuteUpdateAsync(w => w.SetProperty(w => w.Quantity, w => w.Quantity + warehouseDTO.Quantity));
                if (itemToUpdate > 0)
                {
                    _logger.LogInformation($"[INFO] {warehouseDTO.Quantity} items were added to the stock.");
                    return new WarehouseTaskResult(succeeded: true, message: $"{warehouseDTO.Quantity} items were added.", statusCode: StatusCodes.Status200OK);

                }
                return new WarehouseTaskResult(succeeded: false, message: "0 items were added.", statusCode: StatusCodes.Status304NotModified);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> AddStocksAsync(ICollection<IWarehouseDTO> warehouseDTOs)
        {
            try
            {
                var addedItems = 0;
                foreach (var dto in warehouseDTOs)
                {
                    var result = await AddStockAsync(dto);
                    addedItems = result.Succeeded ? addedItems++ : addedItems;
                }

                if (addedItems == warehouseDTOs.Count)
                {
                    _logger.LogInformation($"[INFO] {addedItems} items were added to the stock.");
                    return new WarehouseTaskResult(succeeded: true, message: $"{addedItems} items were added.", statusCode: StatusCodes.Status200OK);
                }

                return new WarehouseTaskResult(succeeded: false, message: "0 items were added.", statusCode: StatusCodes.Status304NotModified);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> UpdateStockAsync(IWarehouseDTO warehouseDTO)
        {
            try
            {
                var itemToUpdate = warehouseDTO.Quantity > 0 ?
                    await UpdateWholeStockAsync(warehouseDTO)
                    : await UpdateNameOnlyAsync(warehouseDTO);

                if (itemToUpdate > 0)
                {
                    _logger.LogInformation($"[INFO] Item with ISBN: {warehouseDTO.ISBN} was updated.");
                    return new WarehouseTaskResult(succeeded: true, message: "Item was updated.", statusCode: StatusCodes.Status200OK);
                }
                return new WarehouseTaskResult(succeeded: false, message: "Item was not found.", statusCode: StatusCodes.Status404NotFound);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> RemoveStockAsync(IWarehouseDTO warehouseDTO)
        {
            try
            {
                var itemInDb = await _context.Warehouse.Where(w => w.ISBN.Equals(warehouseDTO.ISBN))
                    .FirstOrDefaultAsync();

                if (itemInDb!.Quantity < warehouseDTO.Quantity)
                {
                    return new WarehouseTaskResult(succeeded: false, message: "Not enough items in stock.", statusCode: StatusCodes.Status400BadRequest);
                }

                var itemToUpdate = await _context.Warehouse
                    .Where(w => w.ISBN.Equals(warehouseDTO.ISBN))
                    .ExecuteUpdateAsync(w => w.SetProperty(w => w.Quantity, w => w.Quantity - warehouseDTO.Quantity));
                if (itemToUpdate > 0)
                {
                    _logger.LogInformation($"[INFO] {warehouseDTO.Quantity} items were removed from the stock.");
                    return new WarehouseTaskResult(succeeded: true, message: $"{warehouseDTO.Quantity} items were removed.", statusCode: StatusCodes.Status200OK);
                }
                return new WarehouseTaskResult(succeeded: false, message: "0 items were removed.", statusCode: StatusCodes.Status304NotModified);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> RemoveStocksAsync(ICollection<IWarehouseDTO> warehouseDTOs)
        {
            try
            {
                var removedItems = 0;
                foreach (var dto in warehouseDTOs)
                {
                    var result = await RemoveStockAsync(dto);
                    removedItems = result.Succeeded ? removedItems + 1 : removedItems;
                }

                if(removedItems == warehouseDTOs.Count)
                {
                    _logger.LogInformation($"[INFO] {removedItems} items were removed from the stock.");
                    return new WarehouseTaskResult(succeeded: true, message: $"{removedItems} items were removed.", statusCode: StatusCodes.Status200OK);
                }
                
                return new WarehouseTaskResult(succeeded: false, message: "0 items were removed.", statusCode: StatusCodes.Status304NotModified);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new WarehouseTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }

        }
        private async Task<int> UpdateNameOnlyAsync(IWarehouseDTO warehouseDTO)
        {
            try
            {
                return await _context.Warehouse
                    .Where(w => w.ISBN.Equals(warehouseDTO.ISBN))
                    .ExecuteUpdateAsync(w => w.SetProperty(w => w.Name, warehouseDTO.Name));
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return 0;
            }
        }
        private async Task<int> UpdateWholeStockAsync(IWarehouseDTO warehouseDTO)
        {
            try
            {
                return await _context.Warehouse
                    .Where(w => w.ISBN.Equals(warehouseDTO.ISBN))
                    .ExecuteUpdateAsync(w => w
                    .SetProperty(w => w.Name, warehouseDTO.Name)
                    .SetProperty(w => w.Quantity, warehouseDTO.Quantity));
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return 0;
            }
        }
    }
}
