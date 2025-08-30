using Castle.Core.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Server.Tests.Handlers.UnitTests.UserHandlerUnitTests
{
    public abstract class UserHandlerUnitTestBase : UnitTestBase
    {
        protected IUserHandler _userHandler;

        [SetUp]
        public void Setup()
        {
            _userHandler = new UserHandler(
                Mapper,
                new Mock<ILogger<UserHandler>>().Object,
                UserManager.Object);

            SetUpUserManager();
        }

        protected async Task SeedUsers(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = $"Random user {i}",
                    Email = $"validmail{i}@abv.bg"
                };
                user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "Password@123");
                await DbContext.Users.AddAsync(user);
                await DbContext.SaveChangesAsync();
            }
        }

        private void SetUpUserManager()
        {
            UserManager.Setup(um => um.Users).Returns(DbContext.Users);
        }
    }
}
