namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface IWarehouse<TId> : IEntity<TId> where TId : IEquatable<TId> 
    {
        public string ISBN { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
