using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface IOperationDTO<TEntity, TId> : IDTO<IOperation<TId>, TId>, IOperation<TId>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {

    }
    public interface IOperationDTO : IDTO<IOperation<int>, int>, IOperation<int>
    {

    }
}
