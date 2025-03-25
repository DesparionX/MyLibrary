using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Events;
using MyLibrary.Server.Helpers;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public class OperationHandler : IOperationHandler
    {
        private readonly ILogger<OperationHandler> _logger;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public OperationHandler(ILogger<OperationHandler> logger, IMapper mapper, AppDbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ITaskResult> PerformOperationAsync(IOperationDTO operationDTO)
        {
            try
            {
                if (await _context.Operations.AnyAsync(o => o.Id.Equals(operationDTO.Id)))
                {
                    return new OperationTaskResult(succeeded: false, message: "Operation already exists.", statusCode: StatusCodes.Status409Conflict);
                }
                var newOperation = _mapper.Map<Operation>(operationDTO);
                await _context.Operations.AddAsync(newOperation);

                if (await _context.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation($"[INFO] Operation with ID: {operationDTO.Id} was created.");
                    SendToEventBus(operationDTO);

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

        private void SendToEventBus(IOperationDTO operationDTO)
        {
            switch (operationDTO.OperationName)
            {
                case nameof(StockOperations.OperationType.Sell):
                    EventBus.Publish(new ItemSoldEvent(isbn: operationDTO.ArticleISBN, name: operationDTO.ArticleName, quantity: (int)operationDTO.Quantity));
                    break;
                case nameof(StockOperations.OperationType.Borrow):
                    EventBus.Publish(new ItemBorrowedEvent(isbn: operationDTO.ArticleISBN, name: operationDTO.ArticleName, quantity: (int)operationDTO.Quantity));
                    break;
                case nameof(StockOperations.OperationType.Return):
                    EventBus.Publish(new ItemReturnedEvent(isbn: operationDTO.ArticleISBN, name: operationDTO.ArticleName, quantity: (int)operationDTO.Quantity));
                    break;
                default:
                    _logger.LogWarning($"[WARNING] Operation type: {operationDTO.OperationName} is not supported.");
                    break;
            }
        }
    }
}
