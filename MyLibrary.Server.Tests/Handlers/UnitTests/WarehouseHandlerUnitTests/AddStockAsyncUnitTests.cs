using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.WarehouseHandlerUnitTests
{
    [TestFixture]
    public class AddStockAsyncUnitTests : WarehouseHandlerUnitTestBase
    {
        [Test]
        public async Task AddStockAsync_IncreaseStockQuantity_WhenStockAlreadyExists()
        {
            // Arrange
            var existingStock = (await CreateFakeStocksAsync(stocksToBeAdded: 1)).First();
            var stockDTO = new WarehouseDTO
            {
                Id = existingStock.Id,
                ISBN = existingStock.ISBN,
                Name = existingStock.Name,
                Price = existingStock.Price,
                Quantity = 5
            };
            var expectedQuantity = stockDTO.Quantity;

            // Act
            var result = await _warehouseHandler.AddStockAsync(stockDTO);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status200OK
                && r.Message!.Equals($"{expectedQuantity} items were added."));
        }
        [Test]
        public async Task AddStockAsync_CreatesNewStock_WhenStockDoesntExist()
        {
            // Arrange
            var stockDTO = new WarehouseDTO
            {
                ISBN = "978-3-16-148410-0",
                Name = "New Book",
                Price = 15.99m,
                Quantity = 3
            };

            // Act
            var result = await _warehouseHandler.AddStockAsync(stockDTO);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status200OK
                && r.Message!.Equals("Stock created successfully."));
        }
    }
}
