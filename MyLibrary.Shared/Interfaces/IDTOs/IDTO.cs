using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Shared.Interfaces.IDTOs
{
    public interface IDTO<TEntity, TId>
        where TEntity : class, IEntity<TId> 
        where TId : IEquatable<TId>
    {

    }
}
