namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface IEntity<TId> where TId : IEquatable<TId>
    {
        public TId? Id { get; set; }
    }
}
