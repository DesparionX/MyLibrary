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
    public class DeleteStockAsyncUnitTests : WarehouseHandlerUnitTestBase
    {
        [Test]
        public async Task DeleteStockAsync_SuccessfullyDeletesAStock_IfItExists()
        {
            // Arrange
            var existingStock = (await CreateFakeStocksAsync(isbn: "1231-433-322-3", stocksToBeAdded: 1)).First();

            // Act
            var result = await _warehouseHandler.DeleteStockAsync(existingStock.Id);
            var expectedMissingStock = await _warehouseHandler.GetStockAsync(existingStock.ISBN);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status200OK
                && r.Message!.Equals("Stock deleted successfully."));

            // Making sure the stock is actually deleted
            expectedMissingStock.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status404NotFound
                && r.Message!.Equals("Stock not found."));
        }

        [Test]
        public async Task DeleteStockAsync_ReturnsNotFound_IfStockDoesNotExist()
        {
            // Arrange
            var nonExistentStockId = 999;

            // Act
            var result = await _warehouseHandler.DeleteStockAsync(nonExistentStockId);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status404NotFound
                && r.Message!.Equals("Stock not found."));
        }
    }
}
