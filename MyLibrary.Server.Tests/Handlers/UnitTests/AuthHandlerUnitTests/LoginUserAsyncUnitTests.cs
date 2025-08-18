using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Requests;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.AuthHandlerUnitTests
{
    [TestFixture]
    public class LoginUserAsyncUnitTests : AuthHandlerUnitTestBase
    {
        [Test]
        public async Task LoginUserAsync_ShouldReturnSuccess_WhenCredentialsAreValid()

        {
            // Arrange
            var request = new LoginRequest(
                email: "test@abv.bg",
                password: "password123",
                rememberMe: true
                );

            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "Test", Email = request.Email };
            SetupAuthManagersForLogin(request, user);

            // Act
            var result = await _authHandler.LoginUserAsync(request);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<AuthResult>(r => 
                r.Succeeded 
                && r.StatusCode == StatusCodes.Status200OK
                && r.User!.Email == request.Email);
        }

        [Test]
        public async Task LoginUserAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new LoginRequest(
                email: "notexisting@abv.bg",
                password: "password123",
                rememberMe: true
                );

            SetupAuthManagersForLogin(request, null);

            // Act
            var result = await _authHandler.LoginUserAsync(request);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<UserTaskResult>(r => 
                !r.Succeeded 
                && r.StatusCode == StatusCodes.Status404NotFound
                && r.Message == "Couldn't find user with the given email.");
        }

        [Test]
        public async Task LoginUserAsync_ShouldReturnUnauthorized_WhenPasswordIsIncorrect()
        {
            // Arrange
            var request = new LoginRequest(
                email: "validemail@abv.bg",
                password: string.Empty,
                rememberMe: true
                );
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "Test", Email = request.Email };
            SetupAuthManagersForLogin(request, user);

            // Act
            var result = await _authHandler.LoginUserAsync(request);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<AuthResult>(r => 
                !r.Succeeded 
                && r.StatusCode == StatusCodes.Status401Unauthorized
                && r.Message == "Invalid password.");
        }

        [Test]
        public async Task LoginUserAsync_ShouldReturnInternalServerError_WhenUnexpectedExceptionThrows()
        {
            // Arrange
            var request = new LoginRequest(
                email: "validmail@abv.bg",
                password: "password123",
                rememberMe: true);

            UserManager.Setup(x => x.FindByEmailAsync(request.Email!))
                .Throws(new Exception("A test exception"));

            // Act
            var result = await _authHandler.LoginUserAsync(request);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<AuthResult>(r => 
                !r.Succeeded
                && r.StatusCode.Equals(StatusCodes.Status500InternalServerError)
                && r.Message != null
                && r.Message.Equals("An error occurred while logging in user."));
        }
    }
}
