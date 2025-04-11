using MyLibrary.Server.Data.DTOs.Interfaces;
using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public class WarehouseDTO : IWarehouseDTO
    {
        public int Id { get; set; }

        public string ISBN { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}
