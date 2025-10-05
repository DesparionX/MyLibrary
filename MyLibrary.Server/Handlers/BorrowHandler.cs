using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.DTOs;
using MyLibrary.Shared.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Handlers
{
    public class BorrowHandler : IBorrowHandler
    {
        private readonly ILogger<BorrowHandler> _logger;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public BorrowHandler(ILogger<BorrowHandler> logger, IMapper mapper, AppDbContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ITaskResult> GetBorrowHistoryAsync()
        {
            try
            {
                var borrows = await _context.Borrows.ToListAsync();
                if (borrows.Count == 0)
                {
                    _logger.LogWarning("[WARNING] No borrow records found.");
                    return new BorrowTaskResult(succeeded: false, message: "No borrow records found.", statusCode: StatusCodes.Status404NotFound);
                }

                var borrowsDtos = new List<BorrowDTO>();
                borrows.ForEach(b => borrowsDtos.Add(_mapper.Map<BorrowDTO>(b)));

                _logger.LogInformation("[INFO] Found {Count} borrow records.", borrows.Count);
                return new BorrowTaskResult(succeeded: true, message: $"Found {borrows.Count} borrow records.", statusCode: StatusCodes.Status302Found, borrowDtos: borrowsDtos);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR]\n{Message}", err.Message);
                return new BorrowTaskResult(succeeded: false, message: err.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> GetUserBorrowsAsync(string userId)
        {
            try
            {
                var borrows = await _context.Borrows
                    .Where(b => b.UserId.Equals(userId))
                    .ToListAsync();

                if (borrows.Count == 0)
                {
                    _logger.LogWarning("[WARNING] No borrow records found for User ID: {UserId}.", userId);
                    return new BorrowTaskResult(succeeded: false, message: "No borrow records found for this user.", statusCode: StatusCodes.Status404NotFound);
                }

                var borrowsDtos = new List<BorrowDTO>();
                borrows.ForEach(b => borrowsDtos.Add(_mapper.Map<BorrowDTO>(b)));

                _logger.LogInformation("[INFO] Found {Count} borrow records for User ID: {UserId}.", borrows.Count, userId);
                return new BorrowTaskResult(succeeded: true, message: $"Found {borrows.Count} borrow records for this user.", statusCode: StatusCodes.Status302Found, borrowDtos: borrowsDtos);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR]\n{Message}", err.Message);
                return new BorrowTaskResult(succeeded: false, message: "Something went wrong!", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> GetBooksBorrowsAsync(string bookId, bool? isReturned = default)
        {
            try
            {
                var borrows = await _context.Borrows
                    .Where(b => b.BookId.Equals(bookId) && 
                    (
                        isReturned == null
                        || (isReturned == true && b.DateReturned != null)
                        || (isReturned == false && b.DateReturned == null))
                    )
                    .ToListAsync();

                if (borrows.Count == 0)
                {
                    _logger.LogWarning("[WARNING] No borrow records found for Book ID: {BookId}.", bookId);
                    return new BorrowTaskResult(succeeded: false, message: "No borrow records found for this book.", statusCode: StatusCodes.Status404NotFound);
                }

                var borrowsDtos = new List<BorrowDTO>();
                borrows.ForEach(b => borrowsDtos.Add(_mapper.Map<BorrowDTO>(b)));

                _logger.LogInformation("[INFO] Found {Count} borrow records for Book ID: {BookId}.", borrows.Count, bookId);
                return new BorrowTaskResult(succeeded: true, message: $"Found {borrows.Count} borrow records for this book.", statusCode: StatusCodes.Status302Found, borrowDtos: borrowsDtos);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR]\n{Message}", err.Message);
                return new BorrowTaskResult(succeeded: false, message: "Something went wrong!", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> BorrowBookAsync(string bookId, string userId)
        {
            try
            {
                var newBorrow = new Borrow
                {
                    BookId = bookId,
                    UserId = userId,
                    DateBorrowed = DateTime.UtcNow
                };

                _context.Borrows.Add(newBorrow);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("[INFO] Book with ID: {BookId} was borrowed by User ID: {UserId}.", bookId, userId);
                    return new BorrowTaskResult(succeeded: true, message: "Book was borrowed.", statusCode: StatusCodes.Status200OK);
                }

                _logger.LogWarning("[WARNING] Book with ID: {BookId} could not be borrowed by User ID: {UserId}.", bookId, userId);
                return new BorrowTaskResult(succeeded: false, message: "Book was not borrowed.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR]\n{Message}", err.Message);
                return new BorrowTaskResult(succeeded: false, message: "Something went wrong!", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> ReturnBookAsync(string bookId, string userId)
        {
            try
            {
                var borrow = await _context.Borrows
                    .Where(b => b.BookId.Equals(bookId) && b.UserId.Equals(userId) && b.DateReturned == null)
                    .SingleOrDefaultAsync();

                if (borrow is null)
                {
                    _logger.LogWarning("[WARNING] No active borrow record found for Book ID: {BookId} and User ID: {UserId}.", bookId, userId);
                    return new BorrowTaskResult(succeeded: false, message: "No active borrow record found for this book and user.", statusCode: StatusCodes.Status404NotFound);
                }

                borrow.DateReturned = DateTime.UtcNow;
                _context.Borrows.Update(borrow);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    _logger.LogInformation("[INFO] Book with ID: {BookId} was returned by User ID: {UserId}.", bookId, userId);
                    return new BorrowTaskResult(succeeded: true, message: "Book was returned.", statusCode: StatusCodes.Status200OK);
                }

                _logger.LogWarning("[WARNING] Book with ID: {BookId} could not be returned by User ID: {UserId}.", bookId, userId);
                return new BorrowTaskResult(succeeded: false, message: "Book was not returned.", statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR]\n{Message}", err.Message);
                return new BorrowTaskResult(succeeded: false, message: "Something went wrong!", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
