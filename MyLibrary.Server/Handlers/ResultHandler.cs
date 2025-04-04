using Microsoft.AspNetCore.Mvc;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public class ResultHandler : ControllerBase, IResultHandler<ITaskResult>
    {
        public IActionResult ReadResult(ITaskResult response)
        {
            return response.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(response),
                StatusCodes.Status201Created => Ok(response),
                StatusCodes.Status302Found => Ok(response),
                StatusCodes.Status304NotModified => BadRequest(response),
                StatusCodes.Status400BadRequest => BadRequest(response),
                StatusCodes.Status401Unauthorized => Unauthorized(response),
                StatusCodes.Status403Forbidden => Forbid(),
                StatusCodes.Status404NotFound => NotFound(response),
                StatusCodes.Status409Conflict => Conflict(response),
                StatusCodes.Status500InternalServerError => BadRequest(response),
                StatusCodes.Status501NotImplemented => BadRequest(response),
                _ => BadRequest("Status code does not exist !")
            };
        }
    }
}
