using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public class WarehouseDTO : IWarehouseDTO<int>
    {
        public int Id { get; set; }

        public IWarehouse<int> Entity { get; set; }

        public string ISBN { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}
