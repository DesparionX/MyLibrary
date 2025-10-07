using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Http.Responses;

namespace MyLibrary.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
        private readonly IBorrowHandler _borrowHandler;
        private readonly IResultHandler<ITaskResult> _resultHandler;

        public BorrowsController(IBorrowHandler borrowHandler, IResultHandler<ITaskResult> resultHandler)
        {
            _borrowHandler = borrowHandler;
            _resultHandler = resultHandler;
        }

        [HttpGet("getAllBorrows")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status302Found)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBorrowsAsync()
        {
            var result = await _borrowHandler.GetBorrowHistoryAsync();
            return _resultHandler.ReadResult(result);
        }

        [HttpGet("getUserBorrows/{userId}")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status302Found)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserBorrowsAsync([FromRoute] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new BorrowTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "User ID cannot be null or white space."));
            }

            var result = await _borrowHandler.GetUserBorrowsAsync(userId);
            return _resultHandler.ReadResult(result);
        }

        [HttpGet("getBooksBorrows/{bookId}+{isReturned}")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status302Found)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooksBorrowsAsync([FromRoute] string bookId, [FromRoute] bool? isReturned = default)
        {
            if (string.IsNullOrWhiteSpace(bookId))
            {
                return BadRequest(new BorrowTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "Book ID cannot be null or white space."));
            }
            var result = await _borrowHandler.GetBooksBorrowsAsync(bookId, isReturned);
            return _resultHandler.ReadResult(result);
        }
    }
}
