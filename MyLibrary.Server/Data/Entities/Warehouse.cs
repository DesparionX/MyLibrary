using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.Entities
{
    public class Warehouse : IWarehouse<int>
    {
        public int Id { get; set; }
        public required string ISBN { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
