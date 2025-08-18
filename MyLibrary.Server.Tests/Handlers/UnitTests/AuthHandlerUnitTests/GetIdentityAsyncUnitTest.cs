using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Http.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.AuthHandlerUnitTests
{
    [TestFixture]
    public class GetIdentityAsyncUnitTest : AuthHandlerUnitTestBase
    {
        [Test]
        public async Task GetIdentityAsync_ShouldReturnLoggedInUserDTO_WhenIsAuthenticated()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "fakeuser",
                Email = "validmail@abv.bg"
            };
            var fakeIdentity = SetupFakeIdentity(userName: user.UserName, email: user.Email, isAuthenticated: true);
            

            UserManager.Setup(um => um.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _authHandler.GetIdentityAsync(fakeIdentity);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<AuthResult>(r =>
                    r.Succeeded
                    && r.StatusCode.Equals(StatusCodes.Status200OK)
                    && r.Message!.Equals("User identity retrieved successfully.")
                    && r.User != null
                    && r.User!.Id.Equals(user.Id)
                    && r.User.Email!.Equals(user.Email)
                );
        }

        [Test]
        public async Task GetIdentityAsync_ShouldReturnNotFound_WhenUserNotFound()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "fakeuser",
                Email = "notregisteredmail@abv.bg"
            };

            var fakeIdentity = SetupFakeIdentity(userName: user.UserName, email: user.Email, isAuthenticated: false);

            UserManager.Setup(um => um.FindByEmailAsync(user.Email))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _authHandler.GetIdentityAsync(fakeIdentity);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<AuthResult>(r =>
                    !r.Succeeded
                    && r.StatusCode.Equals(StatusCodes.Status404NotFound)
                    && r.Message!.Equals("Couldn't find user with the given email.")
                );
        }

        [Test]
        public async Task GetIdentityAsync_ShouldThrowInternalServerError_WhenUnexpectedException()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "fakeuser",
                Email = "validmail@abv.bg"
            };
            var fakeIdentity = SetupFakeIdentity(userName: user.UserName, email: user.Email, isAuthenticated: true);

            UserManager.Setup(um => um.FindByEmailAsync(user.Email))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _authHandler.GetIdentityAsync(fakeIdentity);

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<AuthResult>(r =>
                    !r.Succeeded
                    && r.StatusCode.Equals(StatusCodes.Status500InternalServerError)
                    && r.Message!.Equals("An error occurred while retrieving user identity.")
                );
        }
    }
}
