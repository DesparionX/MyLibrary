using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Events;

namespace MyLibrary.Server.Handlers.EventHandlers
{
    public class OperationsEventHandler : IOperationsEventHandler
    {
        private readonly EventBus _eventBus;
        private readonly IWarehouseHandler<Warehouse> _warehouseHandler;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OperationsEventHandler> _logger;

        public OperationsEventHandler(EventBus eventBus, IWarehouseHandler<Warehouse> warehouseHandler, IServiceScopeFactory scopeFactory, ILogger<OperationsEventHandler> logger)
        {
            _eventBus = eventBus;
            _warehouseHandler = warehouseHandler;
            _scopeFactory = scopeFactory;
            _logger = logger;

            _eventBus.Subscribe<ItemAddedEvent>(async e => await OnItemAdded(e));
            _eventBus.Subscribe<ItemRemovedEvent>(OnItemRemoved);
            _eventBus.Subscribe<ItemSoldEvent>(OnItemSold);
            _eventBus.Subscribe<ItemBorrowedEvent>(OnBookBorrowed);
            _eventBus.Subscribe<ItemReturnedEvent>(OnBookReturned);

        }
        public async Task OnItemAdded(ItemAddedEvent e)
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

        private async Task HandleAddedItemAsync(IItemOperationEvent e)
        {
            
        }
    }
}
