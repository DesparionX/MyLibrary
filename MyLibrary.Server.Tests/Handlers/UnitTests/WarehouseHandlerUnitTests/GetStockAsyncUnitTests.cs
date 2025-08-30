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
    public class GetStockAsyncUnitTests : WarehouseHandlerUnitTestBase
    {
        [Test]
        public async Task GetStockAsync_ShouldReturnStockDTO_WhenStockWithGivenISBNExists()
        {
            // Arrange
            var isbnToSearch = "978-3-16-148410-1";
            var expectedStockDTOs = await CreateFakeStocksAsync(isbn: isbnToSearch, stocksToBeAdded: 1);
            
            // Act
            var result = await _warehouseHandler.GetStockAsync(isbnToSearch);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status302Found
                && r.Message!.Equals("Item found.")
                && r.StockDTO!.Equals(expectedStockDTOs.First()));
        }

        [Test]
        public async Task GetStockAsync_ShouldReturnNotFound_WhenStockWithGivenISBNDoesNotExist()
        {
            // Arrange
            var isbnToSearch = "978-3-16-148410-999";

            // Act
            var result = await _warehouseHandler.GetStockAsync(isbnToSearch);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status404NotFound
                && r.Message!.Equals("Stock not found.")
                && r.StockDTO == null);
        }
    }
}
