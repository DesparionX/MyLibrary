using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Http.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
