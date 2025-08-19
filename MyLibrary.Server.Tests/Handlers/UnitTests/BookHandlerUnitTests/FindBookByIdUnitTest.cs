using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MyLibrary.Server.Data;
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
    public class FindBookByIdUnitTest : BookHandlerUnitTestBase
    {
        [Test]
        public async Task FindBookById_ShouldReturnBookDTO_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            await AddFakeBook(default, bookId);

            // Act
            var result = await _bookHandler.FindBookById(bookId);

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
            var nonExistentBookId = Guid.NewGuid();

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
