using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Http.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.WarehouseHandlerUnitTests
{
    [TestFixture]
    public class RemoveStockAsyncUnitTests : WarehouseHandlerUnitTestBase
    {
        [Test]
        public async Task RemoveStockAsync_SuccessfullyRemovesStock_WhenThereIsEnoughQuantity()
        {
            // Arrange
            var initialQuantity = 5;
            var fakeStock = (await CreateFakeStocksAsync(stocksToBeAdded: 1, quantity: initialQuantity)).First();
            var quantityToRemove = 3;
            fakeStock.Quantity = quantityToRemove;

            // Act
            var result = await _warehouseHandler.RemoveStockAsync(fakeStock);
            var getStockResult = await _warehouseHandler.GetStockAsync(fakeStock.ISBN);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status200OK
                && r.Message!.Equals($"{quantityToRemove} items were removed."));

            // Verify the stock quantity is updated correctly
            getStockResult.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status302Found
                && r.Message!.Equals("Item found.")
                && r.StockDTO!.Quantity == initialQuantity - quantityToRemove);
        }

        [Test]
        public async Task RemoveStockAsync_FailsToRemoveStock_WhenThereIsNotEnoughQuantity()
        {
            // Arrange
            var initialQuantity = 2;
            var fakeStock = (await CreateFakeStocksAsync(stocksToBeAdded: 1, quantity: initialQuantity)).First();
            var quantityToRemove = 5; // More than available
            fakeStock.Quantity = quantityToRemove;

            // Act
            var result = await _warehouseHandler.RemoveStockAsync(fakeStock);
            var getStockResult = await _warehouseHandler.GetStockAsync(fakeStock.ISBN);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status400BadRequest
                && r.Message!.Equals("Not enough items in stock."));

            // Verify the stock quantity remains unchanged
            getStockResult.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status302Found
                && r.Message!.Equals("Item found.")
                && r.StockDTO!.Quantity == initialQuantity);
        }
    }
}
