namespace MyLibrary.Server.Data.Entities
{
    public interface IWarehouse<TId> : IEntity<TId> where TId : IEquatable<TId> 
    {
        public string ISBN { get; }
        public string Name { get; }
        public int Quantity { get; }
    }
}
