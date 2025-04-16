using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Events;
using MyLibrary.Server.Handlers.Interfaces;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public class OperationsEventHandler : IOperationsEventHandler
    {
        private readonly EventBus _eventBus;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OperationsEventHandler> _logger;

        public OperationsEventHandler(EventBus eventBus, IServiceScopeFactory scopeFactory, ILogger<OperationsEventHandler> logger)
        {
            _eventBus = eventBus;
            _scopeFactory = scopeFactory;
            _logger = logger;

            _eventBus.Subscribe<ItemAddedEvent>(async e => await OnItemAdded(e));
            _eventBus.Subscribe<ItemUpdatedEvent>(async e => await OnItemUpdated(e));
            _eventBus.Subscribe<ItemRemovedEvent>(async e => await OnItemRemoved(e));
            _eventBus.Subscribe<ItemSoldEvent>(async e => await OnItemSold(e));
            _eventBus.Subscribe<ItemBorrowedEvent>(async e => await OnBookBorrowed(e));
            _eventBus.Subscribe<ItemReturnedEvent>(async e => await OnBookReturned(e));

        }
        public async Task OnItemAdded(IItemOperationEvent e)
        {
            using var scope = _scopeFactory.CreateScope();
            var warehouseHandler = scope.ServiceProvider.GetRequiredService<IWarehouseHandler<Warehouse>>();
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await warehouseHandler.AddStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
        public async Task OnItemUpdated(IItemOperationEvent e)
        {
            using var scope = _scopeFactory.CreateScope();
            var warehouseHandler = scope.ServiceProvider.GetRequiredService<IWarehouseHandler<Warehouse>>();
            try
            {
                _logger.LogInformation($"{e.Name}({e.ISBN}) send to warehouse handler.");
                await warehouseHandler.UpdateStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
        public async Task OnItemRemoved(IItemOperationEvent e)
        {
            using var scope = _scopeFactory.CreateScope();
            var warehouseHandler = scope.ServiceProvider.GetRequiredService<IWarehouseHandler<Warehouse>>();
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await warehouseHandler.RemoveStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
        public async Task OnItemSold(IItemOperationEvent e)
        {
            using var scope = _scopeFactory.CreateScope();
            var warehouseHandler = scope.ServiceProvider.GetRequiredService<IWarehouseHandler<Warehouse>>();
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await warehouseHandler.RemoveStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
        public async Task OnBookBorrowed(IItemOperationEvent e)
        {
            using var scope = _scopeFactory.CreateScope();
            var warehouseHandler = scope.ServiceProvider.GetRequiredService<IWarehouseHandler<Warehouse>>();
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await warehouseHandler.RemoveStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }
        public async Task OnBookReturned(IItemOperationEvent e)
        {
            using var scope = _scopeFactory.CreateScope();
            var warehouseHandler = scope.ServiceProvider.GetRequiredService<IWarehouseHandler<Warehouse>>();
            try
            {
                _logger.LogInformation($"{e.Quantity}x {e.Name}({e.ISBN}) send to warehouse handler.");
                await warehouseHandler.AddStockAsync(new WarehouseDTO { ISBN = e.ISBN, Name = e.Name, Quantity = e.Quantity });
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error sending {e.Quantity}x {e.Name}({e.ISBN}) to warehouse handler.");
            }
        }

    }
}
