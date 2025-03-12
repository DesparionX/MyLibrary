using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Events;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public class BookHandler : IBookHandler<Book>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookHandler> _logger;

        public BookHandler(AppDbContext context, IMapper mapper, ILogger<BookHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        
        public async Task<ITaskResponse> FindBookById<TId>(TId id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if(book == null)
                {
                    return new BookTaskResponse(succeeded: false, message: "No book found with given ID.", statusCode: StatusCodes.Status404NotFound);
                }
                var bookDto = _mapper.Map<BookDTO>(book);

                return new BookTaskResponse(succeeded: true, message: "Book found !", statusCode: StatusCodes.Status302Found, book: (IBookDTO)bookDto);
            }
            catch (Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResponse(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResponse> FindBookByISBN(string ISBN)
        {
            try
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.ISBN == ISBN);
                if (book == null)
                {
                    return new BookTaskResponse(succeeded: false, statusCode: StatusCodes.Status404NotFound, "There is no books with given ISBN.");
                }
                var bookDto = _mapper.Map<BookDTO>(book);
                return new BookTaskResponse(succeeded: true, statusCode: StatusCodes.Status302Found, "Book found !", book: (IBookDTO)bookDto);
            }
            catch (Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResponse(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResponse> GetAllBooks()
        {
            try
            {
                var books = await _context.Books.ToListAsync();
                if (books.Count == 0)
                {
                    return new BookTaskResponse(succeeded: false, message: "No books found.", statusCode: StatusCodes.Status404NotFound);
                }
                var bookDtos = _mapper.Map<ICollection<BookDTO>>(books);
                return new BookTaskResponse(succeeded: true, message: "Books found !", statusCode: StatusCodes.Status302Found, books: (ICollection<IBookDTO>)bookDtos);
            }
            catch (Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResponse(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResponse> AddBookAsync(INewBookDTO newBookDTO)
        {
            try
            {
                if(await BookAlreadyExistAsync(newBookDTO.Book))
                {
                    return new BookTaskResponse(succeeded: false, message: "Book already exist in the database.", statusCode: StatusCodes.Status400BadRequest);
                }

                var addedBooks = await CreateNewBooksAsync(existingBook: newBookDTO.Book, quantity: newBookDTO.Quantity);
                if (addedBooks != newBookDTO.Quantity)
                {
                    return new BookTaskResponse(succeeded: false, message: "Added books differs from requested quantity.", statusCode: StatusCodes.Status400BadRequest)
                }

                _logger.LogInformation($"{addedBooks} books added to the database.");
                EventBus.Publish(new ItemAddedEvent(newBookDTO.Book.ISBN, newBookDTO.Book.Title, newBookDTO.Quantity));
                return new BookTaskResponse(succeeded: true, message: $"{addedBooks} books added successfully.", statusCode: StatusCodes.Status201Created);
            }
        }

        public async Task<ITaskResponse> AddBookAsync<TId>(INewBookDTO<TId> bookDto) where TId : IEquatable<TId>
        {
            // TODO: Implement this method later.
            return new BookTaskResponse(succeeded: false, message: "Not implemented yet.", statusCode: StatusCodes.Status501NotImplemented);
        }

        public async Task<ITaskResponse> DeleteBookAsync(Guid id)
        {
            try
            {
                var bookToRemove = await _context.Books.FindAsync(id);
                if (bookToRemove == null)
                {
                    return new BookTaskResponse(succeeded: false, message: "No book found with given ID.", statusCode: StatusCodes.Status404NotFound);
                }

                _context.Books.Remove(bookToRemove);

                var removedBooks = await _context.SaveChangesAsync();
                if (removedBooks == 0)
                {
                    return new BookTaskResponse(succeeded: false, message: "No books removed from the database.", statusCode: StatusCodes.Status400BadRequest);
                }

                _logger.LogInformation($"{removedBooks} books removed from the database.");
                EventBus.Publish(new ItemRemovedEvent(bookToRemove.ISBN, bookToRemove.Title, removedBooks));
                return new BookTaskResponse(succeeded: true, message: $"{removedBooks} books removed successfully.", statusCode: StatusCodes.Status200OK);
            }
            catch(Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResponse(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResponse> DeleteBookAsync<TId>(TId id)
        {
            // TODO: Implement this method later.
            return new BookTaskResponse(succeeded: false, message: "Not implemented yet.", statusCode: StatusCodes.Status501NotImplemented);
        }

        public async Task<ITaskResponse> UpdateBookAsync(IBookDTO bookDto)
        {
            try
            {
                var booksToUpdate = await _context.Books.Where(b => b.ISBN == bookDto.ISBN).ToListAsync();
                if(booksToUpdate.Count == 0)
                {
                    return new BookTaskResponse(succeeded: false, message: "No books found with given ISBN.", statusCode: StatusCodes.Status404NotFound);
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
                    return new BookTaskResponse(succeeded: false, message: "No books updated.", statusCode: StatusCodes.Status400BadRequest);
                }
                return new BookTaskResponse(succeeded: true, message: $"{result} books updated successfully.", statusCode: StatusCodes.Status200OK);
            }
            catch(Exception err)
            {
                _logger.LogError($"[ERROR] {err.Message}\n{err.StackTrace}", err);
                return new BookTaskResponse(succeeded: false, message: "Something went wrong !", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResponse> UpdateBookAsync<TId>(IBookDTO<TId> bookDto) where TId : IEquatable<TId>
        {
            // TODO: Implement this method later.
            return new BookTaskResponse(succeeded: false, message: "Not implemented yet.", statusCode: StatusCodes.Status501NotImplemented);
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
                for(int i = 0; i <= quantity; i++)
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
