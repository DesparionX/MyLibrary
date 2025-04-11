using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.DTOs.Interfaces
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
