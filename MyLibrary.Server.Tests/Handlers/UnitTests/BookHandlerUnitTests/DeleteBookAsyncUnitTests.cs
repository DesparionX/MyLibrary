using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Http.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.BookHandlerUnitTests
{
    [TestFixture]
    public class DeleteBookAsyncUnitTests : BookHandlerUnitTestBase
    {
        [Test]
        public async Task DeleteBookAsync_ShouldDeleteBook_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            await AddFakeBooks(id: bookId);

            // Act
            var result = await _bookHandler.DeleteBookAsync(bookId);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r =>
                r.Succeeded
                && r.Message!.Equals("Book removed from the database successfully.")
                && r.StatusCode == StatusCodes.Status200OK);
        }
        [Test]
        public async Task DeleteBookAsync_ShouldReturnNotFound_WhenBookDoesntExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            // Act
            var result = await _bookHandler.DeleteBookAsync(bookId);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r =>
                !r.Succeeded
                && r.Message!.Equals("No book found with given ID.")
                && r.StatusCode == StatusCodes.Status404NotFound);
        }
    }
}
