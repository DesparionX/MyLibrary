using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IOperationHandler _operationHandler;
        private readonly IResultHandler<ITaskResult> _resultHandler;
        

        public OperationsController(IOperationHandler operationHandler, IResultHandler<ITaskResult> resultHandler)
        {
            _operationHandler = operationHandler;
            _resultHandler = resultHandler;
        }

        [HttpGet("allOperations")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOperations()
        {
            var result = _operationHandler.GetOperationHistoryAsync();
            return _resultHandler.ReadResult(await result);
        }

        [HttpPost("performOperation")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PerformOperation([FromBody] OperationDTO operation)
        {
            if(operation == null)
            {
                return BadRequest(new OperationTaskResult(succeeded: false, message: "Operation cannot be null.", statusCode: StatusCodes.Status400BadRequest));
            }
            var result = await _operationHandler.PerformOperationAsync(operation);
            return _resultHandler.ReadResult(result);
        }
    }
}
