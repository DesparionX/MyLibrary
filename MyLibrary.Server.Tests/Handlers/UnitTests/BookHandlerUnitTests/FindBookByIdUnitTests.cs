using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.BookHandlerUnitTests
{
    [TestFixture]
    public class FindBookByIdUnitTests : BookHandlerUnitTestBase
    {
        [Test]
        public async Task FindBookById_ShouldReturnBookDTO_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            await AddFakeBooks(id: bookId);

            // Act
            var result = await _bookHandler.FindBookById(bookId.ToString());

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r => 
                r.Succeeded
                && r.StatusCode == StatusCodes.Status302Found
                && r.Message!.Equals("Book found !")
                && r.Book!.Id.Equals(bookId));
        }

        [Test]
        public async Task FindBookById_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var nonExistentBookId = Guid.NewGuid().ToString();

            // Act
            var result = await _bookHandler.FindBookById(nonExistentBookId);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r => 
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status404NotFound
                && r.Message!.Equals("No book found with given ID.")
                && r.Book == null);
        }
    }
}
