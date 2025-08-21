using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.BookHandlerUnitTests
{
    [TestFixture]
    public class AddBookAsyncUnitTests : BookHandlerUnitTestBase
    {
        [Test]
        public async Task AddBookAsync_ShouldReturnStatusCreated_WhenNewBooksAreAdded()
        {
            // Arrange
            var expectedQuantity = 5;
            var newBooks = FakeNewBooksDTO(quantity: expectedQuantity);

            // Act
            var result = await _bookHandler.AddBookAsync(newBooks);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r =>
                r.Succeeded
                && r.Message!.Equals($"{expectedQuantity} books added successfully.")
                && r.StatusCode == StatusCodes.Status201Created);
        }

        [Test]
        public async Task AddBookAsync_ReturnsConflict_WhenBookAlreadyExist()
        {
            // Arrange
            var duplicateBookId = Guid.NewGuid();
            var newBook = FakeNewBooksDTO(id: duplicateBookId, quantity: 1);
            await _bookHandler.AddBookAsync(newBook);


            // Act
            var result = await _bookHandler.AddBookAsync(newBook);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r =>
                !r.Succeeded
                && r.Message!.Equals("Book already exist in the database.")
                && r.StatusCode == StatusCodes.Status409Conflict);
        }
    }
}
