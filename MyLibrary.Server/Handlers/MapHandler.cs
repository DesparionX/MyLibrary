using AutoMapper;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;
using MyLibrary.Server.Data.Entities.Interfaces;
using MyLibrary.Shared.DTOs;
using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Handlers
{
    public class MapHandler : Profile
    {
        public MapHandler()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Warehouse, WarehouseDTO>().ReverseMap();
            CreateMap<IWarehouse<int>, IWarehouseDTO>().ReverseMap();
            CreateMap<Operation, OperationDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<SubscriptionTier, SubscriptionTierDTO>().ReverseMap();
            CreateMap<Subscription, SubscriptionDTO>().ReverseMap();
            CreateMap<Borrow, BorrowDTO>().ReverseMap();
        }
    }
}
