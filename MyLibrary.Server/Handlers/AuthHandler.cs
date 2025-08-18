using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyLibrary.Server.Configs;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MyLibrary.Server.Handlers
{
    public class AuthHandler : IAuthHandler
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuthHandler> _logger;
        private readonly IJWTGenerator _jwtGenerator;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthHandler(
            IMapper mapper, 
            ILogger<AuthHandler> logger, 
            IJWTGenerator jwtGenerator,
            SignInManager<User> signInManager, 
            UserManager<User> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<ITaskResult> LoginUserAsync(ILoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email!);
                if (user == null)
                {
                    _logger.LogWarning("User with email {Email} not found.", request.Email);
                    return new UserTaskResult(succeeded: false, message: "Couldn't find user with the given email.", statusCode: StatusCodes.Status404NotFound);
                }

                var result = await _signInManager.PasswordSignInAsync(user: user, password: request.Password!, isPersistent: (bool)request.RememberMe!, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var jwt = await _jwtGenerator.GenerateJWT(user);
                    var dto = _mapper.Map<UserDTO>(user);

                    _logger.LogInformation("User with email {Email} logged in successfully.", request.Email);
                    return new AuthResult(succeeded: true, message: "User logged in successfully.", statusCode: StatusCodes.Status200OK, token: jwt, user: dto);
                }

                _logger.LogWarning("User with email {Email} failed to log in.", request.Email);
                return new AuthResult(succeeded: false, message: "Invalid password.", statusCode: StatusCodes.Status401Unauthorized);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Error occurred while logging in user.\n{StackTrace}", err.StackTrace);
                return new AuthResult(succeeded: false, message: "An error occurred while logging in user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> LogOutAsync(IIdentity userIdentity)
        {
            try
            {
                if (!userIdentity.IsAuthenticated)
                {
                    _logger.LogInformation("User tried to log out without being authenticated.");
                    return new AuthResult(succeeded: false, message: "User tried to log out without being authenticated.", statusCode: StatusCodes.Status401Unauthorized);
                }

                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out successfully.");
                return new AuthResult(succeeded: true, message: "User logged out successfully.", statusCode: StatusCodes.Status200OK);
            }
            catch(Exception err)
            {
                _logger.LogError(err, "Error occurred while logging out user.\n{StackTrace}", err.StackTrace);
                return new AuthResult(succeeded: false, message: "An error occurred while logging out user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> GetIdentityAsync(IIdentity userIdentity)
        {
            try
            {
                var claimsIdentity = userIdentity as ClaimsIdentity;
                var email = claimsIdentity!.FindFirst(ClaimTypes.Email)?.Value;
                var user = await _userManager.FindByEmailAsync(email!);

                if (user == null)
                {
                    _logger.LogWarning("User with email {Email} not found.",email);
                    return new AuthResult(succeeded: false, message: "Couldn't find user with the given email.", statusCode: StatusCodes.Status404NotFound);
                }
                var dto = _mapper.Map<UserDTO>(user);
                return new AuthResult(succeeded: true, message: "User identity retrieved successfully.", statusCode: StatusCodes.Status200OK, user: dto);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Error occurred while getting user identity. \n{StackTrace}", err.StackTrace);
                return new AuthResult(succeeded: false, message: "An error occurred while retrieving user identity.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
