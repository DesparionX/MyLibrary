using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public class ResultHandler : ControllerBase, IResultHandler<ITaskResult>
    {
        public IActionResult ReadResult(ITaskResult response)
        {
            return response.StatusCode switch
            {
                StatusCodes.Status200OK
                or StatusCodes.Status201Created
                or StatusCodes.Status302Found => Ok(response),

                StatusCodes.Status304NotModified
                or StatusCodes.Status400BadRequest => BadRequest(response),

                StatusCodes.Status401Unauthorized => Unauthorized(response),
                StatusCodes.Status403Forbidden => Forbid(),
                StatusCodes.Status404NotFound => NotFound(response),
                StatusCodes.Status409Conflict => Conflict(response),

                StatusCodes.Status500InternalServerError 
                or StatusCodes.Status501NotImplemented => StatusCode(response.StatusCode, response),

                _ => BadRequest("Status code does not exist !")
            };
        }
    }
}
