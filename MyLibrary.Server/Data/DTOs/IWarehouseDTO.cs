using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface IWarehouseDTO<TEntity,TId> : IDTO<IWarehouse<TId>,TId>, IWarehouse<TId>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {

    }
    public interface IWarehouseDTO : IDTO<IWarehouse<int>, int>, IWarehouse<int>
    {

    }
}
