using AutoMapper;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Handlers
{
    public class MapHandler : Profile
    {
        public MapHandler()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Warehouse, WarehouseDTO>().ReverseMap();
            CreateMap<Operation, OperationDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<SubscriptionTier, SubscriptionTierDTO>().ReverseMap();
            CreateMap<Subscription, SubscriptionDTO>().ReverseMap();
        }
    }
}
