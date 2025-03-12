using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookHandler<Book> _bookHandler;
        private readonly ILogger<BooksController> _logger;
        private readonly IResultHandler<ITaskResponse> _resultHandler;

        public BooksController(IBookHandler<Book> bookHandler, ILogger<BooksController> logger, IResultHandler<ITaskResponse> resultHandler)
        {
            _bookHandler = bookHandler;
            _logger = logger;
            _resultHandler = resultHandler;
        }

        [HttpGet("findBookById")]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status302Found)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> FindById<TId>([FromQuery] TId id) where TId : IEquatable<TId>
        {
            if (id == null || string.IsNullOrWhiteSpace(id.ToString()))
            {
                return BadRequest(
                    new BookTaskResponse(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "ID cannot be null or empty.")
                    );
            }
            var response = await _bookHandler.FindBookById(id);

            return _resultHandler.ReadResult(response);
        }

        [HttpGet("findBookByISBN")]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status302Found)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> FindByISBN([FromQuery] string isbn)
        {
            if (isbn == null || string.IsNullOrWhiteSpace(isbn))
            {
                return BadRequest(
                    new BookTaskResponse(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "ISBN cannot be null or empty.")
                    );
            }
            var response = await _bookHandler.FindBookByISBN(isbn);
            return _resultHandler.ReadResult(response);
        }

        [HttpGet("getAllBooks")]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status302Found)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await _bookHandler.GetAllBooks();
            return _resultHandler.ReadResult(response);
        }

        [HttpPost("addBook")]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> AddBook([FromBody] INewBookDTO newBookDto)
        {
            if (newBookDto == null)
            {
                return BadRequest(
                    new BookTaskResponse(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "Book cannot be null.")
                    );
            }
            var response = await _bookHandler.AddBookAsync(newBookDto);
            return _resultHandler.ReadResult(response);
        }

        [HttpDelete("deleteBook")]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> DeleteBookAsync([FromQuery] Guid id)
        {
            if (id == null || string.IsNullOrWhiteSpace(id.ToString()))
            {
                return BadRequest(
                    new BookTaskResponse(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "ISBN cannot be null or empty.")
                    );
            }
            var response = await _bookHandler.DeleteBookAsync(id.ToString());
            return _resultHandler.ReadResult(response);
        }

        [HttpPut("updateBook")]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResponse>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> UpdateBookAsync([FromBody] IBookDTO bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest(
                    new BookTaskResponse(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "Book cannot be null.")
                    );
            }
            var response = await _bookHandler.UpdateBookAsync(bookDto);
            return _resultHandler.ReadResult(response);
        }
    }
}
