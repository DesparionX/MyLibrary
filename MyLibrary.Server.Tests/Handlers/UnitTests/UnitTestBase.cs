using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MyLibrary.Server.Data;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Events;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Tests.Handlers.UnitTests
{
    public abstract class UnitTestBase
    {
        protected Mock<UserManager<User>> UserManager;
        protected Mock<SignInManager<User>> SignInManager;
        protected Mock<IJWTGenerator> JWTGenerator;
        protected Mock<IAuthHandler> AuthHandler;
        protected Mock<IBookHandler<IBook<Guid>>> BookHandler;
        protected Mock<IOperationHandler> OperationHandler;
        protected Mock<IResultHandler<ITaskResult>> ResultHandler;
        protected Mock<ISubscriptionHandler<User>> SubscriptionHandler;
        protected Mock<IUserHandler> UserHandler;
        protected Mock<IWarehouseHandler<IWarehouse<int>>> WarehouseHandler;
        protected Mock<EventBus> EventBus;
        protected IMapper Mapper;
        protected AppDbContext DbContext;

        [SetUp]
        public void SetupBase()
        {
            // Initialize the mocks for handlers and services
            AuthHandler = new Mock<IAuthHandler>();
            BookHandler = new Mock<IBookHandler<IBook<Guid>>>();
            OperationHandler = new Mock<IOperationHandler>();
            ResultHandler = new Mock<IResultHandler<ITaskResult>>();
            SubscriptionHandler = new Mock<ISubscriptionHandler<User>>();
            UserHandler = new Mock<IUserHandler>();
            WarehouseHandler = new Mock<IWarehouseHandler<IWarehouse<int>>>();
            
            EventBus = new Mock<EventBus>();
            JWTGenerator = new Mock<IJWTGenerator>();

            // Mock the UserManager and SignInManager
            var userStore = new Mock<IUserStore<User>>();
            UserManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            SignInManager = new Mock<SignInManager<User>>(
                UserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);

            // Configure the AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapHandler>();
            }, NullLoggerFactory.Instance);
            Mapper = mapperConfig.CreateMapper();

            // Initialize the in-memory database for testing
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            DbContext = new AppDbContext(options);
        }

        [TearDown]
        public void Cleanup()
        {
            // Clear the in-memory database after each test
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }

        protected async Task<User?> AddFakeUserAsync(
            string? id = default,
            string? userName = default,
            string? email = default,
            string? password = "Password@123")
        {
            var fakeUser = new User
            {
                Id = id ?? Guid.NewGuid().ToString(),
                UserName = userName ?? "testuser",
                Email = email ?? "testmail@abv.bg"
            };
            fakeUser.PasswordHash = new PasswordHasher<User>().HashPassword(fakeUser, password!);

            DbContext.Users.Add(fakeUser);
            await DbContext.SaveChangesAsync();

            return fakeUser;
        }
    }
}
