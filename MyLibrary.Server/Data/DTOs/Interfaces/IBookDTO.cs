using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.DTOs.Interfaces
{
    // Full generic interface definition for IBookDTO
    public interface IBookDTO<TEntity,TId> : IDTO<TEntity, TId>, IBook<TId>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {

    }

    // Partial generic interface with fixed ID type.
    public interface IBookDTO<TEntity> : IDTO<TEntity,Guid>, IBook<Guid>
        where TEntity : class, IEntity<Guid>
    {

    }

    // Fixed interface implementing IBook with fixed ID type.
    public interface IBookDTO : IDTO<IBook<Guid>, Guid>, IBook<Guid>
    {
    }
}
