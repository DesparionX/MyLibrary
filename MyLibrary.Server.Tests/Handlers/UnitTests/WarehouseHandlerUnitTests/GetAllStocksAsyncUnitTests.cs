using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.WarehouseHandlerUnitTests
{
    [TestFixture]
    public class GetAllStocksAsyncUnitTests : WarehouseHandlerUnitTestBase
    {
        [Test]
        public async Task GetAllStocksAsync_ShouldReturnAllStocksInTheWarehouse_IfAnyExists()
        {
            // Arrange
            var expectedStockDTOs = await CreateFakeStocksAsync(stocksToBeAdded: 5);

            // Act
            var result = await _warehouseHandler.GetAllStocksAsync();

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status302Found
                && r.Message!.Equals($"{expectedStockDTOs.Count} items found.")
                && r.StockDTOs!.SequenceEqual(expectedStockDTOs));
        }

        [Test]
        public async Task GetAllStocksAsync_ShouldReturnNotFound_IfNoStocksExistInTheWarehouse()
        {
            // Arrange
            ICollection<IWarehouseDTO>? expectedList = default;

            // Act
            var result = await _warehouseHandler.GetAllStocksAsync();

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status404NotFound
                && r.Message!.Equals("No items found.")
                && r.StockDTOs == expectedList);
        }
    }
}
