using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.DTOs.Interfaces
{
    public interface IDTO<TEntity, TId>
        where TEntity : class, IEntity<TId> 
        where TId : IEquatable<TId>
    {

    }
}
