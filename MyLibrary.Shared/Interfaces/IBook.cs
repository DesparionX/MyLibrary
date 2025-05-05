namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface IBook<TId> : IEntity<TId>, IReadable, ISellable where TId : IEquatable<TId>
    {

    }
}
