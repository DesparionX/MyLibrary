using MyLibrary.Server.Data.Entities;
using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Data.DTOs
{
    public class WarehouseDTO : IWarehouseDTO
    {
        public int Id { get; set; }

        public string ISBN { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
