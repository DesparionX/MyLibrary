using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface IWarehouseDTO<TId> : IDTO<TId, IWarehouse<TId>>, IWarehouse<TId> where TId : IEquatable<TId>
    {

    }
    public interface IWarehouseDTO : IDTO<int, IWarehouse<int>>, IWarehouse<int>
    {

    }
}
