using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.BookHandlerUnitTests
{
    [TestFixture]
    public class UpdateBookAsyncUnitTests : BookHandlerUnitTestBase
    {
        [Test]
        public async Task UpdateBookAsync_ShouldUpdateBook_WhenBookExists()
        {
            // Arrange
            var bookISBN = "978-3-16-148410-0";
            var initalCount = 5;
            await AddFakeBooks(isbn: bookISBN, booksCount: initalCount);
            var expectedCount = await GetFilteredBooksCountAsync(propertyName: nameof(BookDTO.ISBN), bookISBN);

            var bookToUpdate = (await _bookHandler.FindBookByISBN(bookISBN)).As<BookTaskResult>().Book!;
            bookToUpdate.Title = "Updated Test Book";

            // Act
            var result = await _bookHandler.UpdateBookAsync(bookToUpdate);

            // Assert
            initalCount.Should().Be(expectedCount);
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r =>
                r.Succeeded
                && r.Message!.Equals($"{expectedCount} books updated successfully.")
                && r.StatusCode == StatusCodes.Status200OK);

        }
        [Test]
        public async Task UpdateBookAsync_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var bookToUpdate = new BookDTO
            {
                Id = Guid.NewGuid(),
                ISBN = "978-3-16-3333333-0",
                Title = "Non-existent Book",
                Author = "Unknown Author",
                Description = "This book does not exist.",
                Publisher = "Unknown Publisher",
                Genre = "Fiction"
            };

            // Act
            var result = await _bookHandler.UpdateBookAsync(bookToUpdate);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r =>
                !r.Succeeded
                && r.Message!.Equals("No books found with given ISBN.")
                && r.StatusCode == StatusCodes.Status404NotFound);
        }
        [Test]
        public async Task UpdateBookAvailabilityAsync_ShouldReturnOK_WhenBooksUpdateSuccessfully()
        {
            // Arrange
            var ids = await AddFakeBooks(booksCount: 5);

            // Act
            var result = await _bookHandler.UpdateBookAvailabilityAsync(ids, isAvailable: false);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r =>
                r.Succeeded
                && r.Message!.Equals($"{ids.Count} books availability updated successfully.")
                && r.StatusCode == StatusCodes.Status200OK);
        }
        [Test]
        public async Task UpdateBookAvailabilityAsync_ShouldReturnBadRequest_WhenBooksIdListIsEmpty()
        {
            // Arrange
            var ids = new List<string>();

            // Act
            var result = await _bookHandler.UpdateBookAvailabilityAsync(ids, isAvailable: false);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r =>
                !r.Succeeded
                && r.Message!.Equals("No books were updated.")
                && r.StatusCode == StatusCodes.Status400BadRequest);
        }
    }
}
