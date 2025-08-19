using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.EntityFrameworkCore;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.Interfaces;

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

        protected async Task AddFakeBook(string? isbn, Guid? id)
        {
            DbContext.Add(new Book
            {
                Id = id ?? Guid.NewGuid(),
                ISBN = isbn ?? DateTime.Now.ToString(),
                Title = "Test Book",
                Author = "Test Author",
                IsAvailable = true
            });
            await DbContext.SaveChangesAsync();
        }
    }
}
