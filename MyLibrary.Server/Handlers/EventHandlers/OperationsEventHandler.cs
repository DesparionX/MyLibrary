using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Events;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public class OperationsEventHandler : IOperationsEventHandler
    {
        private readonly IWarehouseHandler<Warehouse> _warehouseHandler;
        private readonly ILogger<OperationsEventHandler> _logger;

        public OperationsEventHandler(IWarehouseHandler<Warehouse> warehouseHandler, ILogger<OperationsEventHandler> logger)
        {
            _warehouseHandler = warehouseHandler;
            _logger = logger;

            EventBus.Subscribe<ItemAddedEvent>(OnBookAdded);
            
        }


        public void OnBookAdded(IItemOperationEvent e)
        {
            try
            {
                _warehouseHandler.AddStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) added to warehouse.");
            }
            catch(Exception err)
            {
                _logger.LogError(err, $"Error adding {e.Quantity}x {e.Name}({e.ISBN}) to warehouse.");
            }
        }

        public void OnBookBorrowed(IItemOperationEvent e)
        {
            throw new NotImplementedException();
        }

        public void OnBookDeleted(IItemOperationEvent e)
        {
            throw new NotImplementedException();
        }

        public void OnBookReturned(IItemOperationEvent e)
        {
            throw new NotImplementedException();
        }

        public void OnBookUpdated(IItemOperationEvent e)
        {
            throw new NotImplementedException();
        }
    }
}
