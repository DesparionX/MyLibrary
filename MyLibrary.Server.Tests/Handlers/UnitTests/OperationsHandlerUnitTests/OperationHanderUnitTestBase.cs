using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Extensions;
using Moq;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Helpers;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.OperationsHandlerUnitTests
{
    public abstract class OperationHanderUnitTestBase : UnitTestBase
    {
        protected IOperationHandler _operationHandler;

        [SetUp]
        public void SetupOperationHandler()
        {
            _operationHandler = new OperationHandler(
                new Mock<ILogger<OperationHandler>>().Object,
                Mapper,
                BookHandler.Object,
                WarehouseHandler.Object,
                DbContext
            );
        }

        protected async Task<ICollection<OperationDTO>> AddFakeOperation(bool isRandom, StockOperations.OperationType? type = default, int count = 1)
        {
            var operations = new List<Operation>();
            for (int i = 1; i <= count; i++)
            {
                var fakeOrders = new List<Order>
                    {
                        new Order
                        {
                            ItemId = Guid.NewGuid().ToString(),
                            ItemISBN = DateTime.Now.ToString(),
                            ItemName = $"Test Item {i}",
                            Price = 9.99m,
                            Quantity = 1
                        },
                        new Order
                        {
                            ItemId = Guid.NewGuid().ToString(),
                            ItemISBN = DateTime.Now.ToString(),
                            ItemName = $"Test Item {i}",
                            Price = 9.99m,
                            Quantity = 1
                        }
                    };
                var fakeOperation = new Operation
                {
                    OperationDate = DateTime.Now,
                    OperationName = DetermineOperationName(isRandom, type),
                    UserId = Guid.NewGuid().ToString(),
                    OrderListInternal = fakeOrders,
                    UserName = $"Test User {i}",
                    UserRole = "User",
                    TotalPrice = fakeOrders.Sum(o => o.Price * o.Quantity)
                };

                DbContext.Add(fakeOperation);
                operations.Add(fakeOperation);
            }
            await DbContext.SaveChangesAsync();
            return Mapper.Map<ICollection<OperationDTO>>(operations)!;
        }
     
        protected IOperationDTO NewOperation(StockOperations.OperationType type)
        {
            return new OperationDTO
            {
                OperationDate = DateTime.Now,
                OperationName = type.GetDisplayName(),
                UserId = Guid.NewGuid().ToString(),
                OrderList = new List<IOrder>
                {
                    new Order
                    {
                        ItemId = Guid.NewGuid().ToString(),
                        ItemISBN = "4234-234234-12122-33",
                        ItemName = "Test Item",
                        Price = 9.99m,
                        Quantity = 1
                    },
                    new Order
                    {
                        ItemId = Guid.NewGuid().ToString(),
                        ItemISBN = "4234-234234-12122-33323",
                        ItemName = "Test Item 2",
                        Price = 9.99m,
                        Quantity = 1
                    }
                },
                UserName = "Test User",
                UserRole = "User",
                TotalPrice = 19.98m
            };
        }
        protected void SetupHandlersForSuccess()
        {
            WarehouseHandler.Setup(w => w.RemoveStockAsync(It.IsAny<IWarehouseDTO>()))
                .ReturnsAsync(new WarehouseTaskResult(succeeded: true, message: "Stock removed.", statusCode: StatusCodes.Status200OK));
            WarehouseHandler.Setup(w => w.RemoveStocksAsync(It.IsAny<ICollection<IWarehouseDTO>>()))
                .ReturnsAsync(new WarehouseTaskResult(succeeded: true, message: "Stock removed.", statusCode: StatusCodes.Status200OK));
            WarehouseHandler.Setup(w => w.AddStocksAsync(It.IsAny<ICollection<IWarehouseDTO>>()))
                .ReturnsAsync(new WarehouseTaskResult(succeeded: true, message: "Stocks added successfully.", statusCode: StatusCodes.Status200OK));
            WarehouseHandler.Setup(w => w.AddStockAsync(It.IsAny<IWarehouseDTO>()))
                .ReturnsAsync(new WarehouseTaskResult(succeeded: true, message: "Stock added successfully.", statusCode: StatusCodes.Status200OK));

            BookHandler.Setup(b => b.UpdateBookAvailabilityAsync(It.IsAny<ICollection<string>>(), It.IsAny<bool>()))
                .ReturnsAsync(new BookTaskResult(succeeded: true, message: "Book availability updated.", statusCode: StatusCodes.Status200OK));
        }

        protected void SetupWarehouseHandlersForFailure()
        {
            WarehouseHandler.Setup(w => w.RemoveStockAsync(It.IsAny<IWarehouseDTO>()))
                .ReturnsAsync(new WarehouseTaskResult(succeeded: false, message: "Stock removal failed.", statusCode: StatusCodes.Status200OK));
            WarehouseHandler.Setup(w => w.RemoveStocksAsync(It.IsAny<ICollection<IWarehouseDTO>>()))
                .ReturnsAsync(new WarehouseTaskResult(succeeded: false, message: "Stocks removal failed.", statusCode: StatusCodes.Status200OK));
            WarehouseHandler.Setup(w => w.AddStocksAsync(It.IsAny<ICollection<IWarehouseDTO>>()))
                .ReturnsAsync(new WarehouseTaskResult(succeeded: false, message: "Failed to add stocks.", statusCode: StatusCodes.Status200OK));
            WarehouseHandler.Setup(w => w.AddStockAsync(It.IsAny<IWarehouseDTO>()))
                .ReturnsAsync(new WarehouseTaskResult(succeeded: false, message: "Failed to add stock.", statusCode: StatusCodes.Status200OK));
        }

        protected void SetupWarehouseHandlersForFakeException()
        {
            WarehouseHandler.Setup(w => w.RemoveStockAsync(It.IsAny<IWarehouseDTO>()))
                .ThrowsAsync(new Exception("Failed to remove stock."));
            WarehouseHandler.Setup(w => w.RemoveStocksAsync(It.IsAny<ICollection<IWarehouseDTO>>()))
                .ThrowsAsync(new Exception("Failed to remove stocks."));
            WarehouseHandler.Setup(w => w.AddStocksAsync(It.IsAny<ICollection<IWarehouseDTO>>()))
                .ThrowsAsync(new Exception("Failed to add stocks."));
            WarehouseHandler.Setup(w => w.AddStockAsync(It.IsAny<IWarehouseDTO>()))
                .ThrowsAsync(new Exception("Failed to add stock."));
        }

        private string DetermineOperationName(bool isRandom, StockOperations.OperationType? type)
        {
            if (isRandom)
            {
                var stockOperations = Enum.GetValues(typeof(StockOperations.OperationType));
                return stockOperations.GetValue(new Random().Next(stockOperations.Length))!.ToString()!;
            }
            return nameof(type);
        }
    }
}
