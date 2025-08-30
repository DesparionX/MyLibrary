using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.WarehouseHandlerUnitTests
{
    [TestFixture]
    public class CreateStockAsyncUnitTests : WarehouseHandlerUnitTestBase
    {
        [Test]
        public async Task CreateStockAsync_ReturnsOK_WhenStockSucessfullyCreated()
        {
            // Arrange
            var stockDTO = new WarehouseDTO
            {
                ISBN = "978-3-16-148410-0",
                Name = "New Sample Book",
                Price = 15.99m,
                Quantity = 10
            };

            // Act
            var result = await _warehouseHandler.CreateStockAsync(stockDTO);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status200OK
                && r.Message!.Equals("Stock created successfully."));
        }

        [Test]
        public async Task CreateStockAsync_ReturnsConflict_WhenStockWithSameISBNExists()
        {
            // Arrange
            var existingStock = (await CreateFakeStocksAsync(isbn: "978-3-16-148410-0")).First();
            var stockDTO = new WarehouseDTO
            {
                ISBN = existingStock.ISBN,
                Name = "Another Sample Book",
                Price = 12.99m,
                Quantity = 5
            };

            // Act
            var result = await _warehouseHandler.CreateStockAsync(stockDTO);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status409Conflict
                && r.Message!.Equals("Item with the same ISBN already exists."));
        }
    }
}
