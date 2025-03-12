using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface IDTO<TId, TEntity>
        where TId : IEquatable<TId>
        where TEntity : class, IEntity<TId>
    {
        public TEntity Entity { get; }
    }
}
