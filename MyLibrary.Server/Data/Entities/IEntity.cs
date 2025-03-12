namespace MyLibrary.Server.Data.Entities
{
    public interface IEntity<TId> where TId : IEquatable<TId>
    {
        public TId Id { get; }
    }
}
