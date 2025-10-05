using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookHandler<IBook<Guid>> _bookHandler;
        private readonly ILogger<BooksController> _logger;
        private readonly IResultHandler<ITaskResult> _resultHandler;

        public BooksController(IBookHandler<IBook<Guid>> bookHandler, ILogger<BooksController> logger, IResultHandler<ITaskResult> resultHandler)
        {
            _bookHandler = bookHandler;
            _logger = logger;
            _resultHandler = resultHandler;
        }

        [HttpGet("findBookById/{id}")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status302Found)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> FindById([FromRoute] string id)
        {
            if (id == null || string.IsNullOrWhiteSpace(id.ToString()))
            {
                return BadRequest(new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "ID cannot be null or empty."));
            }
            var response = await _bookHandler.FindBookById(id);

            return _resultHandler.ReadResult(response);
        }

        [HttpGet("findBookByISBN/{isbn}+{isAvailable}")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status302Found)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> FindByISBN([FromRoute] string isbn, bool? isAvailable = default)
        {
            if (isbn == null || string.IsNullOrWhiteSpace(isbn))
            {
                return BadRequest(new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "ISBN cannot be null or empty."));
            }
            var response = await _bookHandler.FindBookByISBN(isbn, isAvailable);
            return _resultHandler.ReadResult(response);
        }

        [HttpGet("getAllBooks")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status302Found)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await _bookHandler.GetAllBooks();
            return _resultHandler.ReadResult(response);
        }

        [HttpPost("addBook")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> AddBook([FromBody] NewBook newBookDto)
        {
            if (newBookDto == null)
            {
                return BadRequest(new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "Book cannot be null."));
            }
            var response = await _bookHandler.AddBookAsync(newBookDto);
            return _resultHandler.ReadResult(response);
        }

        [HttpDelete("deleteBook/{id}")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> DeleteBookAsync([FromRoute] Guid id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString()))
            {
                return BadRequest(new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "ISBN cannot be null or empty."));
            }
            var response = await _bookHandler.DeleteBookAsync(id);
            return _resultHandler.ReadResult(response);
        }

        [HttpPut("updateBook")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> UpdateBookAsync([FromBody] BookDTO bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest(new BookTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "Book cannot be null."));
            }
            var response = await _bookHandler.UpdateBookAsync(bookDto);
            return _resultHandler.ReadResult(response);
        }
    }
}
