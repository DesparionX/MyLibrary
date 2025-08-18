using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Server.Events;
using MyLibrary.Server.Handlers;
using MyLibrary.Server.Handlers.Interfaces;
using MyLibrary.Server.Http.Responses;

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

        [SetUp]
        public void SetupBase()
        {
            var userStore = new Mock<IUserStore<User>>();
            UserManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            SignInManager = new Mock<SignInManager<User>>(
                UserManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null, null, null, null);

            AuthHandler = new Mock<IAuthHandler>();
            BookHandler = new Mock<IBookHandler<IBook<Guid>>>();
            OperationHandler = new Mock<IOperationHandler>();
            ResultHandler = new Mock<IResultHandler<ITaskResult>>();
            SubscriptionHandler = new Mock<ISubscriptionHandler<User>>();
            UserHandler = new Mock<IUserHandler>();
            WarehouseHandler = new Mock<IWarehouseHandler<IWarehouse<int>>>();
            EventBus = new Mock<EventBus>();
            JWTGenerator = new Mock<IJWTGenerator>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapHandler>();
                // Add other profiles as needed
            }, NullLoggerFactory.Instance);
            Mapper = mapperConfig.CreateMapper();
        }
    }
}
