using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.OperationsHandlerUnitTests
{
    [TestFixture]
    public class GetOperationHistoryAsyncUnitTests : OperationHanderUnitTestBase
    {
        [Test]
        public async Task GetOperationHistoryAsync_ReturnsAllOperations_WhenThereAreOperationsInTheDatabase()
        {
            // Arrange
            var expectedCount = 3;
            var operations = await AddFakeOperation(isRandom: true, count: expectedCount);

            // Act
            var result = await _operationHandler.GetOperationHistoryAsync();

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<OperationTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status302Found
                && r.Message!.Equals($"Found {expectedCount} operations.")
                && r.OperationDTOs != null);
        }
        [Test]
        public async Task GetOperationHistoryAsync_ReturnsNotFound_WhenThereAreNoOperationsInTheDatabase()
        {
            // Arrange
            ICollection<IOperationDTO>? expetedOperationsList = null;

            // Act
            var result = await _operationHandler.GetOperationHistoryAsync();

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<OperationTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status404NotFound
                && r.Message!.Equals("No operations found.")
                && r.OperationDTOs == expetedOperationsList);
        }
    }
}
