using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Events;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public class OperationsEventHandler : IOperationsEventHandler, IItemEvents
    {
        private readonly IWarehouseHandler<Warehouse> _warehouseHandler;
        private readonly ILogger<OperationsEventHandler> _logger;

        public OperationsEventHandler(IWarehouseHandler<Warehouse> warehouseHandler, ILogger<OperationsEventHandler> logger)
        {
            _warehouseHandler = warehouseHandler;
            _logger = logger;

            EventBus.Subscribe<ItemAddedEvent>(OnItemAdded);
            EventBus.Subscribe<ItemRemovedEvent>(OnItemRemoved);
            EventBus.Subscribe<ItemSoldEvent>(OnItemSold);
            EventBus.Subscribe<ItemBorrowedEvent>(OnBookBorrowed);
            EventBus.Subscribe<ItemReturnedEvent>(OnBookReturned);

        }
        public async void OnItemAdded(IItemOperationEvent e)
        {
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await _warehouseHandler.AddStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
        public async void OnItemRemoved(IItemOperationEvent e)
        {
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await _warehouseHandler.RemoveStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
        public async void OnItemSold(IItemOperationEvent e)
        {
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await _warehouseHandler.RemoveStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
        public async void OnBookBorrowed(IItemOperationEvent e)
        {
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await _warehouseHandler.RemoveStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
        public async void OnBookReturned(IItemOperationEvent e)
        {
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await _warehouseHandler.AddStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
    }
}
