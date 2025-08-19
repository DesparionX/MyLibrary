using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
    public class FindBookByISBNUnitTests : BookHandlerUnitTestBase
    {
        [Test]
        public async Task FindBookByISBN_ShouldReturnBook_WhenISBNExists()
        {
            // Arrange
            var isbn = "978-3-16-148410-0";
            await AddFakeBook(isbn, default);

            // Act
            var result = await _bookHandler.FindBookByISBN(isbn);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r => 
                r.Succeeded
                && r.StatusCode == StatusCodes.Status302Found
                && r.Message!.Equals("Book found !")
                && r.Book != null 
                && r.Book.ISBN == isbn);
        }

        [Test]
        public async Task FindBookByISBN_ShouldReturnNotFound_WhenISBNDoesNotExist()
        {
            // Arrange
            var isbn = "978-3-16-148410-0";

            // Act
            var result = await _bookHandler.FindBookByISBN(isbn);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<BookTaskResult>(r => 
                !r.Succeeded
                && r.StatusCode == StatusCodes.Status404NotFound
                && r.Message!.Equals("There is no books with given ISBN.")
                && r.Book == null);
        }
    }
}
