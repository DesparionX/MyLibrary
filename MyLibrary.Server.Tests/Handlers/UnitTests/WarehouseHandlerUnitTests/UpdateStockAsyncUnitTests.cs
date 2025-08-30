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
    public class UpdateStockAsyncUnitTests : WarehouseHandlerUnitTestBase
    {
        [Test]
        public async Task UpdateStockAsync_SuccessfullyUpdatesStock()
        {
            // Arrange
            var fakeStocks = await CreateFakeStocksAsync(stocksToBeAdded: 3);
            var stockToUpdate = fakeStocks.First();
            stockToUpdate.Quantity += 5;
            stockToUpdate.Name = "Updated Book Name";

            // Act
            var result = await _warehouseHandler.UpdateStockAsync(stockToUpdate);
            var getStockResult = await _warehouseHandler.GetStockAsync(stockToUpdate.ISBN);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status200OK
                && r.Message!.Equals("Item updated successfully."));

            // Check if the stock was updated correctly
            getStockResult.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status302Found
                && r.Message!.Equals("Item found.")
                && r.StockDTO!.Equals(stockToUpdate));
        }
    }
}
