namespace MyLibrary.Server.Data.Entities
{
    public interface IBook<TId> : IEntity<TId>, IReadable, ISellable where TId : IEquatable<TId>
    {

    }
}
