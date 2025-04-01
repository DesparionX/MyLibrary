using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface IDTO<TEntity, TId>
        where TEntity : class, IEntity<TId> 
        where TId : IEquatable<TId>
    {

    }
}
