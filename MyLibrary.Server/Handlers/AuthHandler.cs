using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyLibrary.Server.Configs;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthHandler(IMapper mapper, ILogger<AuthHandler> logger, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _mapper = mapper;
            _logger = logger;
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
                    _logger.LogWarning($"User with email {request.Email} not found.");
                    return new UserTaskResult(succeeded: false, message: "Couldn't find user with the given email.", statusCode: StatusCodes.Status404NotFound);
                }

                var result = await _signInManager.PasswordSignInAsync(user: user, password: request.Password!, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var jwt = await GenerateJwt(user);
                    var dto = _mapper.Map<IUserDTO>(user);

                    _logger.LogInformation($"User with email {request.Email} logged in successfully.");
                    return new AuthResult(succeeded: true, message: "User logged in successfully.", statusCode: StatusCodes.Status200OK, token: jwt, user: dto);
                }

                _logger.LogWarning($"User with email {request.Email} failed to log in.");
                return new AuthResult(succeeded: false, message: "Invalid password.", statusCode: StatusCodes.Status401Unauthorized);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error occurred while logging in user.\n{err.StackTrace}");
                return new AuthResult(succeeded: false, message: "An error occurred while logging in user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> LogOutAsync(IIdentity userIdentity)
        {
            try
            {
                if(userIdentity == null)
                {
                    _logger.LogWarning("User not logged in.");
                    return new AuthResult(succeeded: false, message: "User not logged in.", statusCode: StatusCodes.Status401Unauthorized);
                }

                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out successfully.");
                return new AuthResult(succeeded: true, message: "User logged out successfully.", statusCode: StatusCodes.Status200OK);
            }
            catch(Exception err)
            {
                _logger.LogError(err, $"Error occurred while logging out user.\n{err.StackTrace}");
                return new AuthResult(succeeded: false, message: "An error occurred while logging out user.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ITaskResult> GetIdentityAsync(IIdentity userIdentity)
        {
            try
            {
                if (userIdentity == null)
                {
                    _logger.LogWarning("User not logged in.");
                    return new AuthResult(succeeded: false, message: "User not logged in.", statusCode: StatusCodes.Status401Unauthorized);
                }
                var claimsIdentity = userIdentity as ClaimsIdentity;
                var email = claimsIdentity!.FindFirst(ClaimTypes.Email)?.Value;
                var user = await _userManager.FindByEmailAsync(email!);

                if (user == null)
                {
                    _logger.LogWarning($"User with email {email} not found.");
                    return new UserTaskResult(succeeded: false, message: "Couldn't find user with the given email.", statusCode: StatusCodes.Status404NotFound);
                }
                var dto = _mapper.Map<IUserDTO>(user);
                return new AuthResult(succeeded: true, message: "User found.", statusCode: StatusCodes.Status200OK, user: dto);
            }
            catch (Exception err)
            {
                _logger.LogError(err, $"Error occurred while getting user identity.\n{err.StackTrace}");
                return new AuthResult(succeeded: false, message: "An error occurred while getting user identity.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<string> GenerateJwt(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: JwtConfig.JwtIssuer,
                audience: JwtConfig.JwtAudience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
