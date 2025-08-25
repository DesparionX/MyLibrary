using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Helpers;
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
    public class PerformOperationAsyncUnitTests : OperationHanderUnitTestBase
    {
        [TestCase(StockOperations.OperationType.Sell)]
        [TestCase(StockOperations.OperationType.Return)]
        [TestCase(StockOperations.OperationType.Borrow)]
        public async Task PerformOperationAsync_ReturnsSuccess_WhenOperationIsValidAndProcessedInWareHouse(StockOperations.OperationType operationType)
        {
            // Arrange
            var operation = NewOperation(operationType);
            SetupHandlersForSuccess();
            

            // Act
            var result = await _operationHandler.PerformOperationAsync(operation);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<OperationTaskResult>(r =>
                r.Succeeded
                && r.StatusCode == StatusCodes.Status200OK
                && r.Message == "Operation was added.");
        }

        [TestCase(StockOperations.OperationType.Sell)]
        [TestCase(StockOperations.OperationType.Return)]
        [TestCase(StockOperations.OperationType.Borrow)]
        public async Task PerformOperationAsync_ReturnsConflict_WhenOperationWithSameIdExists(StockOperations.OperationType operationType)
        {
            // Arrange
            var existingOperations = await AddFakeOperation(isRandom: false, type: operationType, count: 1);
            var operation = existingOperations.First();

            // Act
            var result = await _operationHandler.PerformOperationAsync(operation);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<OperationTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status409Conflict
                && r.Message == "Operation already exists.");
        }

        [TestCase(StockOperations.OperationType.Sell)]
        [TestCase(StockOperations.OperationType.Return)]
        [TestCase(StockOperations.OperationType.Borrow)]
        public async Task PerformOperationAsync_ReturnsInternalServerError_WhenProcessingInWarehouseFails(StockOperations.OperationType operationType)
        {
            // Arrange
            var operation = NewOperation(operationType);
            SetupWarehouseHandlersForFailure();

            // Act
            var result = await _operationHandler.PerformOperationAsync(operation);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<OperationTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status500InternalServerError
                && r.Message == "Operation failed while processing in Warehouse.");
        }

        [TestCase(StockOperations.OperationType.Sell)]
        [TestCase(StockOperations.OperationType.Return)]
        [TestCase(StockOperations.OperationType.Borrow)]
        public async Task PerformOperationAsync_ReturnsInternalServerError_WhenUnexpectedExceptionThrows(StockOperations.OperationType operationType)
        {
            // Arrange
            var operation = NewOperation(operationType);
            SetupWarehouseHandlersForFakeException();

            // Act
            var result = await _operationHandler.PerformOperationAsync(operation);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<OperationTaskResult>(r =>
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status500InternalServerError
                && r.Message == "Something went wrong !");
        }
    }
}
