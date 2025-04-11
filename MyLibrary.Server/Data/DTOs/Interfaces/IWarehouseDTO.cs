using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.DTOs.Interfaces
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
