using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserHandler> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserHandler(IMapper mapper, ILogger<UserHandler> logger, AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public async Task<ITaskResult> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                if (users.Count == 0)
                {
                    return new UserTaskResult(succeeded: false, message: "There are no users in db.", statusCode: StatusCodes.Status404NotFound);
                }
                var userDTOs = _mapper.Map<ICollection<IUserDTO>>(users);
                _logger.LogInformation("Users fetched successfully.");
                return new UserTaskResult(succeeded: true, message: "Users fetched successfully.", statusCode: StatusCodes.Status200OK, users: userDTOs);
            }
            catch(Exception err)
            {
                _logger.LogError(err, "[ERROR] \nError while fetching users.");
                return new UserTaskResult(succeeded: false, message: "Error while fetching users.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> GetUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new UserTaskResult(succeeded: false, message: "User not found.", statusCode: StatusCodes.Status404NotFound);
                }

                var userDTO = _mapper.Map<IUserDTO>(user);
                _logger.LogInformation("User found.");
                return new UserTaskResult(succeeded: true, message: "User found.", statusCode: StatusCodes.Status200OK, user: userDTO);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR] \nError while fetching user.");
                return new UserTaskResult(succeeded: false, message: "Error while fetching user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> RegisterUserAsync(INewUser newUserDTO)
        {
            try
            {
                var newUser = _mapper.Map<User>(newUserDTO.UserDTO);
                var result = await _userManager.CreateAsync(newUser, newUserDTO.Password);
                if (!result.Succeeded)
                {
                    return new UserTaskResult(succeeded: false, message: "Error while registering user.", statusCode: StatusCodes.Status400BadRequest);
                }

                _logger.LogInformation("User registered successfully.");
                return new UserTaskResult(succeeded: true, message: "User registered successfully.", statusCode: StatusCodes.Status201Created);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR] \nError while registering user.");
                return new UserTaskResult(succeeded: false, message: "Error while registering user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> UpdateUserAsync(IUserDTO userDTO)
        {
            try
            {
                var result = await _userManager.UpdateAsync(_mapper.Map<User>(userDTO));
                if (!result.Succeeded)
                {
                    _logger.LogError($"[ERROR] \nError while updating user. \n{result.Errors}");
                    return new UserTaskResult(succeeded: false, message: "Error while updating user.", statusCode: StatusCodes.Status400BadRequest);
                }
                _logger.LogInformation("User updated.");
                return new UserTaskResult(succeeded: true, message: "User updated", statusCode: StatusCodes.Status200OK);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR] \nError while updating user.");
                return new UserTaskResult(succeeded: false, message: "Error while updating user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ITaskResult> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new UserTaskResult(succeeded: false, message: "User not found.", statusCode: StatusCodes.Status404NotFound);
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError($"[ERROR] \nError while deleting user. \n{result.Errors}");
                    return new UserTaskResult(succeeded: false, message: "Error while deleting user.", statusCode: StatusCodes.Status400BadRequest);
                }

                _logger.LogInformation("User deleted.");
                return new UserTaskResult(succeeded: true, message: "User deleted.", statusCode: StatusCodes.Status200OK);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "[ERROR] \nError while deleting user.");
                return new UserTaskResult(succeeded: false, message: "Error while deleting user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
