using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Requests;
using System.Security.Claims;
using System.Security.Principal;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.AuthHandlerUnitTests
{
    public abstract class AuthHandlerUnitTestBase : UnitTestBase
    {
        protected IAuthHandler _authHandler;

        [SetUp]
        public void Setup()
        {
            _authHandler = new AuthHandler(
                Mapper,
                new Mock<ILogger<AuthHandler>>().Object,
                JWTGenerator.Object,
                SignInManager.Object,
                UserManager.Object
            );
        }
        protected void SetupAuthManagersForLogin(ILoginRequest request, User? user)
        {
            UserManager
                .Setup(um => um.FindByEmailAsync(request.Email!))
                .ReturnsAsync(user);

            if (user is not null)
            {
                SignInManager
                .Setup(sm => sm.PasswordSignInAsync(
                    user,
                    request.Password!,
                    request.RememberMe!.Value,
                    false))
                .ReturnsAsync(
                    string.IsNullOrWhiteSpace(request.Password) ?
                    SignInResult.Failed : SignInResult.Success
                    );

                JWTGenerator
                    .Setup(jg => jg.GenerateJWT(user))
                    .ReturnsAsync("mocked-jwt-token");

            }
        }
        protected static IIdentity SetupFakeIdentity(string? userName, string? email,bool isAuthenticated = true)
        {
            var fakeIdentity = new Mock<ClaimsIdentity>();
            fakeIdentity.Setup(x => x.IsAuthenticated).Returns(isAuthenticated);
            fakeIdentity.Setup(x => x.Name).Returns(userName);
            fakeIdentity.Setup(x => x.FindFirst(ClaimTypes.Name)).Returns(new Claim(ClaimTypes.Name, userName!));
            fakeIdentity.Setup(x => x.FindFirst(ClaimTypes.Email)).Returns(new Claim(ClaimTypes.Name, email!));
            fakeIdentity.Setup(x => x.Claims).Returns(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName!),
                new Claim(ClaimTypes.Email, email!)
            });


            return fakeIdentity.Object;
        }
        protected static string GetUserEmailFromIdentity(IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            return claimsIdentity!.FindFirst(ClaimTypes.Email)!.Value;
        }
    }
}
