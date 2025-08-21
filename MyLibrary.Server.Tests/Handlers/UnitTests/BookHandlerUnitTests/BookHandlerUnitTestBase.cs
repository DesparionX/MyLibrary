using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.BookHandlerUnitTests
{
    public abstract class BookHandlerUnitTestBase : UnitTestBase
    {
        protected IBookHandler<IBook<Guid>> _bookHandler;

        [SetUp]
        public void SetupBookHandler()
        {
            _bookHandler = new BookHandler(
                EventBus.Object,
                DbContext,
                Mapper,
                new Mock<ILogger<BookHandler>>().Object
            );
        }

        protected async Task AddFakeBooks(string? isbn = default, Guid? id = default, int? booksCount = 1)
        {
            for (int i = 1; i <= booksCount; i++)
            {
                DbContext.Add(new Book
                {
                    Id = id ?? Guid.NewGuid(),
                    ISBN = isbn ?? DateTime.Now.ToString(),
                    Title = $"Test Book {i}",
                    Author = $"Test Author {i}",
                    IsAvailable = true
                });
            }
            await DbContext.SaveChangesAsync();
        }
        protected INewBook<BookDTO> FakeNewBooksDTO(
            Guid? id = default,
            string? isbn = default,
            string? title = default,
            string? author = default,
            string? description = default,
            string? publisher = default,
            string? genre = default,
            int? quantity = 1)
        {
            return new NewBook
            {
                Book = new BookDTO
                {
                    Id = id ?? Guid.Empty,
                    ISBN = isbn ?? "978-3-16-148410-0",
                    Title = title ?? "Test Book 1",
                    Author = author ?? "Test Author 1",
                    Description = description ?? "Test Description 1",
                    Publisher = publisher ?? "Test Publisher 1",
                    Genre = genre ?? "Fiction",
                },
                Quantity = quantity ?? 1
            };
        }
        protected async Task<int> GetFilteredBooksCountAsync(string propertyName, object value)
        {
            var books = (await _bookHandler.GetAllBooks()).As<BookTaskResult>().Books!;
            return books.Count(b =>
            {
                var prop = typeof(BookDTO).GetProperty(propertyName);
                return prop?.GetValue(b)?.Equals(value) ?? false;
            });
        }
    }
}
