using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Shared.Interfaces.IDTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Events;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Helpers;
using MyLibrary.Server.Http.Responses;
using NJsonSchema.CodeGeneration.Models;
using Sprache;

namespace MyLibrary.Server.Handlers
{
    public class OperationHandler : IOperationHandler
    {
        private readonly EventBus _eventBus;
        private readonly ILogger<OperationHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IBookHandler<Book> _bookHandler;
        private readonly IWarehouseHandler<Warehouse> _warehouseHandler;
        private readonly AppDbContext _context;

        public OperationHandler(
            EventBus eventBus,
            ILogger<OperationHandler> logger,
            IMapper mapper,
            IBookHandler<Book> bookHandler,
            IWarehouseHandler<Warehouse> warehouseHandler,
            AppDbContext context)
        {
            _eventBus = eventBus;
            _logger = logger;
            _mapper = mapper;
            _bookHandler = bookHandler;
            _warehouseHandler = warehouseHandler;
            _context = context;
        }
        public async Task<ITaskResult> GetOperationHistoryAsync()
        {
            try
            {
                var operations = await _context.Operations.ToListAsync();
                if (operations.Count == 0)
                {
                    _logger.LogWarning("[WARNING] No operations found.");
                    return new OperationTaskResult(succeeded: false, message: "No operations found.", statusCode: StatusCodes.Status404NotFound);
                }
                var dtos = _mapper.Map<ICollection<OperationDTO>>(operations);
                _logger.LogInformation($"[INFO] Found {operations.Count} operations.");
                return new OperationTaskResult(succeeded: true, message: $"Found {operations.Count} operations.", statusCode: StatusCodes.Status200OK, operationDtos: dtos);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new OperationTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> PerformOperationAsync(IOperationDTO operationDTO)
        {
            try
            {
                // Check for identity conflict.
                if (await _context.Operations.AnyAsync(o => o.Id.Equals(operationDTO.Id)))
                {
                    return new OperationTaskResult(succeeded: false, message: "Operation already exists.", statusCode: StatusCodes.Status409Conflict);
                }

                // First process the operation in the warehouse before storing it.
                if(!await ProcessedInWarehouseAsync(operationDTO))
                {
                    _logger.LogWarning($"[WARNING] Operation with ID: {operationDTO.Id} failed to process in Warehouse.");
                    return new OperationTaskResult(succeeded: false, message: "Operation failed while processing in Warehouse.", statusCode: StatusCodes.Status400BadRequest);
                }

                // Map the DTO to the entity and add it to the context.
                var newOperation = _mapper.Map<Operation>(operationDTO);
                await _context.Operations.AddAsync(newOperation);

                // If successfully added to the context, return positive task result.
                if (await _context.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation($"[INFO] Operation with ID: {operationDTO.Id} was created.");
                    return new OperationTaskResult(succeeded: true, message: "Operation was added.", statusCode: StatusCodes.Status200OK);
                }
                return new OperationTaskResult(succeeded: false, message: "Operation was not added.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch(Exception err)
            {
                _logger.LogError(err, $"[ERROR]\n{err.Message}");
                return new OperationTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // I realized that stock should be processed in the warehouse first before saving the operation,
        // therefore publishing to event bus is no longer needed.
        /* private void SendToEventBus(IOperationDTO operationDTO)
        {
            switch (operationDTO.OperationName)
            {
                case nameof(StockOperations.OperationType.Sell):
                    _eventBus.Publish(new ItemSoldEvent(isbn: operationDTO.ArticleISBN, name: operationDTO.ArticleName, quantity: (int)operationDTO.Quantity));
                    break;
                case nameof(StockOperations.OperationType.Borrow):
                    _eventBus.Publish(new ItemBorrowedEvent(isbn: operationDTO.ArticleISBN, name: operationDTO.ArticleName, quantity: (int)operationDTO.Quantity));
                    break;
                case nameof(StockOperations.OperationType.Return):
                    _eventBus.Publish(new ItemReturnedEvent(isbn: operationDTO.ArticleISBN, name: operationDTO.ArticleName, quantity: (int)operationDTO.Quantity));
                    break;
                default:
                    _logger.LogWarning($"[WARNING] Operation type: {operationDTO.OperationName} is not supported.");
                    break;
            }
        }
        */

        private async Task<bool> ProcessedInWarehouseAsync(IOperationDTO operationDTO)
        {
            if (operationDTO.OrderList == null || operationDTO.OrderList.Count == 0)
            {
                return false;
            }

            switch (operationDTO.OperationName)
            {
                case nameof(StockOperations.OperationType.Sell) or nameof(StockOperations.OperationType.Borrow):

                    var itemsToRemove = new List<IWarehouseDTO>();
                    var bookIds = new List<string>();

                    foreach (var item in operationDTO.OrderList!)
                    {
                        itemsToRemove.Add(new WarehouseDTO
                        {
                            ISBN = item.ItemISBN!,
                            Name = item.ItemName!,
                            Price = (decimal)item.Price!,
                            Quantity = item.Quantity
                        });
                        bookIds.Add(item.ItemId!);
                    }

                    // If warehouse handler fails to remove the stocks, return false.
                    // Else, update the book availability and return result status.
                    var warehouseResult = await _warehouseHandler.RemoveStocksAsync(itemsToRemove);
                    if (!warehouseResult.Succeeded)
                    {
                        _logger.LogWarning($"[WARNING] Operation with ID: {operationDTO.Id} failed to remove stocks.");
                        return false;
                    }
                    else
                    {
                        var bookResult = await _bookHandler.UpdateBookAvailabilityAsync(ids: bookIds, isAvailable: false);
                        return bookResult.Succeeded;

                        //TODO: CREATE NEW BORROW
                    }

                case nameof(StockOperations.OperationType.Return):

                    var itemsToAdd = new List<IWarehouseDTO>();
                    bookIds = new List<string>();

                    foreach (var item in operationDTO.OrderList!)
                    {
                        itemsToAdd.Add(new WarehouseDTO
                        {
                            ISBN = item.ItemISBN!,
                            Name = item.ItemName!,
                            Price = (decimal)item.Price!,
                            Quantity = item.Quantity
                        });
                        bookIds.Add(item.ItemId!);
                    }

                    // If warehouse handler fails to add the stocks, return false.
                    // Else, update the book availability and return result status.
                    warehouseResult = await _warehouseHandler.RemoveStocksAsync(itemsToAdd);
                    if (!warehouseResult.Succeeded)
                    {
                        _logger.LogWarning($"[WARNING] Operation with ID: {operationDTO.Id} failed to remove stocks.");
                        return false;
                    }
                    else
                    {
                        var bookResult = await _bookHandler.UpdateBookAvailabilityAsync(ids: bookIds, isAvailable: true);
                        return bookResult.Succeeded;
                    }

                default:
                    _logger.LogWarning($"[WARNING] Operation type: {operationDTO.OperationName} is not supported.");
                    return false;
            }
        }
    }
}
