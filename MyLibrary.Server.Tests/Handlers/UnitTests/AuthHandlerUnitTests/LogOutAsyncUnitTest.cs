using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Http.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.AuthHandlerUnitTests
{
    [TestFixture]
    public class LogOutAsyncUnitTest : AuthHandlerUnitTestBase
    {

        [Test]
        public async Task LogOutAsync_ShouldReturnSuccess_WhenUserIsLoggedIn()
        {
            // Arrange
            var fakeIdentity = SetupFakeIdentity(userName: "fakeuser", "validmail@abv.bg", isAuthenticated: true);

            // Act
            var result = await _authHandler.LogOutAsync(fakeIdentity);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<AuthResult>(r => 
                r.Succeeded
                && r.StatusCode.Equals(StatusCodes.Status200OK)
                && r.Message!.Equals("User logged out successfully.")
                );
        }
        [Test]
        public async Task LogOutAsync_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var fakeIdentity = SetupFakeIdentity(userName: "fakeuser", "validmail@abv.bg", isAuthenticated: false);

            // Act
            var result = await _authHandler.LogOutAsync(fakeIdentity);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<AuthResult>(r => 
                !r.Succeeded 
                && r.StatusCode.Equals(StatusCodes.Status401Unauthorized)
                && r.Message!.Equals("User tried to log out without being authenticated.")
                );
        }
        [Test]
        public async Task LogOutAsync_ShouldReturnServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var fakeIdentity = SetupFakeIdentity(userName: "fakeuser", "validmail@abv.bg", isAuthenticated: true);

            SignInManager
                .Setup(sm => sm.SignOutAsync())
                .Throws(new Exception("Unhandled exception during logout."));

            // Act
            var result = await _authHandler.LogOutAsync(fakeIdentity);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<AuthResult>(r => 
                !r.Succeeded 
                && r.StatusCode.Equals(StatusCodes.Status500InternalServerError)
                && r.Message!.Equals("An error occurred while logging out user.")
                );
        }
    }
}
