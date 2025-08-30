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
    public class AddStocksAsyncUnitTests : WarehouseHandlerUnitTestBase
    {
        [Test]
        public async Task AddStocksAsync_CreatesOrUpdatesMultipleStocks()
        {
            // Arrange
            var expectedCount = 3;
            var newStocks = NewStocks(stocksToBeAdded: expectedCount);

            // Act
            var result = await _warehouseHandler.AddStocksAsync(newStocks);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<WarehouseTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status200OK
                && r.Message!.Equals($"{expectedCount} items were added."));
        }
    }
}
