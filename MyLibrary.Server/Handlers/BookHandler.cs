using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Events;
using MyLibrary.Server.Handlers.EventHandlers;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public class BookHandler : IBookHandler<Book>
    {
        private readonly EventBus _eventBus;
        private readonly IOperationsEventHandler _eventHandler;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookHandler> _logger;

        public BookHandler(EventBus eventBus, IOperationsEventHandler eventHandler, AppDbContext context, IMapper mapper, ILogger<BookHandler> logger)
        {
            _eventBus = eventBus;
            _eventHandler = eventHandler;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        
        public async Task<ITaskResult> FindBookById<TId>(TId id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if(book == null)
                {
                    return new BookTaskResult(succeeded: false, message: "No book found with given ID.", statusCode: StatusCodes.Status404NotFound);
                }
                var bookDto = _mapper.Map<BookDTO>(book);

                return new BookTaskResult(succeeded: true, message: "Book found !", statusCode: StatusCodes.Status302Found, book: bookDto);
            }
            catch (Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> FindBookByISBN(string ISBN)
        {
            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == ISBN);
                if (book == null)
                {
                    return new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status404NotFound, "There is no books with given ISBN.");
                }
                var bookDto = _mapper.Map<BookDTO>(book);
                return new BookTaskResult(succeeded: true, statusCode: StatusCodes.Status302Found, "Book found !", book: bookDto);
            }
            catch (Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> GetAllBooks()
        {
            try
            {
                var books = await _context.Books.ToListAsync();
                if (books.Count == 0)
                {
                    return new BookTaskResult(succeeded: false, message: "No books found.", statusCode: StatusCodes.Status404NotFound);
                }
                var bookDtos = _mapper.Map<ICollection<BookDTO>>(books);
                return new BookTaskResult(succeeded: true, message: "Books found !", statusCode: StatusCodes.Status302Found, books: bookDtos);
            }
            catch (Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> AddBookAsync(INewBook<BookDTO> newBookDto)
        {
            try
            {
                if(await BookAlreadyExistAsync(newBookDto.Book))
                {
                    return new BookTaskResult(succeeded: false, message: "Book already exist in the database.", statusCode: StatusCodes.Status409Conflict);
                }

                var addedBooks = await CreateNewBooksAsync(existingBook: newBookDto.Book, quantity: newBookDto.Quantity);
                if (addedBooks != newBookDto.Quantity)
                {
                    return new BookTaskResult(succeeded: false, message: "Added books differs from requested quantity.", statusCode: StatusCodes.Status400BadRequest);
                }

                _logger.LogInformation($"{addedBooks} books added to the database.");
                _eventBus.Publish(new ItemAddedEvent(newBookDto.Book.ISBN, newBookDto.Book.Title, newBookDto.Quantity));
                return new BookTaskResult(succeeded: true, message: $"{addedBooks} books added successfully.", statusCode: StatusCodes.Status201Created);
            }
            catch(Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> DeleteBookAsync(Guid id)
        {
            try
            {
                var bookToRemove = await _context.Books.FindAsync(id);
                if (bookToRemove == null)
                {
                    return new BookTaskResult(succeeded: false, message: "No book found with given ID.", statusCode: StatusCodes.Status404NotFound);
                }

                _context.Books.Remove(bookToRemove);

                var removedBooks = await _context.SaveChangesAsync();
                if (removedBooks == 0)
                {
                    return new BookTaskResult(succeeded: false, message: "No books removed from the database.", statusCode: StatusCodes.Status400BadRequest);
                }

                _logger.LogInformation($"{removedBooks} books removed from the database.");
                _eventBus.Publish(new ItemRemovedEvent(bookToRemove.ISBN, bookToRemove.Title, removedBooks));
                return new BookTaskResult(succeeded: true, message: $"{removedBooks} books removed successfully.", statusCode: StatusCodes.Status200OK);
            }
            catch(Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> UpdateBookAsync(IBookDTO bookDto)
        {
            try
            {
                var booksToUpdate = await _context.Books.Where(b => b.ISBN == bookDto.ISBN).ToListAsync();
                if(booksToUpdate.Count == 0)
                {
                    return new BookTaskResult(succeeded: false, message: "No books found with given ISBN.", statusCode: StatusCodes.Status404NotFound);
                }
                foreach(var book in booksToUpdate)
                {
                    // Converting BookDTO to Book just in case there are different property names or special mapping.
                    var convertedBook = _mapper.Map<Book>(bookDto);
                    _context.Entry(book).CurrentValues.SetValues(convertedBook);

                    // Preventing Id from being updated.
                    _context.Entry(book).Property(b => b.Id).IsModified = false;
                }
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                {
                    return new BookTaskResult(succeeded: false, message: "No books updated.", statusCode: StatusCodes.Status400BadRequest);
                }
                return new BookTaskResult(succeeded: true, message: $"{result} books updated successfully.", statusCode: StatusCodes.Status200OK);
            }
            catch(Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResult(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        // Private methods
        private async Task<bool> BookAlreadyExistAsync(IBookDTO bookDto)
        {
            try
            {
                return await _context.Books.AnyAsync(b => b.Id.Equals(bookDto.Id));
            }
            catch (Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return false;
            }
        }
        private async Task<int> CreateNewBooksAsync(IBookDTO existingBook, int quantity)
        {
            try
            {
                for (int i = 0; i < quantity; i++)
                {
                    var book = _mapper.Map<Book>(existingBook);
                    await _context.Books.AddAsync(book);
                    _logger.LogInformation($"Book {book.Title} added to the database.");
                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return 0;
            }
        }
    }
}
