using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.Entities
{
    public class Warehouse : IWarehouse<int>
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
