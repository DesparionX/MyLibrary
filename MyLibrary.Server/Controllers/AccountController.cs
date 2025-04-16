using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;
using System.Security.Claims;

namespace MyLibrary.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserHandler _userHandler;
        private readonly IAuthHandler _authHandler;
        private readonly IResultHandler<ITaskResult> _resultHandler;

        public AccountController(ILogger<AccountController> logger, IUserHandler userHandler, IAuthHandler authHandler,IResultHandler<ITaskResult> resultHandler)
        {
            _logger = logger;
            _userHandler = userHandler;
            _authHandler = authHandler;
            _resultHandler = resultHandler;
        }

        [HttpGet("user")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser([FromQuery] string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new UserTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "User ID cannot be null or white space."));
            }
            var response = await _userHandler.GetUserAsync(id);

            return _resultHandler.ReadResult(response);
        }

        [HttpGet("getUsers")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userHandler.GetAllUsers();
            return _resultHandler.ReadResult(response);
        }

        [HttpPost("registerUser")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status201Created)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] NewUser newUserDto)
        {
            if (newUserDto == null)
            {
                return BadRequest(new UserTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "RegisterDTO cannot be null."));
            }
            var response = await _userHandler.RegisterUserAsync(newUserDto);
            return _resultHandler.ReadResult(response);
        }

        [HttpPut("updateUser")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new UserTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "UserDTO cannot be null."));
            }
            var response = await _userHandler.UpdateUserAsync(userDto);
            return _resultHandler.ReadResult(response);
        }

        [HttpDelete("deleteUser")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromQuery] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new UserTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "User ID cannot be null or white space."));
            }
            var response = await _userHandler.DeleteUserAsync(id);
            return _resultHandler.ReadResult(response);
        }

        [HttpPost("login")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest(new UserTaskResult(succeeded: false, statusCode: StatusCodes.Status400BadRequest, message: "Login request cannot be null."));
            }
            var response = await _authHandler.LoginUserAsync(request);
            return _resultHandler.ReadResult(response);
        }

        [HttpGet("getIdentity")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetIdentity()
        {
            var response = await _authHandler.GetIdentityAsync(User.Identity);
            return _resultHandler.ReadResult(response);
        }

        [HttpPost("logout")]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status200OK)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ITaskResult>(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            var response = await _authHandler.LogOutAsync(User.Identity);
            return _resultHandler.ReadResult(response);
        }
    }
}
