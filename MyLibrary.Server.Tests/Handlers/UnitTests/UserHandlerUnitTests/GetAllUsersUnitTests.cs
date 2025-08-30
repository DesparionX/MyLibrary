using FluentAssertions;
using Microsoft.AspNetCore.Http;
using MyLibrary.Server.Http.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.UserHandlerUnitTests
{
    [TestFixture]
    public class GetAllUsersUnitTests : UserHandlerUnitTestBase
    {
        [Test]
        public async Task GetAllUsers_ShouldReturnAllUsers_WhenUsersExist()
        {
            // Arrange
            var expectedUserCount = 5;
            await SeedUsers(count: expectedUserCount);

            // Act
            var result = await _userHandler.GetAllUsers();

            // Assert
            result.As<ITaskResult>().Should().NotBeNull()
                .And
                .Match<UserTaskResult>(r => 
                r.Succeeded
                && r.StatusCode == StatusCodes.Status200OK
                && r.Message!.Equals("Users fetched successfully.")
                && r.Users!.Count == expectedUserCount);
        }
    }
}
