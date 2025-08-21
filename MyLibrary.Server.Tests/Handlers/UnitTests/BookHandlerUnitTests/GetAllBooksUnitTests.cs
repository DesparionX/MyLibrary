using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.BookHandlerUnitTests
{
    [TestFixture]
    public class GetAllBooksUnitTests : BookHandlerUnitTestBase
    {
        [Test]
        public async Task GetAllBooks_ShouldReturnAllBooks()
        {
            // Arrange
            var expectedCount = 5;
            await AddFakeBooks(booksCount: expectedCount);

            // Act
            var result = await _bookHandler.GetAllBooks();

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r => 
                    r.Succeeded 
                    && r.Message!.Equals($"{expectedCount} Books found !")
                    && r.StatusCode == StatusCodes.Status302Found
                    && r.Books!.Count == expectedCount
                );
        }

        [Test]
        public async Task GetAllBooks_ShouldReturnNotFound_WhenNoBooksExist()
        {
            // Arrange
            var expectedCount = 0;

            // Act
            var result = await _bookHandler.GetAllBooks();

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r => 
                    !r.Succeeded 
                    && r.Message!.Equals("No books found.")
                    && r.StatusCode == StatusCodes.Status404NotFound
                    && (r.Books == null || r.Books.Count == expectedCount)
                );
        }
    }
}
